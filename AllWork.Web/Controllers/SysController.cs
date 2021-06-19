using AllWork.IServices.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;


namespace AllWork.Web.Controllers
{
    [Route("allwork/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SysController : BaseController
    {
        readonly IUserServices _userServices;
        readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userServices"></param>
        /// <param name="configuration"></param>
        public SysController(IUserServices userServices,IConfiguration configuration)
        {
            _userServices = userServices;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取我的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string LoadMyInfo()
        {
            var info = new
            {
                name = "roy",
                age = 30,
                desc = "就在那一刻，世间安静了"
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);
        }

        [HttpGet]
        public string GetUser()
        {
            var items = _userServices.QueryUser("roy", "123").Result.ToList();
            if (items.Count > 0)
                return items[0].UserName + "there is will ,there is way" + _configuration["WXWork:corpid"];
            else
                return "没有找到";
        }
    }
}
