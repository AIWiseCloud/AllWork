using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 工程案例
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectCaseController : ControllerBase
    {
        readonly IProjectCaseServices _projectCaseServices;
        public ProjectCaseController(IProjectCaseServices projectCaseServices)
        {
            _projectCaseServices = projectCaseServices;
        }
        /// <summary>
        /// 保存工程案例
        /// </summary>
        /// <param name="projectCase"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveProjectCase(ProjectCase projectCase)
        {
            var res = await _projectCaseServices.SaveProjectCase(projectCase);
            return Ok(res);
        }

        /// <summary>
        /// 获取工程案例详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetProjectCase(string id)
        {
            var res = await _projectCaseServices.GetProjectCase(id);
            return Ok(res);
        }

        /// <summary>
        /// 删除工程案例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteProjectCase(string id)
        {
            var res = await _projectCaseServices.DeleteProjectCase(id);
            return Ok(res);
        }

        /// <summary>
        /// 查询工程案例
        /// </summary>
        /// <param name="projectCaseParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryProjectCases(ProjectCaseParams projectCaseParams)
        {
            var res = await _projectCaseServices.QueryProjectCase(projectCaseParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }
    }
}
