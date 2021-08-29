using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class ReportItemRepository : Base.BaseRepository<ReportItem>, IReportItemRepository
    {
        public async Task<bool> SaveReportItem(ReportItem reportItem)
        {
            var instance = await base.QueryFirst("Select * from ReportItem Where ReportId = @ReportId", reportItem);
            string sql;
            if (instance == null)
            {
                sql = "Insert ReportItem (ReportId,TemplateName,Title,GroupNo,Remark,IsCancellation,FIndex,DbConId,SourceSql,TestId)values(@ReportId,@TemplateName,@Title,@GroupNo,@Remark,@IsCancellation,@FIndex,@DbConId,@SourceSql,@TestId)";
            }
            else
            {
                sql = @"Update ReportItem set TemplateName = @TemplateName,Title = @Title,GroupNo = @GroupNo,Remark = @Remark,IsCancellation = @IsCancellation,FIndex = @FIndex,DbConId = @DbConId,SourceSql = @SourceSql,TestId = @TestId Where ReportId = @ReportId";
            }
            return await base.Execute(sql, reportItem) > 0;
        }

        public async Task<bool> DeleteReportItem(string reportId)
        {
            var sql = "Delete from ReportItem Where ReportId = @ReportId";
            var res = await base.Execute(sql, new { ReportId = reportId });
            return res > 0;
        }

        public async Task<ReportItem> GetReportItem(string reportId)
        {
            var sql = "Select * from ReportItem Where ReportId = @ReportId";
            var res = await base.QueryFirst(sql, new { ReportId = reportId });
            return res;
        }

        public async Task<Tuple<IEnumerable<ReportItem>, int>> QueryReportItems(ReportItemParams reportItemParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(" from ReportItem a ");
            if (!string.IsNullOrWhiteSpace(reportItemParams.QueryValue))
            {
                sqlpub.Append(" Where Title = @Title or GroupNo = @GroupNo or TemplateName = @TemplateName ");
            }
            //固定排序
            string sqlorder = " Order by GroupNo, FIndex desc ";
            //求记录数
            var sql1 = "Select Count(a.ReportId) as totalCount " + sqlpub.ToString();
            //获取分页数据
            var sql2 = "Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + " ; " + sql2;

            var res = await base.QueryPagination<ReportItem>(sql,
                new
                {
                    Title = reportItemParams.QueryValue,
                    GroupNo = reportItemParams.QueryValue,
                    TemplateName = reportItemParams.QueryValue,
                    reportItemParams.PageModel.Skip,
                    reportItemParams.PageModel.PageSize
                });
            return res;
        }
    }
}
