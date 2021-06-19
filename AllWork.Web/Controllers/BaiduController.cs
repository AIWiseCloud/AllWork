using AllWork.Web.Helper.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using AllWork.Common;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 百度智能云
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaiduController : ControllerBase
    {
        readonly IHttpClientFactory _httpClientFactory;

        readonly string speech_ApiKey = "V9YdHAhGcG4o3x1KZfRyurhi";//百度云中开通对应服务应用的 API Key
        readonly string speech_SecretKey = "xGt6UqYw49Ug1yRGZTf6HcexPSOuh55x";//百度云中开通对应服务应用的 Secret Key
        readonly string speech_CacheKey = "speech_CacheKey";//accessToken缓存键

        readonly string ocr_ApiKey = "Kzyvlzd0Ubau9WovEP8zTbSv";//百度云中开通对应服务应用的 API Key
        readonly string ocr_SecretKey = "A2Uc2qHY09a684USezQW20ffPdN8pEHv";//百度云中开通对应服务应用的 Secret Key
        readonly string ocr_CacheKey = "ocr_CacheKey";//accessToken缓存键

        public BaiduController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 鉴权认证机制--获取Access Token
        /// </summary>
        /// <param name="apikey"></param>
        /// <param name="secretkey"></param>
        /// <param name="cachekey"></param>
        /// <returns></returns>
        async Task<IActionResult> getAccessToken(string apikey, string secretkey, string cachekey)
        {
            var token = RedisClient.redisClient.GetStringKey(cachekey);
            //如果缓存没有token
            if (string.IsNullOrEmpty(token))
            {
                var client = _httpClientFactory.CreateClient();
                var authHost = "https://aip.baidubce.com/oauth/2.0/token";
                List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>();
                paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                paraList.Add(new KeyValuePair<string, string>("client_id", apikey));
                paraList.Add(new KeyValuePair<string, string>("client_secret", secretkey));
                HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
                var result = await response.Content.ReadAsStringAsync();

                dynamic resultObject = JsonConvert.DeserializeObject(result);
                string access_token = resultObject.access_token;
                if (!string.IsNullOrEmpty(access_token))
                {
                    token = access_token;
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
        /// 短语音识别（支持pcm/wav/amr/m4a格式的语音文件）
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SpeechRecognition(IFormCollection formCollection)
        {
            var files = (IFormFileCollection)formCollection.Files;
            var stream = files[0].OpenReadStream();
            var extename= files[0].FileName.Substring(files[0].FileName.LastIndexOf('.')).Replace(".","");
            //注：开发者工具为aac， 小程序端为m4a
            if ("pcm/wav/amr/m4a".IndexOf(extename) == -1)
            {
                return BadRequest(string.Format("当前格式{0},只支持pcm/wav/amr/m4a格式",extename));
            }
            var result = getAccessToken(speech_ApiKey, speech_SecretKey, speech_CacheKey).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                    //语音数据和其他参数通过标准 JSON 格式串行化 POST 上传
                    var data = new
                    {
                        format = extename,     //语音文件的格式，pcm/wav/amr/m4a。不区分大小写。推荐pcm文件
                        rate = 16000,       //采样率，16000、8000，固定值
                        dev_pid = 1537,     //默认1537（普通话 输入法模型）
                        channel = 1,        //声道数，仅支持单声道，请填写固定值 1
                        token = result.Value,
                        cuid = Utils.GetMacByNetworkInterface(),//用户唯一标识，用来区分用户，计算UV值。建议填写能区分用户的机器 MAC 地址或 IMEI 码，长度为60字符以内。
                        len = stream.Length,                //len 字段表示原始语音大小字节数，不是 base64 编码之后的长度
                        speech = Utils.FileToBase64(stream)      //本地语音文件的二进制语音数据 ，需要进行base64 编码
                    };
                    var httpContent = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                    var url = "http://vop.baidu.com/server_api";
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("Method", "POST");
                    var response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync().Result;
                    return Ok(res);
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 名片识别
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BusinessCardRecognition(IFormCollection formCollection)
        {
            var result = getAccessToken(ocr_ApiKey, ocr_SecretKey, ocr_CacheKey).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                IFormFileCollection files = (IFormFileCollection)formCollection.Files;
                var url = "https://aip.baidubce.com/rest/2.0/ocr/v1/business_card?access_token=" + result.Value;
                var stream = files[0].OpenReadStream();//将文件转为流
                var str64 = Utils.FileToBase64(stream);//base64
                var str = "image=" + HttpUtility.UrlEncode(str64);//对字符串进行编码，以便于可靠的http传输
                var httpContent = new StringContent(str, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("Method", "POST");

                    var response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync().Result;
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> IdentityRecognition(IFormCollection formCollection)
        {
            var result = getAccessToken(ocr_ApiKey, ocr_SecretKey, ocr_CacheKey).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                IFormFileCollection files = (IFormFileCollection)formCollection.Files;
                var url = "https://aip.baidubce.com/rest/2.0/ocr/v1/idcard?access_token=" + result.Value;
                var stream = files[0].OpenReadStream();//将文件转为流
                var str64 = Utils.FileToBase64(stream);//base64
                //传front如果实际为back也可识别
                var str = "id_card_side=" + "front" + "&image=" + HttpUtility.UrlEncode(str64);//对字符串进行编码，以便于可靠的http传输
                var httpContent = new StringContent(str, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("Method", "POST");

                    var response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync().Result;
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 驾驶证识别
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DrivingLicenseRecognition(IFormCollection formCollection)
        {
            var result = getAccessToken(ocr_ApiKey, ocr_SecretKey, ocr_CacheKey).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                IFormFileCollection files = (IFormFileCollection)formCollection.Files;
                var url = "https://aip.baidubce.com/rest/2.0/ocr/v1/driving_license?access_token=" + result.Value;
                var stream = files[0].OpenReadStream();//将文件转为流
                var str64 = Utils.FileToBase64(stream);//base64
                //传front如果实际为back也可识别
                var str = "image=" + HttpUtility.UrlEncode(str64);//对字符串进行编码，以便于可靠的http传输
                var httpContent = new StringContent(str, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("Method", "POST");

                    var response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync().Result;
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(result.Value);
            }
        }
    }
}
