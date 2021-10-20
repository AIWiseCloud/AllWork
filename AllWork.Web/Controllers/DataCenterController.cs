using AllWork.IServices.DataCenter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 数据中心
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataCenterController : ControllerBase
    {
        readonly IAppVisitsServices _appVisitsServices;
        readonly IMallDataServices _mallDataServices;
        public DataCenterController(IAppVisitsServices appVisitsServices, IMallDataServices mallDataServices)
        {
            _appVisitsServices = appVisitsServices;
            _mallDataServices = mallDataServices;
        }

        /// <summary>
        /// 更新日访问量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> WriteAppVisits()
        {
            var res = await _appVisitsServices.WriteAppVisits();
            return Ok(res);
        }

        /// <summary>
        /// 获取访问量
        /// </summary>
        /// <param name="rangType">访问量类型(0表示总访问量，1表示今天访问量)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAppVisits(int rangType = 0)
        {
            var res = await _appVisitsServices.GetAppVisits(rangType);
            return Ok(res);
        }

        /// <summary>
        /// 获取商城统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMallData()
        {
            var res = await _mallDataServices.GetMallData();
            return Ok(res);
        }
    }
}
