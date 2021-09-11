using AllWork.IServices.Order;
using AllWork.Model.RequestParams;
using AllWork.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 微信APP支付(基于V2版)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        readonly string _appid = PayHelper.AppId; //app应用的id
        readonly string __mchid = PayHelper.MchId; //直接商户号
        readonly string _notify_url = PayHelper.NotifyUrl;//通知地址(不允许携带查询串)
        readonly string _refundNotify_url = PayHelper.RefudNotifyUrl; //退款结果通知
        readonly string _ipaddress = PayHelper.IpAddr;//调用微信支付API的机器IP

        readonly IHttpClientFactory _httpClientFactory;
        readonly IOrderServices _orderServices;

        public PaymentController(IHttpClientFactory httpClientFactory, IOrderServices orderServices)
        {
            _httpClientFactory = httpClientFactory;
            _orderServices = orderServices;
        }

        /// <summary>
        /// 统一下单(App获取预支付交易会话标识)
        /// </summary>
        /// <param name="atp"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UnifiedOrder(AppTransactionsParams atp)
        {
            var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            var body = $"盛天商城-{atp.GoodsName}";
            var nonce_str = PayHelper.GetRandomString(30);
            //以下签名必须按照官方签名算法的要求进行：按参数顺序排列，非空参数不参与进来，参数区分大小写，url参数键值对的形式（即key1=value1&key2=value2…）
            //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
            SortedDictionary<string, object> dictData = new SortedDictionary<string, object>
            {
                {"appid", _appid },
                {"body", body },
                {"mch_id", __mchid },
                { "nonce_str", nonce_str},
                { "notify_url",_notify_url},
                {"out_trade_no", atp.OrderId },
                {"spbill_create_ip",_ipaddress },
                {"total_fee", atp.OrderAmount },
                {"trade_type", "APP" }
            };
            //转url格式
            string str = PayHelper.ToUrl(dictData);
            //在string后加入KEY（ key不参与参数排序，放在最后）
            str += "&key=" + PayHelper.Key;
            string strMD5 = PayHelper.MD5(str).ToUpper();//MD5签名

            //上面的签名是为了下面参数所需
            var formData = "<xml>";
            formData += "<appid>" + _appid + "</appid>";//appid
            formData += "<body>" + body + "</body>";//商品描述
            formData += "<mch_id>" + PayHelper.MchId + "</mch_id>";//商户号
            formData += "<nonce_str>" + nonce_str + "</nonce_str>";//随机字符串，不长于32位。
            formData += "<notify_url>" + _notify_url + "</notify_url>";//通知地址
            formData += "<out_trade_no>" + atp.OrderId + "</out_trade_no>";//商户订单号    --待
            formData += "<spbill_create_ip>" + _ipaddress + "</spbill_create_ip>";//终端IP  --用户ip
            formData += "<total_fee>" + atp.OrderAmount + "</total_fee>";//支付金额单位为（分）
            formData += "<trade_type>APP</trade_type>";//交易类型
            formData += "<sign>" + strMD5 + "</sign>"; //签名
            formData += "</xml>";

            //请求数据
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("method", "POST");
            StringContent content = new StringContent(formData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();

            //获取xml数据
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(res);
            //xml格式转json
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

            JObject jo = (JObject)JsonConvert.DeserializeObject(json);
            string prepay_id = jo["xml"]["prepay_id"]["#cdata-section"].ToString();
            string _time = PayHelper.GetTime().ToString(); //时间戳
            //再次签名返回数据至客户端
            SortedDictionary<string, object> dictB = new SortedDictionary<string, object> {
                {"appid", _appid },
                {"partnerid",__mchid },
                {"noncestr", nonce_str},//参数名
                {"prepayid",prepay_id},
                {"package", "Sign=WXPay"},
                {"timestamp", _time }
            };
            string strB = PayHelper.ToUrl(dictB);
            strB += "&key=" + PayHelper.Key; //key不参与参数排序，放在最后
            var wx = new
            {
                timeStamp = _time,
                nonceStr = nonce_str,
                package = prepay_id,
                paySign = PayHelper.MD5(strB).ToUpper(),
                signType = "MD5"
            };
            return Ok(wx);
        }

        /// <summary>
        /// 支付结果通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> OrderNotify()
        {

            string strResult;
            var strXML = string.Empty;
            try
            {
                //接收从微信后台POST过来的数据
                var request = this.HttpContext.Request;
                request.EnableBuffering();
                var stream = this.HttpContext.Request.Body;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    strXML = reader.ReadToEndAsync().Result;
                    request.Body.Position = 0;
                }

                //推荐的做法是，当收到通知进行处理时，首先检查对应业务数据的状态，判断该通知是否已经处理过，如果没有处理过再进行处理，
                //如果处理过直接返回结果成功。在对业务数据进行状态检查和处理之前，要采用数据锁进行并发控制，以避免函数重入造成的数据混乱。
                //判断请求是否成功
                if (PayHelper.GetXmlValue(strXML, "return_code") == "SUCCESS")
                {
                    //判断支付是否成功
                    if (PayHelper.GetXmlValue(strXML, "result_code") == "SUCCESS")
                    {
                        var dictData = PayHelper.GetFromXml(strXML);
                        var appid = PayHelper.GetXmlValue(strXML, "appid");
                        var returnSign = PayHelper.GetXmlValue(strXML, "sign"); //取得签名
                        var sign = PayHelper.GetSignInfo(dictData);// PayHelper.MakeSign(dictData);这个签名也一样的状况
                        //未解之谜 ：微信服务器会连续发7个通知，只有最后一个通知的签名才对 
                        //对比签名
                        if (sign == returnSign)
                        {
                            //校验订单信息
                            string wxOrderNum = PayHelper.GetXmlValue(strXML, "transaction_id"); //微信订单号
                            string orderNum = PayHelper.GetXmlValue(strXML, "out_trade_no");    //商户订单号
                            string orderTotal = PayHelper.GetXmlValue(strXML, "total_fee");
                            string openid = PayHelper.GetXmlValue(strXML, "openid");
                            //查询微信订单
                            var resObj = await OrderQuery(appid, wxOrderNum, "");
                            string resStr = JsonConvert.SerializeObject(resObj);
                            if (resStr.Contains("transaction_id"))
                            {
                                strResult = PayHelper.GetReturnXml("SUCCESS", "OK");
                                //2.更新商户订单的相关状态
                                await _orderServices.PaySuccess(orderNum, wxOrderNum);
                                Common.Mail.SendMail("Pay Success", $"支付成功！订单号：{orderNum} 微信订单号：{wxOrderNum} 金额：{orderTotal}");
                            }
                            else
                            {
                                strResult = PayHelper.GetReturnXml("FAIL", "支付结果中微信订单号数据不存在！");
                            }
                        }
                        else
                        {
                            strResult = PayHelper.GetReturnXml("FAIL", "签名不一致: 新签名" + sign + " 原签名：" + returnSign);
                        }
                    }
                    else
                    {
                        strResult = PayHelper.GetReturnXml("FAIL", "支付通知失败!");
                    }
                }
                else
                {
                    strResult = PayHelper.GetReturnXml("FAIL", "支付通知失败!");
                }
            }
            catch (WebException ex)
            {
                strResult = ex.Message;
                Common.Mail.SendMail("Pay Notifier", ex.Message);
            }
            //最终回应给微信服务器
            return strResult;
        }

        #region 订单查询
        /// <summary>
        /// 查询订单（至少为其中一个参数提供值)
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="tranId">微信订单号</param>
        /// <param name="orderId">商户订单号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OrderQuery(string appId, string tranId, string orderId)
        {
            if (string.IsNullOrWhiteSpace(tranId) && string.IsNullOrWhiteSpace(orderId))
            {
                return Ok(JsonConvert.SerializeObject(new { result_code = "FAIL", err_code_des = "至少必须为微信订单号和商户订单号其中一个参数提供非空值" }));
            }
            var url = "https://api.mch.weixin.qq.com/pay/orderquery";
            var nonce_str = PayHelper.GetRandomString(30);
            SortedDictionary<string, object> dictData = new SortedDictionary<string, object>
            {
                { "appid", _appid},{ "mch_id", __mchid},{"nonce_str", nonce_str}
            };
            //优先微信订单号
            if (!string.IsNullOrWhiteSpace(tranId))
                dictData.Add("transaction_id", tranId);
            else
                dictData.Add("out_trade_no", orderId);
            var str = PayHelper.ToUrl(dictData);
            str += "&key=" + PayHelper.Key;
            var strMD5 = PayHelper.MD5(str).ToUpper(); //得到签名(以上不包括sign)

            var formData = "<xml>";
            formData += "<appid>" + appId + "</appid>";
            formData += "<mch_id>" + PayHelper.MchId + "</mch_id>";
            if (!string.IsNullOrWhiteSpace(tranId))
                formData += "<transaction_id>" + tranId + "</transaction_id>";
            else
                formData += "<out_trade_no>" + orderId + "</out_trade_no>";
            formData += "<nonce_str>" + nonce_str + "</nonce_str>";
            formData += "<sign>" + strMD5 + "</sign>"; //签名
            formData += "</xml>";
            //发送post请求
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("method", "POST");
            StringContent content = new StringContent(formData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            return Ok(PayHelper.GetFromXml(res));
        }
        #endregion


        #region 申请退款
        /// <summary>
        /// 退款申请
        /// </summary>
        /// <param name="refundApplyParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ApplyRefund(RefundApplyParams refundApplyParams)
        {
            var url = "https://api.mch.weixin.qq.com/secapi/pay/refund";//申请退款接口地址
            var nonce_str = PayHelper.GetRandomString(30); //随机字符串
            //签名串：严格按官方要求（按照参数名ASCII码从小到大排序顺序、大小写、url键值对形式等）
            SortedDictionary<string, object> dictData = new SortedDictionary<string, object>
            {
                { "appid",refundApplyParams.AppId},
                {"mch_id", __mchid },
                {"nonce_str", nonce_str },
                {"out_refund_no", refundApplyParams.RefundId },
                {"out_trade_no", refundApplyParams.OrderId },
                {"refund_fee", refundApplyParams.RefundFee },
                {"total_fee", refundApplyParams.TotalFee },
                {"notify_url", _refundNotify_url },
            };
            var str = PayHelper.ToUrl(dictData);
            str += "&key=" + PayHelper.Key;
            var signValue = PayHelper.MD5(str).ToUpper();//获得签名值
            //要发送的数据
            var formData = "<xml>";
            formData += "<appid>" + refundApplyParams.AppId + "</appid>";
            formData += "<mch_id>" + __mchid + "</mch_id>";
            formData += "<nonce_str>" + nonce_str + "</nonce_str>";
            formData += "<sign>" + signValue + "</sign>";
            formData += "<out_trade_no>" + refundApplyParams.OrderId + "</out_trade_no>";
            formData += "<out_refund_no>" + refundApplyParams.RefundId + "</out_refund_no>";
            formData += "<total_fee>" + refundApplyParams.TotalFee + "</total_fee>";
            formData += "<refund_fee>" + refundApplyParams.RefundFee + "</refund_fee>";
            formData += "<notify_url>" + _refundNotify_url + "</notify_url>";
            formData += "</xml>";

            //以安全证书发起退款请求(注：配置文件要用记事本另存为utf8格式，否则申请退款会报系统找不到指定路径的错误）
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (x, y, z, m) => true,
            };
            handler.ClientCertificates.Add(new X509Certificate2(PayHelper.CertPath, PayHelper.CertPassword));//安全证书绝对路径，密码（默认为商户密钥）
            var client = new HttpClient(handler);
            var content = new StringContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            return Ok(res);
        }
        #endregion

        #region 退款结果通知
        /// <summary>
        /// 退款结果通知：不用客户端调用，退款后微信服务器发送通知给此接口，然后商户在此接口进行一些处理. 要外网能访问，不能带参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> RefundNotifyUrl()
        {
            var strResult = string.Empty;
            var sourceStr = string.Empty;
            var strXml = string.Empty;
            try
            {
                //接收从微信后台POST过来的数据
                var request = this.HttpContext.Request;
                request.EnableBuffering();
                var stream = this.HttpContext.Request.Body;
                long? length = this.HttpContext.Request.ContentLength;
                if (length != null && length > 0)
                {
                    // 使用这个方式读取，并且使用异步
                    StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                    sourceStr = await streamReader.ReadToEndAsync();
                }
                this.HttpContext.Request.Body.Position = 0;

                var req_info = PayHelper.GetXmlValue(sourceStr, "req_info");

                //官方特别说明：退款结果对重要的数据进行了加密，商户需要用商户秘钥进行解密后才能获得结果通知的内容
                var key = PayHelper.MD5(PayHelper.Key);//(2)对商户key做md5，得到32位小写key*
                strXml = PayHelper.DecodeAES256ECB(req_info, key); //用key*对加密串B做AES-256-ECB解密（PKCS7Padding）
                var dictResult = PayHelper.GetFromXml(strXml);

                //此处更新商户退款单状态及商户订单状态（如果申请退款则订单状变更为取消; 如果是退货则取消是取消订单而是生成其他入库，收到后再审
                if (dictResult["refund_status"].ToString().ToUpper().Equals("SUCCESS")) //退款成功
                {
                    var orderId = dictResult["out_trade_no"].ToString();//商户系统内部的订单号
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        var res = await _orderServices.CancelOrder(long.Parse(orderId));
                        if (res.Status)
                        {
                            //此处给微信服务器应答(应答后微信将不再送消息)
                            strResult = PayHelper.GetReturnXml("SUCCESS", "OK");
                            Common.Mail.SendMail($"{orderId}退款通知", strXml);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResult = PayHelper.GetReturnXml("FAIL", "退款通知失败!" + ex.Message);
                Common.Mail.SendMail("退款异常", strXml);
            }
            return strResult;
        }
        #endregion

        #region 退款查询
        /// <summary>
        /// 查询退款
        /// </summary>
        /// <param name="appId">appId</param>
        /// <param name="refundId">商户退款单号(如果单个支付订单部分退款次数超过20次请使用退款单号查询)</param>
        /// <param name="orderId">商户订单号</param>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<IActionResult> RefundQuery(string appId, string refundId, string orderId)
        {
            if (string.IsNullOrWhiteSpace(refundId) && string.IsNullOrWhiteSpace(orderId))
            {
                return Ok(JsonConvert.SerializeObject(new { result_code = "FAIL", err_code_des = "至少必须为商户退款单号和商户订单号其中一个参数提供非空值" }));
                //官方接口提供了4选1查询，若有必要再扩展
            }
            var url = "https://api.mch.weixin.qq.com/pay/refundquery";
            var nonce_str = PayHelper.GetRandomString(30);
            SortedDictionary<string, object> dictData = new SortedDictionary<string, object> {
                { "appid", appId },
                { "mch_id", __mchid},
                { "nonce_str", nonce_str } };
            if (!string.IsNullOrWhiteSpace(refundId))
                dictData.Add("refund_id", refundId);
            else
                dictData.Add("out_trade_no", orderId);
            var str = PayHelper.ToUrl(dictData);
            str += "&key=" + PayHelper.Key;
            var strMD5 = PayHelper.MD5(str).ToUpper(); //得到签名(以上不包括sign)

            var formData = "<xml>";
            formData += "<appid>" + appId + "</appid>";
            formData += "<mch_id>" + __mchid + "</mch_id>";
            if (!string.IsNullOrWhiteSpace(refundId))
                formData += "<refund_id>" + refundId + "</refund_id>";
            else
                formData += "<out_trade_no>" + orderId + "</out_trade_no>";
            formData += "<nonce_str>" + nonce_str + "</nonce_str>";
            formData += "<sign>" + strMD5 + "</sign>"; //签名
            formData += "</xml>";

            //请求数据
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("method", "POST");
            StringContent content = new StringContent(formData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();

            return Ok(PayHelper.GetFromXml(res));
        }
        #endregion
    }
}
