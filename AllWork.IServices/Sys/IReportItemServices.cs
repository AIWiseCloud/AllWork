using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IReportItemServices:Base.IBaseServices<ReportItem>
    {
        Task<bool> SaveReportItem(ReportItem reportItem);

        Task<bool> DeleteReportItem(string reportId);

        Task<ReportItem> GetReportItem(string reportId);

        Task<Tuple<IEnumerable<ReportItem>, int>> QueryReportItems(ReportItemParams reportItemParams);
    }
}
