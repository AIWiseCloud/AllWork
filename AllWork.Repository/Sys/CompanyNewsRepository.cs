using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class CompanyNewsRepository:Base.BaseRepository<CompanyNews>, ICompanyNewsRepository
    {
        public async Task<int> SaveCompanyNews(CompanyNews companyNews)
        {
            var instance = await base.QueryFirst("Select * from CompanyNews Where NewsId = @NewsId", companyNews);
            string sql;
            if (instance == null)
            {
                sql = @"Insert CompanyNews (NewsId,Title,ImgUrl,Body,Source,Author,Creator,FIndex)"+
"values(@NewsId,@Title,@ImgUrl,@Body,@Source,@Author,@Creator,@FIndex)";
            }
            else
            {
                sql = @"Update CompanyNews set Title = @Title,ImgUrl = @ImgUrl,Body = @Body,Source = @Source,
Author = @Author,Creator = @Creator,FIndex = @FIndex Where NewsId = @NewsId ";
            }
            return await base.Execute(sql, companyNews);
        }

        public async Task<CompanyNews> GetCompanyNews(string newsId)
        {
            //阅读量(只计审核状态下的)
            await base.Execute("Update CompanyNews set AmountReading=AmountReading+1 Where NewsId = @NewsId and StatusId = 2", new { NewsId = newsId });
            var res = await base.QueryFirst("Select * from CompanyNews Where NewsId = @NewsId", new { NewsId = newsId });
            return res;
        }

        public async Task<int> DeleteCompanyNews(string newsId)
        {
            var res = await base.Execute("Delete from CompanyNews Where NewsId = @NewsId", new { NewsId = newsId });
            return res;
        }

        public async Task<int> SubmitAudit(string newsId, bool isSubmit)
        {
            var sql = "Update CompanyNews Set StatusId = @StatusId Where NewsId = @NewsId";
            var res = await base.Execute(sql, new { NewsId = newsId, StatusId = isSubmit ? 1 : 0 });
            return res;
        }

        public async Task<int> AuditNews(string newsId, bool isAudit)
        {
            var sql = "Update CompanyNews Set StatusId = @StatusId Where NewsId = @NewsId";
            var res = await base.Execute(sql, new { NewsId = newsId, StatusId = isAudit ? 2 : 0 });
            return res;
        }

        public async Task<Tuple<IEnumerable<CompanyNews>, int>> QueryCompanyNews(NewsParams commonParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(" from CompanyNews a ");
            if (!string.IsNullOrWhiteSpace(commonParams.Keywords))
            {
                sqlpub.Append(" Where (Title = @Title or Source = @Source or Author = @Author) ");
            }
            if (commonParams.OnlyShowSubmitStatus == 1)
            {
                sqlpub.Append(" and StatusId > 0");
            }
            //固定排序
            string sqlorder = " Order by CreateDate, FIndex desc ";
            //求记录数
            var sql1 = "Select Count(a.NewsId) as totalCount " + sqlpub.ToString();
            //获取分页数据(不带正文)
            var sql2 = "Select NewsId, Title, ImgUrl, Source, Author, Creator, CreateDate, FIndex,StatusId, AmountReading, NumberLike " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + " ; " + sql2;

            var res = await base.QueryPagination<CompanyNews>(sql,
                new
                {
                    Title = commonParams.Keywords,
                    Source = commonParams.Keywords,
                    Author = commonParams.Keywords,
                    commonParams.PageModel.Skip,
                    commonParams.PageModel.PageSize
                });
            return res;
        }
    }
}
