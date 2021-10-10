using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using AllWork.Common;
using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Web.Helper.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 短信息服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        readonly string _accessKeyId;
        readonly string _accessKeySecret;
        readonly IConfiguration _configuration;
        //readonly List<TemplateItem> _allTemplates;
        readonly Client _client;
        readonly IUserServices _userServices;

        public SMSController(IConfiguration configuration, IUserServices userServices)
        {
            _configuration = configuration;
            _accessKeyId = DesEncrypt.Decrypt(_configuration.GetSection("SMS:AccessKeyId").Value);
            _accessKeySecret = DesEncrypt.Decrypt(_configuration.GetSection("SMS:AccessKeySecret").Value);
            _userServices = userServices;

            //使用AK&SK初始化账号Client
            Config config = new Config
            {
                AccessKeyId = _accessKeyId,
                AccessKeySecret = _accessKeySecret,
                Endpoint = "dysmsapi.aliyuncs.com",
            };
            _client = new Client(config);
            //_allTemplates = new List<TemplateItem>
            //{
            //    new TemplateItem{ Id="01",TemplateCode="SMS_214835012", TemplateName="身份验证验证码"},
            //    new TemplateItem{Id="07",TemplateCode="SMS_225369583", TemplateName="账号信息通知"},
            //    new TemplateItem{Id="02",TemplateCode="SMS_214835011", TemplateName="登录确认验证码"},
            //    new TemplateItem{Id="03",TemplateCode="SMS_214835010",  TemplateName="登录异常验证码"},
            //    new TemplateItem{Id="04",TemplateCode="SMS_214835009", TemplateName="用户注册验证码"},
            //    new TemplateItem{Id="05",TemplateCode="SMS_214835008", TemplateName="修改密码验证码"},
            //    new TemplateItem{Id="06",TemplateCode="SMS_214835007", TemplateName="信息变更验证码"}
            //};
        }
        /*
      class TemplateItem
      {
          public string Id { get; set; }
          public string TemplateCode { get; set; }
          public string TemplateName { get; set; }
      }

      /// <summary>
      /// 获取所有短信模板
      /// </summary>
      /// <returns></returns>
      [HttpGet]
      public IActionResult GetTemplates()
      {
          return Ok(_allTemplates);
      }


      /// <summary>
      /// 发送短信
      /// </summary>
      /// <param name="phoneNumbers">接收短信的手机号码: 支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔。上限为1000个手机号</param>
      /// <param name="id">模板项Id （可在获取所有短信模板接口查看）</param>
      /// <param name="templateParam">短信模板变量对应的实际值，JSON格式。示例值：{"code":"1111"}</param>
      /// <returns></returns>
      [HttpPost]
      public async Task<IActionResult> SendSms(string phoneNumbers, string id, string templateParam = "")
      {
          if (string.IsNullOrWhiteSpace(phoneNumbers))
          {
              return BadRequest("请提供手机号（多个手机号以逗号分开)");
          }
          var templateItem = _allTemplates.FindLast(x => x.Id == id);
          if (templateItem == null)
          {
              return BadRequest("清提供正确的短信模板Id");
          }
          SendSmsRequest request = new SendSmsRequest
          {
              PhoneNumbers = phoneNumbers,
              TemplateCode = templateItem.TemplateCode,
              TemplateParam = templateParam,
              SignName = "盛天商城", //短信签名名称：必须是已添加、并通过审核的短信签名
          };
          var res = await _client.SendSmsAsync(request);
          return Ok(res);
      }
      */

        /// <summary>
        /// 获取验证码（有效期1分钟）
        /// </summary>
        /// <param name="unionId">UnionId</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetVerifyCode(string unionId, string phoneNumber)
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
                return BadRequest($"{phoneNumber}不是正确的手机号");

            }
            else
            {
                var code = AllWork.Common.Utils.GetRandomNum(100000, 999999);
                SendSmsRequest request = new SendSmsRequest
                {
                    PhoneNumbers = phoneNumber,
                    TemplateCode = "SMS_214835012",
                    TemplateParam = JsonConvert.SerializeObject(new { code = code }),
                    SignName = "盛天商城", //短信签名名称：必须是已添加、并通过审核的短信签名
                };
                var res = await _client.SendSmsAsync(request);
                if (!string.IsNullOrWhiteSpace(res.Body.Code) && res.Body.Code == "OK")
                {
                    result.Status = true;
                    //获取验证码成功，缓存60秒
                    RedisClient.redisClient.SetStringKey(unionId, $"{phoneNumber},{code}", new TimeSpan(0, 0, 60));
                }
                else
                {
                    result.ErrorMsg = res.Body.Message;
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// 用户请求账号、密码
        /// </summary>
        /// <param name="unionId">UnionId</param>
        /// <param name="code">短信验证码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAccountPassword(string unionId, string code)
        {
            var userInfo = await _userServices.GetUserInfo(unionId);
            var value = RedisClient.redisClient.GetStringKey(unionId);
            var result = new OperResult { Status = false };
            if (value == null || value.Split(',').Length != 2)
            {
                result.ErrorMsg = "验证码不存在或已过期";
            }
            else if (string.IsNullOrWhiteSpace(userInfo.PhoneNumber))
            {
                result.ErrorMsg = "当前账号未绑定手机号";
            }
            else if (value.Split(',')[1] != code)
            {
                result.ErrorMsg = "验证码错误";
            }
            else
            {
                var pw = AllWork.Common.DesEncrypt.Decrypt(userInfo.Password);
                SendSmsRequest request = new SendSmsRequest
                {
                    PhoneNumbers = userInfo.PhoneNumber,
                    TemplateCode = "SMS_225369583",
                    TemplateParam = JsonConvert.SerializeObject(new { code = userInfo.UserName, password = pw }),
                    SignName = "盛天商城账号", //短信签名名称：必须是已添加、并通过审核的短信签名
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
            return Ok(result);
        }

        
    }
}
