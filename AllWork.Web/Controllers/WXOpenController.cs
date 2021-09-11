using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 微信开放平台网站应用
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WXOpenController : ControllerBase
    {
        //建议将secret、用户数据（如access_token）放在App云端服务器，由云端中转接口调用请求
        //以下用于过网站应用的登录等

        readonly string openPlatformAppId;//应用唯一标识，在微信开放平台提交应用审核通过后获得
        readonly string openPlatformAppSecret;//应用密钥AppSecret，在微信开放平台提交应用审核通过后获得
        readonly IHttpClientFactory _httpClientFactory;
        readonly IConfiguration _configuration;

        public WXOpenController(IHttpClientFactory clientFactory,IConfiguration configuration)
        {
            _httpClientFactory = clientFactory;
            _configuration = configuration;
            openPlatformAppId = _configuration.GetSection("MP:AppId").Value;
            openPlatformAppSecret = _configuration.GetSection("MP:AppSecret").Value;
        }

        /// <summary>
        /// 获取接口调用凭证access_token (返回包括access_token、expires_in、refresh_token、openid、scope、unionid）
        /// </summary>
        /// <param name="code">填写第一步获取的code参数（一个code只能成功换取一次access_token即失效）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAccess_token(string code)
        {
            var client = _httpClientFactory.CreateClient();
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                openPlatformAppId, openPlatformAppSecret, code);
            var result = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject(result);
            dynamic resultObject = res;
            if (resultObject.errcode == 40029 || resultObject.errcode == 40163)
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }

        /// <summary>
        /// 刷新access_token有效期(返回包括access_token、expires_in、refresh_token、openid、scope）
        /// </summary>
        /// <param name="refresh_token">填写通过access_token获取到的refresh_token参数</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshAccess_token(string refresh_token)
        {
            var client = _httpClientFactory.CreateClient();
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                openPlatformAppId, refresh_token);
            var result = await client.GetStringAsync(url);
            dynamic resultObject = JsonConvert.DeserializeObject(result);
            if (resultObject.errcode == 40030)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

        /// <summary>
        /// 获取用户个人信息（UnionID机制）
        /// </summary>
        /// <param name="access_token">调用凭证</param>
        /// <param name="openid">普通用户的标识，对当前开发者帐号唯一</param>
        /// <param name="lang">国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语，默认为zh-CN</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string access_token, string openid, string lang = "zh_CN")
        {
            var client = _httpClientFactory.CreateClient();
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}",
                access_token, openid, lang);
            var result = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject(result);
            dynamic resultObject = res;
            if (resultObject.errcode == 40003)
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }

        /// <summary>
        /// 检验授权凭证（access_token）是否有效
        /// </summary>
        /// <param name="access_token">调用接口凭证</param>
        /// <param name="openid">普通用户标识，对该公众帐号唯一</param>
        /// <returns>errcode为0表示有效</returns>
        [HttpGet]
        public async Task<IActionResult> AuthAccess_token(string access_token, string openid)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}",
                    access_token, openid);
                var result = await client.GetStringAsync(url);
                var res = JsonConvert.DeserializeObject(result);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
