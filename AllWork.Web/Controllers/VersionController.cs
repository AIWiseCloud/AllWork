using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 版本管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class VersionController : ControllerBase
    {
        readonly IVersionManagementServices _versionManagementServices;
        readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _env;
        public VersionController(IVersionManagementServices versionManagementServices, IConfiguration configuration, IWebHostEnvironment env)
        {
            _versionManagementServices = versionManagementServices;
            _configuration = configuration;
            _env = env;
        }

        /// <summary>
        /// 保存版本记录
        /// </summary>
        /// <param name="versionManagement"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveVersionManagement(VersionManagement versionManagement)
        {
            var res = await _versionManagementServices.SaveVersionManagement(versionManagement);
            return Ok(res);
        }

        /// <summary>
        /// 删除版本记录
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteVersonManagement(string versionId)
        {
            var res = await _versionManagementServices.DeleteVersionManagement(versionId);
            return Ok(res);
        }

        /// <summary>
        /// 查询版本记录
        /// </summary>
        /// <param name="versionParams"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> QueryVersionManagement(VersionParams versionParams)
        {
            var res = await _versionManagementServices.QueryVersionManagerments(versionParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 获取最新版本记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewestVersionManagement()
        {
            var res = await _versionManagementServices.GetNewestVersionManagement();
            return Ok(res);
        }

        /// <summary>
        /// 上传App打包文件
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAppPackage(IFormCollection formCollection)
        {
            Dictionary<string, string> diclist = new Dictionary<string, string>();
            //不同版本均保存在同一个目录，实际上一次仅限上传一个包文件（虽然这里没有限制）
            var subPath = _configuration.GetSection("App:PackagePath").Value; //wwwroot下的路径
            var savePath = _env.WebRootPath+ subPath; //文件保存路径
            DirectoryInfo di = new DirectoryInfo(savePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FormFileCollection filelist = (FormFileCollection)formCollection.Files;
            long size = filelist.Sum(f => f.Length);
            foreach (IFormFile file in filelist)
            {
                diclist.Add(file.FileName, subPath + file.FileName);
                //目标文件不存在，则上传（2021-8-30）
                if (!System.IO.File.Exists(savePath + file.FileName))
                {
                    using FileStream fs = System.IO.File.Create(savePath + file.FileName);
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
            }
            return Ok( new { count = filelist.Count, size, diclist });
        }
    }
}
