using AllWork.Web.Helper.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 物流助手
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogisticsController : ControllerBase
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly string _key_shop = "mp_shop_token";//商城小程序缓存键
        readonly string _appid_shop = "wxe6ead5b837107111";//商城小程序appid
        readonly string _secret_shop = "076b95c6aa288460980cc303d5fd741b";//商城小程序secret

        public LogisticsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据（access_token）。调用绝大多数后台接口时都需使用 access_token，开发者需要进行妥善保存。
        /// </summary>
        /// <param name="cachekey">缓存token的key</param>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        async Task<IActionResult> getAccessToken(string cachekey, string appid, string secret)
        {
            var token = RedisClient.redisClient.GetStringKey(cachekey);
            //如果缓存没有token
            if (string.IsNullOrEmpty(token))
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
                var result = await client.GetStringAsync(url);
                dynamic resultObject = JsonConvert.DeserializeObject(result);
                if (string.IsNullOrEmpty(resultObject.errcode))
                {
                    token = resultObject.access_token;
                    int seconds = resultObject.expires_in;//凭证的有效时间（秒）
                    //缓存token, 有效期seconds
                    RedisClient.redisClient.SetStringKey(cachekey, token, new TimeSpan(0, 0, seconds));
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
        /// 获取支持的快递公司列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getAllDelivery()
        {
            var result = getAccessToken(_key_shop, _appid_shop, _secret_shop).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/express/business/delivery/getall?access_token={0}", result.Value);
                var response = await client.GetStringAsync(url);
                return Ok(response);
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 配置面单打印员（可以设置多个，若需要使用微信打单 PC 软件，才需要调用）
        /// </summary>
        /// <param name="openid">打印员 openid</param>
        /// <param name="update_type">更新类型（合法值有bind，unbind）</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> updatePrinter(string openid, string update_type)
        {
            var result = getAccessToken(_key_shop, _appid_shop, _secret_shop).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("method", "post");
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/express/business/printer/update?access_token={0}", result.Value);

                var requestData = new
                {
                    access_token = result.Value,
                    openid = openid,
                    update_type = update_type
                };
                var httpContent = new JsonContent(requestData);

                try
                {
                    var response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync().Result;
                    return Ok(res);
                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
