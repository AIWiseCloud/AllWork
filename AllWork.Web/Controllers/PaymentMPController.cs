using AllWork.Model.RequestParams;
using AllWork.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 微信小程序支付(基于V2版)
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentMPController : ControllerBase
    {
        readonly string _appid = PayHelper.AppId_mp; //小程序应用的id
        readonly string __mchid = PayHelper.MchId; //直接商户号
        readonly string _notify_url = PayHelper.NotifyUrl;//通知地址(不允许携带查询串)
        readonly string _ipaddress = PayHelper.IpAddr;//调用微信支付API的机器IP

        readonly IHttpClientFactory _httpClientFactory;

        public PaymentMPController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 统一下单(获取预支付交易会话标识)
        /// </summary>
        /// <param name="atp"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UnifiedOrder(MPTransactionsParams atp)
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
                {"openid",atp.OpenId },
                {"out_trade_no", atp.OrderId },
                {"spbill_create_ip",_ipaddress },
                {"total_fee", atp.OrderAmount },
                {"trade_type", "JSAPI" }
            };
            //转url格式
            string str = PayHelper.ToUrl(dictData);
            //在string后加入KEY（ key不参与参数排序，放在最后）
            str += "&key=" + PayHelper.Key;
            string strMD5 = PayHelper.MD5(str).ToUpper();//MD5签名

            //上面的签名是为了下面sign参数所需
            var formData = "<xml>";
            formData += "<appid>" + _appid + "</appid>";//appid
            formData += "<body>" + body + "</body>";//商品描述
            formData += "<mch_id>" + PayHelper.MchId + "</mch_id>";//商户号
            formData += "<nonce_str>" + nonce_str + "</nonce_str>";//随机字符串，不长于32位。
            formData += "<notify_url>" + _notify_url + "</notify_url>";//通知地址
            formData += "<openid>" + atp.OpenId + "</openid>";//用户标识
            formData += "<out_trade_no>" + atp.OrderId + "</out_trade_no>";//商户订单号    --待
            formData += "<spbill_create_ip>" + _ipaddress + "</spbill_create_ip>";//终端IP  --用户ip
            formData += "<total_fee>" + atp.OrderAmount + "</total_fee>";//支付金额单位为（分）
            formData += "<trade_type>JSAPI</trade_type>";//交易类型
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
            //若失败，返回错误描述
            if(jo["xml"]["result_code"]["#cdata-section"].ToString() == "FAIL")
            {
                return BadRequest(jo["xml"]["err_code_des"]["#cdata-section"].ToString());
            }

            string prepay_id = jo["xml"]["prepay_id"]["#cdata-section"].ToString();
            string _time = PayHelper.GetTime().ToString(); //时间戳
            //再次签名返回数据至客户端  (这里一定要注意大小写，与官方的一致，而且小程序与app中的大小写不一致，导致签名无效）
            SortedDictionary<string, object> dictB = new SortedDictionary<string, object> {
                {"appId", _appid },
                {"nonceStr", nonce_str},//参数名
                {"package=prepay_id",prepay_id}, //与app不同
                {"signType", "MD5" },//与app不同
                {"timeStamp", _time }
            };
            string strB = PayHelper.ToUrl(dictB);
            strB += "&key=" + PayHelper.Key; //key不参与参数排序，放在最后
            var wx = new
            {
                timeStamp = _time,
                nonceStr = nonce_str,
                package = "prepay_id=" + prepay_id,//prepay_id,
                paySign = PayHelper.MD5(strB).ToUpper(),
                signType = "MD5"
            };
            return Ok(wx);
        }

    
    }
}
