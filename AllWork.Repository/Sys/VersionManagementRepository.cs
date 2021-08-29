using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class VersionManagementRepository:Base.BaseRepository<VersionManagement>,IVersionManagementRepository
    {
        public async Task<bool> SaveVersionManagement(VersionManagement versionManagement)
        {
            var instance = await base.QueryFirst("Select * from VersionManagement Where VersionId = @VersionId", versionManagement);
            string sql;
            if (instance == null)
            {
                sql = "Insert VersionManagement (VersionId,PackageUrl,FNote,Creator)values(@VersionId,@PackageUrl,@FNote,@Creator)";
            }
            else
            {
                sql = "Update VersionManagement set PackageUrl = @PackageUrl,FNote = @FNote,Creator = @Creator Where VersionId = @VersionId";
            }
            return await base.Execute(sql, versionManagement) > 0;
        }

        public async Task<bool> DeleteVersionManagement(string versionId)
        {
            var sql = "Delete from VersionManagement Where VersionId = @VersionId";
            var res = await base.Execute(sql, new { VersionId = versionId });
            return res > 0;
        }

        public async Task<Tuple<IEnumerable<VersionManagement>, int>> QueryVersionManagerments(VersionParams versionParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(" from VersionManagement a ");
            if (!string.IsNullOrWhiteSpace(versionParams.Keywords))
            {
                sqlpub.AppendFormat(" Where VersionId = @VersionId or FNote like '%{0}%' ",versionParams.Keywords);
            }
            //固定排序
            string sqlorder = " Order by VersionId desc ";
            //求记录数
            var sql1 = "Select Count(a.VersionId) as totalCount " + sqlpub.ToString();
            //获取分页数据
            var sql2 = "Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + " ; " + sql2;

            var res = await base.QueryPagination<VersionManagement>(sql,
                new
                {
                    VersionId = versionParams.Keywords,
                    versionParams.PageModel.Skip,
                    versionParams.PageModel.PageSize
                });
            return res;
        }

        public async Task<VersionManagement> GetNewestVersionManagement()
        {
            var sql = "select * from VersionManagement where VersionId in (select max(VersionId) from VersionManagement)";
            var res = await base.QueryFirst(sql);
            return res;
        }
    }
}
