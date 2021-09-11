using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 微信
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WXController : ControllerBase
    {
        readonly IHttpClientFactory _httpClientFactory;
        public WXController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 解密微信对称加密数据，获取用户联合运营编号UnionID
        /// </summary>
        /// <param name="sessionKey">临时会话秘钥</param>
        /// <param name="encryptedData">微信用户敏感加密数据</param>
        /// <param name="iv">解密初始向量</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DecryptSensitiveData(string sessionKey, string encryptedData, string iv)
        {
            try
            {
                var data = DecryptByAesBytes(encryptedData, sessionKey, iv);
                return Ok(new { code = 1, msg = "SUCCESS", result = data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = 0, msg = "FAIL,cause：" + ex.Message });
            }
        }

        /// <summary>
        /// AES对称解密
        /// </summary>
        /// <param name="encryptedData">包括敏感数据在内的完整用户信息的加密数据</param>
        /// <param name="sessionKey">临时会话秘钥</param>
        /// <param name="iv">加密算法的初始向量</param>
        /// <returns></returns>
        private static string DecryptByAesBytes(string encryptedData, string sessionKey, string iv)
        {
            try
            {
                //非空验证
                if (!string.IsNullOrWhiteSpace(encryptedData) && !string.IsNullOrWhiteSpace(sessionKey) && !string.IsNullOrWhiteSpace(iv))
                {
                    var decryptBytes = Convert.FromBase64String(encryptedData.Replace(' ', '+'));
                    var keyBytes = Convert.FromBase64String(sessionKey.Replace(' ', '+'));
                    var ivBytes = Convert.FromBase64String(iv.Replace(' ', '+'));

                    var aes = new AesCryptoServiceProvider
                    {
                        Key = keyBytes,
                        IV = ivBytes,
                        Mode = CipherMode.CBC,
                        Padding = PaddingMode.PKCS7
                    };
                    var outputBytes = aes.CreateDecryptor().TransformFinalBlock(decryptBytes, 0, decryptBytes.Length);
                    var decryptResult = Encoding.UTF8.GetString(outputBytes);
                    dynamic decryptData = JsonConvert.DeserializeObject(decryptResult, new { unionid = "" }.GetType());
                    return decryptData.unionid;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取用户openid, session_key等数据
        /// </summary>
        /// <param name="appid">小程序ID</param>
        /// <param name="secret">小程序密钥</param>
        /// <param name="json_code">登录态(先从wx.login()获取)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserData(string appid, string secret, string json_code)
        {
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret="
                + secret + "&js_code=" + json_code + "&grant_type=authorization_code";
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var result = await httpClient.GetStringAsync(url);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
