using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 报表项目
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class ReportController : ControllerBase
    {
        readonly IReportItemServices _reportItemServices;
        readonly IConfiguration _configuration;
        public ReportController(IReportItemServices reportItemServices, IConfiguration configuration)
        {
            _reportItemServices = reportItemServices;
            _configuration = configuration;
        }

        /// <summary>
        /// 保存报表项目
        /// </summary>
        /// <param name="reportItem"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveReportItem(ReportItem reportItem)
        {
            var res = await _reportItemServices.SaveReportItem(reportItem);
            return Ok(res);
        }

        /// <summary>
        /// 删除报表项目
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteReportItem(string reportId)
        {
            var res = await _reportItemServices.DeleteReportItem(reportId);
            return Ok(res);
        }

        /// <summary>
        /// 获取报表项目
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetReportItem(string reportId)
        {
            var res = await _reportItemServices.GetReportItem(reportId);
            return Ok(res);
        }

        /// <summary>
        /// 查询报表项目
        /// </summary>
        /// <param name="reportItemParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryReportItems(ReportItemParams reportItemParams)
        {
            var res = await _reportItemServices.QueryReportItems(reportItemParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// json字符串存成json文件
        /// </summary>
        /// <param name="jsonInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> StringSaveAsJsonFile(JsonInfo jsonInfo)
        {
            try
            {
                //生成的文件名格式：报表Id + 逗号 + 单事情
                var fileName = jsonInfo.ReportId + "," + jsonInfo.BillId;
                var fullName = _configuration.GetSection("Report:SourcePath").Value + fileName + ".json";
                await System.IO.File.WriteAllTextAsync(fullName, Newtonsoft.Json.JsonConvert.SerializeObject(jsonInfo.Source));
                return Ok(new { status = true, id = fileName });
            }
            catch
            {
                return Ok(new { status = false, id = "error" });
            }

        }


    }




    public class JsonInfo
    {
        public string ReportId
        { get; set; }

        public object BillId
        { get; set; }

        public object Source
        { get; set; }
    }
}
