using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class ProjectCaseRepository:Base.BaseRepository<ProjectCase>,IProjectCaseRepository
    {
        public async Task<int> SaveProjectCase(ProjectCase projectCase)
        {
            var instance = await base.QueryFirst("Select * from ProjectCase Where ID = @ID", projectCase);
            string sql;
            if (instance == null)
            {
                sql = @"Insert ProjectCase (ID,OrganizationName,ProjectName,SiteCategory,Location,Area,ImgUrl,InspectionDate,Summary,Creator)
values(@ID,@OrganizationName,@ProjectName,@SiteCategory,@Location,@Area,@ImgUrl,@InspectionDate,@Summary,@Creator)";
            }
            else
            {
                sql = @"Update ProjectCase set OrganizationName = @OrganizationName,ProjectName = @ProjectName,SiteCategory = @SiteCategory,
Location = @Location,Area = @Area,ImgUrl = @ImgUrl,InspectionDate = @InspectionDate,Summary = @Summary,Creator = @Creator Where ID = @ID";
            }
            var res = await base.Execute(sql, projectCase);
            return res;
        }

        public async Task<ProjectCase> GetProjectCase(string id)
        {
            var instance = await base.QueryFirst("Select * from ProjectCase Where ID = @ID", new { ID = id });
            return instance;
        }

        public async Task<int> DeleteProjectCase(string id)
        {
            var instance = await base.Execute("Delete from ProjectCase Where ID = @ID", new { ID = id });
            return instance;
        }

        public async Task<Tuple<IEnumerable<ProjectCase>, int>> QueryProjectCase(ProjectCaseParams projectCaseParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(" from ProjectCase a Where (1=1)");
            if (!string.IsNullOrWhiteSpace(projectCaseParams.Keywords))
            {
                sqlpub.AppendFormat(" and (OrganizationName like '%{0}%' or ProjectName like '%{0}%' or SiteCategory like '%{0}%' or Summary like '%{0}%' ) ", projectCaseParams.Keywords);
            }
            if (!string.IsNullOrWhiteSpace(projectCaseParams.SiteCategory))
            {
                sqlpub.Append(" and SiteCategory = @SiteCategory");
            }
            if (!string.IsNullOrWhiteSpace(projectCaseParams.Location))
            {
                sqlpub.AppendFormat(" and Location like CONCAT('%','{0}','%')", projectCaseParams.Location);
            }
            //固定排序
            string sqlorder = " Order by Area desc ";
            //求记录数
            var sql1 = "Select Count(a.ID) as totalCount " + sqlpub.ToString();
            //获取分页数据(不带正文)
            var sql2 = "Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + " ; " + sql2;

            var res = await base.QueryPagination<ProjectCase>(sql,
                new
                {
                    projectCaseParams.SiteCategory,
                    projectCaseParams.PageModel.Skip,
                    projectCaseParams.PageModel.PageSize
                });
            return res;
        }
    }
}
