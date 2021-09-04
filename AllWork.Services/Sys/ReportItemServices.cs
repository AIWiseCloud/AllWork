using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class ReportItemServices:Base.BaseServices<ReportItem>,IReportItemServices
    {
        readonly IReportItemRepository _dal;
        public ReportItemServices(IReportItemRepository reportItemRepository)
        {
            _dal = reportItemRepository;
        }

        public async Task<bool> SaveReportItem(ReportItem reportItem)
        {
            var res = await _dal.SaveReportItem(reportItem);
            return res;
        }

        public async Task<bool> DeleteReportItem(string reportId)
        {
            var res = await _dal.DeleteReportItem(reportId);
            return res;
        }

        public async Task<ReportItem> GetReportItem(string reportId)
        {
            var res = await _dal.GetReportItem(reportId);
            return res;
        }

        public async Task<Tuple<IEnumerable<ReportItem>, int>> QueryReportItems(CommonParams reportItemParams)
        {
            var res = await _dal.QueryReportItems(reportItemParams);
            return res;
        }
    }
}
