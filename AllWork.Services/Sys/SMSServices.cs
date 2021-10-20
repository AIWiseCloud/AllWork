using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using AllWork.Common;
using AllWork.IServices.Sys;
using AllWork.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    /// <summary>
    /// 阿里云短信（直接参照官网示例及API说明进行)
    /// </summary>
    public class SMSServices : ISMSServices
    {
        readonly string _accessKeyId;
        readonly string _accessKeySecret;
        readonly IConfiguration _configuration;
        readonly Client _client;

        public SMSServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKeyId = DesEncrypt.Decrypt(_configuration.GetSection("SMS:AccessKeyId").Value);
            _accessKeySecret = DesEncrypt.Decrypt(_configuration.GetSection("SMS:AccessKeySecret").Value);


            //使用AK&SK初始化账号Client
            Config config = new Config
            {
                AccessKeyId = _accessKeyId,
                AccessKeySecret = _accessKeySecret,
                Endpoint = "dysmsapi.aliyuncs.com",
            };
            _client = new Client(config);

        }

        /// <summary>
        /// 新订单提醒
        /// </summary>
        /// <param name="phoneNumbers">接收短信的手机号码: 支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔。上限为1000个手机号</param>
        /// <param name="receiver">收货人</param>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        public async Task<OperResult> SendOrderMsg(string phoneNumbers, string receiver, string orderId)
        {
            var result = new OperResult { Status = false };
            if (string.IsNullOrWhiteSpace(phoneNumbers))
            {
                result.ErrorMsg = "请提供手机号（多个手机号以逗号分开)";
            }
            else
            {
                SendSmsRequest request = new SendSmsRequest
                {
                    PhoneNumbers = phoneNumbers,
                    TemplateCode = "SMS_226530151",
                    TemplateParam = JsonConvert.SerializeObject(new { consignee = receiver, order = orderId }),
                    SignName = "盛天商城订单", //短信签名名称：必须是已添加、并通过审核的短信签名
                };
                var res = await _client.SendSmsAsync(request);
                if (!string.IsNullOrWhiteSpace(res.Body.Code) && res.Body.Code == "OK")
                {
                    result.Status = true;
                }
                else
                {
                    result.ErrorMsg = res.Body.Message;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="unionId"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<OperResult> GetVerifyCode(string unionId, string phoneNumber)
        {
            var result = new OperResult { Status = false };
            if (string.IsNullOrWhiteSpace(unionId) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                result.ErrorMsg = "请求参数不完整";
            }
            else if (unionId.Length != 28)
            {
                result.ErrorMsg = "UnionId不正确";
            }
            else if (!AllWork.Common.Utils.IsMobilePhone(phoneNumber))
            {
                result.ErrorMsg = $"{phoneNumber}不是正确的手机号";
            }
            else
            {
                var code = AllWork.Common.Utils.GetRandomNum(100000, 999999);
                SendSmsRequest request = new SendSmsRequest
                {
                    PhoneNumbers = phoneNumber,
                    TemplateCode = "SMS_214835012",
                    TemplateParam = JsonConvert.SerializeObject(new { code }),
                    SignName = "盛天商城", //短信签名名称：必须是已添加、并通过审核的短信签名
                };
                var res = await _client.SendSmsAsync(request);
                if (!string.IsNullOrWhiteSpace(res.Body.Code) && res.Body.Code == "OK")
                {
                    result.Status = true;
                    result.IdentityKey = code.ToString();
                    //获取验证码成功，缓存60秒  (现只能在控制器中缓存，这个要作修改)
                    //RedisClient.redisClient.SetStringKey(unionId, $"{phoneNumber},{code}", new TimeSpan(0, 0, 60));
                }
                else
                {
                    result.ErrorMsg = res.Body.Message;
                }
            }

            return result;
        }

        public async Task<OperResult> SendMSG(string phoneNumber,string templateCode,string templateParam,string signName)
        {
            var result = new OperResult { Status = false };
            SendSmsRequest request = new SendSmsRequest
            {
                PhoneNumbers = phoneNumber,
                TemplateCode = templateCode,
                TemplateParam = templateParam,
                SignName = signName, //短信签名名称：必须是已添加、并通过审核的短信签名
            };
            var res = await _client.SendSmsAsync(request);
            if (!string.IsNullOrWhiteSpace(res.Body.Code) && res.Body.Code == "OK")
            {
                result.Status = true;
            }
            else
            {
                result.ErrorMsg = res.Body.Message;
            }
            return result;
        }
    }
}
