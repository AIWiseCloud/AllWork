using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        readonly IWebHostEnvironment _env;
        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] IFormCollection formCollection)
        {
            Dictionary<string, string> diclist = new Dictionary<string, string>();
            string webRootPath = _env.WebRootPath;
            FormFileCollection filelist = (FormFileCollection)formCollection.Files;
            long size = filelist.Sum(f => f.Length);
            foreach (IFormFile file in filelist)
            {
                string subpath = "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";//日期作文件夹
                string FilePath = webRootPath + subpath;
                var extendname = file.FileName.Substring(file.FileName.LastIndexOf('.')); //扩展名（含点号)
                var newfilename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extendname;
                DirectoryInfo di = new DirectoryInfo(FilePath);
                if (!di.Exists)
                {
                    di.Create();
                }
                using FileStream fs = System.IO.File.Create(FilePath + newfilename);
                diclist.Add(file.FileName, subpath + newfilename);
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            return Ok(new { count = filelist.Count, size, diclist });
        }

        
    }
}
