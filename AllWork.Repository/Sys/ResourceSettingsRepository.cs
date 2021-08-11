using AllWork.IRepository.Sys;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class ResourceSettingsRepository:Base.BaseRepository<ResourceSettings>, IResourceSettingsRepository
    {
        public ResourceSettingsRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> SaveResourceSettings(ResourceSettings resourceSettings)
        {
            var instance = await base.QueryFirst("Select * from ResourceSettings Where SourceId = @SourceId", new {resourceSettings.SourceId });
            string sql;
            if (instance == null)
            {
                sql = @"Insert ResourceSettings (SourceId,Subject,GroupNo,ImgUrl,Navigator,Remark,IsCancellation,FIndex)values(@SourceId,@Subject,@GroupNo,@ImgUrl,@Navigator,@Remark,@IsCancellation,@FIndex)";
            }
            else
            {
                sql = @"Update ResourceSettings set Subject = @Subject,GroupNo = @GroupNo,ImgUrl = @ImgUrl,Navigator = @Navigator,Remark = @Remark,IsCancellation = @IsCancellation,FIndex = @FIndex Where SourceId = @SourceId";
            }
            return await base.Execute(sql, resourceSettings) > 0;
        }

        public async Task<bool> DeleteResourceSettings(string sourceId)
        {
            var sql = "Delete from ResourceSettings Where SourceId = @SourceId";
            return await base.Execute(sql, new { SourceId = sourceId }) > 0;
        }

        public async Task<ResourceSettings> GetResourceSettings(string sourceId)
        {
            var sql = "Select * from ResourceSettings Where SourceId = @SourceId";
            return await base.QueryFirst(sql, new { SourceId = sourceId });
        }

        public async Task<dynamic> GetGroups()
        {
            var sql = "Select distinct GroupNo from ResourceSettings";
            return await base.QueryList<dynamic>(sql, new { });
        }

        public async Task<List<ResourceSettings>> GetResourceSettingsByGroup(string groupNo)
        {
            var sql = "Select * from ResourceSettings Where GrupNo = @GroupNo order by FIndex";
            var res = await base.QueryList(sql, new { GroupNo = groupNo });
            return res;
        }

        public async Task<Tuple<IEnumerable<ResourceSettings>, int>> QueryResourceSettings(ResourceParams resourceParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(" from Settings ");
            if (!string.IsNullOrWhiteSpace(resourceParams.KeyWords))
            {
                sqlpub.AppendFormat(" Where Subject = @Subject or GroupNo = @GroupNo or Remark = '%{0}%' ", resourceParams.KeyWords);
            }
            //固定排序
            string sqlorder = " Order by GroupNo, FIndex desc ";
            //求记录数
            var sql1 = "Select Count(a.SourceId) as totalCount " + sqlpub.ToString();
            //获取分页数据
            var sql2 = "Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + " ; " + sql2;

            var res = await base.QueryPagination<ResourceSettings>(sql, 
                new {
                    Subject = resourceParams.KeyWords,
                    GroupNo = resourceParams.KeyWords,
                    resourceParams.PageModel.Skip,
                    resourceParams.PageModel.PageSize
                });
            return res;
        }
    }
}
