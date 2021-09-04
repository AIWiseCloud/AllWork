using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ICompanyNewsServices:Base.IBaseServices<CompanyNews>
    {
        Task<bool> SaveCompanyNews(CompanyNews companyNews);

        Task<CompanyNews> GetCompanyNews(string newsId);

        Task<bool> DeleteCompanyNews(string newsId);

        Task<bool> SubmitAudit(string newsId, bool isSubmit);

        Task<bool> AuditNews(string newsId, bool isAudit);

        Task<Tuple<IEnumerable<CompanyNews>, int>> QueryCompanyNews(CommonParams commonParams);
    }
}
