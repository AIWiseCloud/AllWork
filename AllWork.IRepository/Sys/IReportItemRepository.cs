using AllWork.Model;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IReportItemRepository:Base.IBaseRepository<ReportItem>
    {
        Task<bool> SaveReportItem(ReportItem reportItem);

        Task<bool> DeleteReportItem(string reportId);

        Task<ReportItem> GetReportItem(string reportId);

        Task<Tuple<IEnumerable<ReportItem>, int>> QueryReportItems(ReportItemParams reportItemParams);
    }
}
