using AllWork.Web.Helper.Redis;
using AllWork.Web.Helper.WXWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 企业微信
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WXWorkController : ControllerBase
    {
        readonly IConfiguration _cofiguration;
        readonly IWebHostEnvironment _env;
        readonly string _corpId;
        readonly string _agentid;//企业助手
        readonly string _secret;//企业助手的密钥
        readonly IHttpClientFactory _httpClientFactory;
        readonly string _key_qyzs = "WX_QYZS_TOKEN";//企业助手token缓存key
        public WXWorkController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
            _cofiguration = configuration;
            _corpId = _cofiguration["WXWork:corpid"];
            _secret = _cofiguration["WXWork:secret"];
            _agentid = _cofiguration["WXWork:agentid"];
        }

        /// <summary>
        /// 获取access_token是调用企业微信API接口的第一步，相当于创建了一个登录凭证，其它的业务API接口，都需要依赖于access_token来鉴权调用者身份。
        /// 需要缓存access_token，用于后续接口的调用,有效期2小时，不能频繁调用gettoken接口，否则会受到频率拦截
        /// 为了安全考虑，开发者请勿将access_token返回给前端，需要开发者保存在后台，所有访问企业微信api的请求由后台发起
        /// </summary>
        /// <returns></returns>
        async Task<IActionResult> GetQYAccessToken(string key, string secret)
        {
            //从redis读取token
            var token = RedisClient.redisClient.GetStringKey(key);
            //如果缓存没有token
            if (string.IsNullOrEmpty(token))
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", _corpId, secret);
                var result = await client.GetStringAsync(url);
                dynamic resultObject = JsonConvert.DeserializeObject(result);
                if (resultObject.errcode == 0)
                {
                    token = resultObject.access_token;
                    int seconds = resultObject.expires_in;//凭证的有效时间（秒）
                    //(这里有坑，总是返回7200,网上说还有提前过期可能，所以还得通过方法clearToken判断errcode:42001的返回
                    //缓存token, 有效期seconds
                    RedisClient.redisClient.SetStringKey(key, token, new TimeSpan(0, 0, seconds));
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

        //从企业微信返回数据中查看errcode判断token是否过期，若过期则删除缓存
        void clearToken(string response, string key)
        {
            dynamic result = JsonConvert.DeserializeObject(response);
            //42001为token过期
            if (result.errcode == 42001)
            {
                RedisClient.redisClient.KeyDelete(key);
                //但还有一问题，某方法调用缓存实际过期在此清除后，方法不会重新执行，如何处理？
            }
        }

        /// <summary>
        /// 获取访问用户身份
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string code)
        {
            //待进行事项：建立企业微信小程序应用，用应用的secret才能getUserInfo, 但在企业微信建立小程序应用要先发布小程序
            var result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", result.Value, code);
                var response = await client.GetStringAsync(url);

                clearToken(response, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
                return Ok(response);
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="deptId">部门id。获取指定部门及其下的子部门（以及及子部门的子部门等等，递归）。 如果不填，默认获取全量组织架构</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDeptList(string deptId = "")
        {
            var result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                var client = _httpClientFactory.CreateClient();
                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}&id={1}", result.Value, deptId);
                var response = await client.GetStringAsync(url);

                clearToken(response, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
                return Ok(response);
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 获取部门成员详情
        /// </summary>
        /// <param name="department_id">获取的部门id</param>
        /// <param name="fetch_child">1/0：是否递归获取子部门下面的成员</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDeptUsers(string department_id, int fetch_child = 1)
        {
            var result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id={1}&fetch_child={2}", result.Value, department_id, fetch_child);
                    var response = await client.GetStringAsync(url);
                    clearToken(response, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
                    return Ok(response);
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
        /// 通过手机号获取其所对应的userid
        /// </summary>
        /// <param name="mobile">手机号码。长度为5~32个字节</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUserId(string mobile)
        {
            var result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;
            if (result.StatusCode == 200)
            {
                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserid?access_token={0}", result.Value);
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("method", "POST");
                var str = JsonConvert.SerializeObject(new { mobile });
                HttpContent httpContent = new StringContent(str, Encoding.UTF8);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(url, httpContent);
                response.EnsureSuccessStatusCode();
                var res = response.Content.ReadAsStringAsync().Result;
                clearToken(res, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
                return Ok(res);
            }
            else
            {
                return BadRequest(result.Value);
            }
        }

        /// <summary>
        /// 发送应用消息
        /// </summary>
        /// <param name="msgEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(MsgEntity msgEntity)
        {
            if (string.IsNullOrEmpty(msgEntity.ToUser))
            {
                return BadRequest(new { MessageProcessingHandler = "touser不能为空！" });
            }

            dynamic result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;

            if (result.StatusCode == 200)
            {
                CorpSendBase paramData;
                var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", result.Value);
                if (msgEntity.MsgType == "markdown")
                {
                    paramData = new CorpSendMarkdown(msgEntity.Content);
                }
                else if (msgEntity.MsgType == "file")
                {
                    paramData = new CorpSendFile(msgEntity.Media_Id);
                }
                else
                {
                    paramData = new CorpSendText(msgEntity.Content);//text
                }
                try
                {
                    paramData.touser = msgEntity.ToUser;
                    //paramData.toparty = msgEntity.ToParty;
                    //paramData.totag = msgEntity.ToTag;
                    paramData.agentid = _agentid;
                    var content = JsonConvert.SerializeObject(paramData);

                    var httpContent = new StringContent(content, Encoding.UTF8);
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    var client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Add("Method", "Post");
                    HttpResponseMessage response = await client.PostAsync(url, httpContent);
                    response.EnsureSuccessStatusCode();
                    var res = response.Content.ReadAsStringAsync().Result;
                    clearToken(res, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    AllWork.Common.Mail.SendMail("企业微信消息发送出错", ex.Message);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// 上传临时素材 (手工选择文件后上传）
        /// </summary>
        /// <param name="file">素材</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            var result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;
            if (result.StatusCode != 200)
            {
                return BadRequest(result.Value);
            }
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", result.Value, file.Name);
            //读取上载文件流
            var stream = file.OpenReadStream();
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            var boundary = DateTime.Now.Ticks.ToString("X");// 随机分隔线
            var content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

            HttpContent byteContent = new ByteArrayContent(data);
            content.Add(byteContent);
            byteContent.Headers.Remove("Content-Disposition");
            byteContent.Headers.TryAddWithoutValidation("Content-Disposition", $"form-data; name=\"media\";filename=\"{file.FileName}\"" + "");
            byteContent.Headers.Remove("Content-Type");
            byteContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Method", "Post");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var res = response.Content.ReadAsStringAsync().Result;
            clearToken(res, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
            return Ok(res);
        }



        /// <summary>
        /// 上传临时素材(以流的形式接收后上传)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadMedia2(string fileName)
        {
            var result = GetQYAccessToken(_key_qyzs, _secret).Result as ObjectResult;
            if (result.StatusCode != 200)
            {
                return BadRequest(result.Value);
            }
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", result.Value, "file");
            try
            {
                var request = this.HttpContext.Request;
                request.EnableBuffering();//确保可以多次读取
                var requestReader = new StreamReader(request.Body);
                //以下要用异步：否则会报Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead
                var requestContent = await requestReader.ReadToEndAsync();
                request.Body.Position = 0;

                //读取上载文件流
                byte[] fileData = new byte[requestReader.BaseStream.Length];
                requestReader.BaseStream.Read(fileData, 0, fileData.Length);

                var boundary = DateTime.Now.Ticks.ToString("X");// 随机分隔线
                var content = new MultipartFormDataContent(boundary);
                content.Headers.Remove("Content-Type");
                content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

                HttpContent byteContent = new ByteArrayContent(fileData);
                content.Add(byteContent);
                byteContent.Headers.Remove("Content-Disposition");
                byteContent.Headers.TryAddWithoutValidation("Content-Disposition", $"form-data; name=\"media\";filename=\"{fileName}\"" + "");
                byteContent.Headers.Remove("Content-Type");
                byteContent.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Method", "Post");
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                clearToken(res, _key_qyzs);//通过errcode判断token是否实际过期，若过期则清除
                return Ok(res);
            }
            catch (Exception ex)
            {
                AllWork.Common.Mail.SendMail("上传临时素材出错", ex.Message);
                return BadRequest();
            }
        }


    }
}
