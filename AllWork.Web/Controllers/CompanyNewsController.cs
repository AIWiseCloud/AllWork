using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 新闻动态
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(policy: "Editor")]
    public class CompanyNewsController : ControllerBase
    {
        readonly ICompanyNewsServices _companyNewsServices;
        public CompanyNewsController(ICompanyNewsServices companyNewsServices)
        {
            _companyNewsServices = companyNewsServices;
        }

        /// <summary>
        /// 保存新闻动态草稿
        /// </summary>
        /// <param name="companyNews"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveCompanyNews(CompanyNews companyNews)
        {
            if (string.IsNullOrEmpty(companyNews.NewsId))
            {
                companyNews.NewsId = Guid.NewGuid().ToString();
            }
            var res = await _companyNewsServices.SaveCompanyNews(companyNews);
            return Ok(new OperResult { Status = res, IdentityKey = companyNews.NewsId });
        }

        /// <summary>
        /// 获取新闻动态详情
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyNews(string newsId)
        {
            var res = await _companyNewsServices.GetCompanyNews(newsId);
            return Ok(res);
        }

        /// <summary>
        /// 删除动态
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCompanyNews(string newsId)
        {
            var res = await _companyNewsServices.DeleteCompanyNews(newsId);
            return Ok(res);
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <param name="newsId">动态ID</param>
        /// <param name="isSubmit">true提交审核，false撤销提交</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> SubmitAudit(string newsId, bool isSubmit)
        {
            var res = await _companyNewsServices.SubmitAudit(newsId, isSubmit);
            return Ok(res);
        }

        /// <summary>
        /// 审核新闻动态
        /// </summary>
        /// <param name="newsId">动态ID</param>
        /// <param name="isAudit">true通过审核，false撤销打回</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AuditNews(string newsId, bool isAudit)
        {
            var res = await _companyNewsServices.AuditNews(newsId, isAudit);
            return Ok(res);
        }

        /// <summary>
        /// 分页查询新闻动态
        /// </summary>
        /// <param name="commonParams"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> QueryCompanyNews(NewsParams commonParams)
        {
            var res = await _companyNewsServices.QueryCompanyNews(commonParams);
            return Ok(new { TotalCount = res.Item2, items = res.Item1 });
        }
    }
}
