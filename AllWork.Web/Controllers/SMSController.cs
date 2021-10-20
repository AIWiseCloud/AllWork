using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Web.Helper.Redis;
using Microsoft.AspNetCore.Mvc;
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

        readonly IUserServices _userServices;
        readonly ISMSServices _smsServices;

        public SMSController(IUserServices userServices, ISMSServices sMSServices)
        {

            _userServices = userServices;
            _smsServices = sMSServices;
        }

        /// <summary>
        /// 获取验证码（有效期60秒）
        /// </summary>
        /// <param name="unionId">UnionId</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetVerifyCode(string unionId, string phoneNumber)
        {
            var res = await _smsServices.GetVerifyCode(unionId, phoneNumber);
            if (res.Status)
            {
                //获取验证码成功，缓存60秒
                RedisClient.redisClient.SetStringKey(unionId, $"{phoneNumber},{res.IdentityKey}", new TimeSpan(0, 0, 60));
            }
            return Ok(res);
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
                var res = await _smsServices.SendMSG(userInfo.PhoneNumber, "SMS_225369583", JsonConvert.SerializeObject(new { code = userInfo.UserName, password = pw }), "盛天商城账号");
                result = res;

            }
            return Ok(result);
        }


    }
}
