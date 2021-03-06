using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using AllWork.Web.Helper.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 纷享销客
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class XiaokeController : ControllerBase
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly string _appId = "FSAID_131a12a";
        readonly string _appSecret = "72c1867434064e63a4abbca5144f63fb";
        readonly string _permanentCode = "0648EE60D0B06ABC2D7FF7419B154148";//永久授权码
        readonly string _key_corpAccessToken = "xiaoke_corpAccessToken"; //缓存键
        readonly string _key_corpId = "xiaoke_corpId"; //缓存键-
        readonly ISalesmanServices _salesmanServices;
        public XiaokeController(IHttpClientFactory httpClientFactory, ISalesmanServices salesmanServices)
        {
            _httpClientFactory = httpClientFactory;
            _salesmanServices = salesmanServices;
        }

        async Task<object> GetCorpAccessToken()
        {
            var corpAccessToken= RedisClient.redisClient.GetStringKey(_key_corpAccessToken);
            var corpId = RedisClient.redisClient.GetStringKey(_key_corpId);
            if(string.IsNullOrEmpty(corpAccessToken) || string.IsNullOrEmpty(corpId))
            {
                var data = new
                {
                    appId = _appId,
                    appSecret = _appSecret,
                    permanentCode = _permanentCode
                };
                var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync("https://open.fxiaoke.com/cgi/corpAccessToken/get/V2", content);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();
                dynamic resobj = JsonConvert.DeserializeObject(res);
                //缓存
                int expires = Convert.ToInt32(resobj.expiresIn);
                RedisClient.redisClient.SetStringKey(_key_corpAccessToken, resobj.corpAccessToken, new TimeSpan(0, 0, expires));
                RedisClient.redisClient.SetStringKey(_key_corpId, resobj.corpId, new TimeSpan(0, 0, expires));
                return JsonConvert.DeserializeObject(res);
            }
            else
            {
                var res = new
                {
                    corpAccessToken= corpAccessToken.Replace("\"", string.Empty),
                    corpId=corpId.Replace("\"", string.Empty)
                };
                return res ;
            }
        }

        /// <summary>
        /// 从销客获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepts()
        {
            var data = await GetCorpAccessToken();
            var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Method", "POST"); //不要也正常
            var response=await client.PostAsync("https://open.fxiaoke.com/cgi/department/list", content);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            return Ok(JsonConvert.DeserializeObject(res));
        }

        /// <summary>
        /// 从销客获取销客部门下成员信息(详细)
        /// </summary>
        /// <param name="departmentId">部门ID, 为非负整数 </param>
        /// <param name="fetchChild">获取子部门员工</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> GetEmps(int departmentId=999999, bool fetchChild=true)
        {
            object rightdata2 = await GetCorpAccessToken();
            dynamic rightdata = rightdata2;
            var data = new
            {
                rightdata.corpAccessToken,
                rightdata.corpId,
                departmentId,
                fetchChild,
                showDepartmentIdsDetail = false
            };
            var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Method", "POST"); //不要也正常
            var response = await client.PostAsync("https://open.fxiaoke.com/cgi/user/list", content);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync();
            return Ok(JsonConvert.DeserializeObject(res));
        }

        /// <summary>
        /// 批量汇入业务员到APP后台
        /// </summary>
        /// <param name="salesmen">业务员集合</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy:"Editor")]
        public async Task<IActionResult> ImportSalesmen(List<Salesman> salesmen)
        {
            var res = await _salesmanServices.ImportSalesma(salesmen);
            return Ok(res);
        }

        /// <summary>
        /// 从APP数据库获取营销人员列表
        /// </summary>
        /// <param name="keywords">关键字(可为空)</param>
        /// <param name="ignoreStop">是否忽略离职人员，默认不忽略</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSalesmen(string keywords = "", bool ignoreStop = false)
        {
            var res = await _salesmanServices.GetSalesmen(keywords, ignoreStop);
            return Ok(res);
        }
    }
}
