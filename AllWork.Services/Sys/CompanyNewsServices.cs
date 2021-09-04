using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class CompanyNewsServices:Base.BaseServices<CompanyNews>,ICompanyNewsServices
    {
        readonly ICompanyNewsRepository _dal;
        public CompanyNewsServices(ICompanyNewsRepository companyNewsRepository)
        {
            _dal = companyNewsRepository;
        }

        public async Task<bool> SaveCompanyNews(CompanyNews companyNews)
        {
            var res = await _dal.SaveCompanyNews(companyNews);
            return res > 0;
        }

        public async Task<CompanyNews> GetCompanyNews(string newsId)
        {
            var res = await _dal.GetCompanyNews(newsId);
            return res;
        }

        public async Task<bool> DeleteCompanyNews(string newsId)
        {
            var res = await _dal.DeleteCompanyNews(newsId);
            return res > 0;
        }

        public async Task<bool> SubmitAudit(string newsId, bool isSubmit)
        {
            var res = await _dal.SubmitAudit(newsId, isSubmit);
            return res > 0;
        }

        public async Task<bool> AuditNews(string newsId, bool isAudit)
        {
            var res = await _dal.AuditNews(newsId, isAudit);
            return res > 0;
        }

        public async Task<Tuple<IEnumerable<CompanyNews>, int>> QueryCompanyNews(CommonParams commonParams)
        {
            var res = await _dal.QueryCompanyNews(commonParams);
            return res;
        }
    }
}
