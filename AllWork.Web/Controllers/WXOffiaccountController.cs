using AllWork.Web.Helper.Redis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AllWork.Web.Helper;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 微信公众平台
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WXOffiaccountController : ControllerBase
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly string _appid = "wxc0b3fbfff8a06786";
        readonly string _token_key = "offiaccount_tokenkey";
        readonly string _ticket_key = "offiaccount_tokenkey";
        readonly string _appSecret = "7dd75e68a18e48518f85485eadb609fc";


        public WXOffiaccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 获取公众号全局唯一接口调用凭据access_token
        /// (注意：要在公众号设定服务器IP到IP白名单才可以成功调用)
        /// </summary>
        /// <returns></returns>
        async Task<IActionResult> GetAccess_token()
        {
            var token = RedisClient.redisClient.GetStringKey(_token_key);
            if (string.IsNullOrWhiteSpace(token))
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appid, _appSecret);
                var result = await client.GetStringAsync(url);
                dynamic resultObject = JsonConvert.DeserializeObject(result);

                if (result.IndexOf("access_token") != -1)
                {
                    token = resultObject.access_token;
                    int secondes = resultObject.expires_in;
                    RedisClient.redisClient.SetStringKey(_token_key, token, new TimeSpan(0, 0, secondes));
                    return Ok(token);
                }
                else
                {
                    return BadRequest(resultObject.errmsg);
                }
            }
            else
            {
                return Ok(token);
            }
        }

        /// <summary>
        /// 获取调用微信JS接口的临时票据（必须在自己的服务全局缓存jsapi_ticket）
        /// </summary>
        /// <returns></returns>
        async Task<IActionResult> GetJsapi_ticket()
        {
            var ticket = RedisClient.redisClient.GetStringKey(_ticket_key);//先从缓存读取票据
            if (string.IsNullOrWhiteSpace(ticket))
            {
                var tokenResult = GetAccess_token().Result as ObjectResult;//调用获取token的接口
                if (tokenResult.StatusCode == 200)
                {
                    var client = _httpClientFactory.CreateClient();
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", tokenResult.Value);
                    var result = await client.GetStringAsync(url);
                    dynamic resultObject = JsonConvert.DeserializeObject(result);

                    if (resultObject.errcode == 0)
                    {
                        ticket = resultObject.ticket;
                        int secondes = resultObject.expires_in;
                        RedisClient.redisClient.SetStringKey(_ticket_key, ticket, new TimeSpan(0, 0, secondes));
                        return Ok(ticket);
                    }
                    else
                    {
                        return BadRequest(resultObject.errmsg);
                    }
                }
                else
                {
                    return BadRequest(tokenResult.Value);
                }
            }
            else
            {
                return Ok(ticket);
            }
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="url">调用JS接口页面的完整URL</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSignature(string url)
        {
            var ticket = RedisClient.redisClient.GetStringKey(_ticket_key);//先从缓存读取票据
            if (string.IsNullOrWhiteSpace(ticket))
            {
                var ticketResult = GetJsapi_ticket().Result as ObjectResult;
                if (ticketResult.StatusCode == 200)
                {
                    ticket = ticketResult.Value.ToString();
                }
                else
                {
                    return BadRequest(ticketResult.Value);
                }
            }
            try
            {
                var string1Builder = new StringBuilder();
                var _timestamp = Util.CreateTimestamp();
                var _noncestr = Util.CreateNonce_str();
                string1Builder.Append("jsapi_ticket=").Append(ticket).Append("&")
                              .Append("noncestr=").Append(_noncestr).Append("&")
                              .Append("timestamp=").Append(_timestamp).Append("&")
                              .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
                var result = string1Builder.ToString();

                return Ok(new
                {
                    appId = _appid,
                    timestamp = _timestamp,
                    noncestr = _noncestr,
                    signature = Util.Sha1(result),
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
