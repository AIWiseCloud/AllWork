using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface ICompanyNewsRepository:Base.IBaseRepository<CompanyNews>
    {
        Task<int> SaveCompanyNews(CompanyNews companyNews);

        Task<CompanyNews> GetCompanyNews(string newsId);

        Task<int> DeleteCompanyNews(string newsId);

        Task<int> SubmitAudit(string newsId, bool isSubmit);

        Task<int> AuditNews(string newsId, bool isAudit);

        Task<Tuple<IEnumerable<CompanyNews>, int>> QueryCompanyNews(NewsParams commonParams);
    }
}
