using AllWork.IRepository.Sys;
using AllWork.Model;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class SubMessageRepository:Base.BaseRepository<SubMessage>, ISubMessageRepository
    {
        public async Task<OperResult> SaveSubmessage(SubMessage subMessage)
        {
            if (string.IsNullOrEmpty(subMessage.ID))
            {
                subMessage.ID = System.Guid.NewGuid().ToString();
            }
            var instance = await base.QueryFirst("Select * from SubMessage Where ID = @ID", new { subMessage.ID });
            if (instance == null)
            {
                var insertSql = "Insert SubMessage (ID,ParentId,FNumber,FName,IsCancellation,FIndex,FNote)values(@ID,@ParentId,@FNumber,@FName,@IsCancellation,@FIndex,@FNote)";
                var status = await base.Execute(insertSql, subMessage) > 0;
                return new OperResult { IdentityKey = subMessage.ID, Status = status };
            }
            else
            {
                var updateSql = "Update SubMessage set ParentId = @ParentId,FNumber = @FNumber,FName = @FName,IsCancellation = @IsCancellation,FIndex = @FIndex,FNote = @FNote Where ID = @ID";
                var status = await base.Execute(updateSql, subMessage) > 0;
                return new OperResult { IdentityKey = subMessage.ID, Status = status };
            }
        }

        public async Task<SubMessage> GetSubMessage(string id)
        {
            var sql = "Select * from SubMessage Where ID = @ID";
            return await base.QueryFirst(sql, new { ID = id });
        }

        public async Task<IEnumerable<SubMessage>> GetSubMessageList(string parentId)
        {
            var sql = "Select * from SubMessage Where ParentId = @ParentId";
            return await base.QueryList(sql, new { ParentId = parentId });
        }

        public async Task<bool> DeleteSubMessage(string id)
        {
            var sql = "Delete from SubMessage Where ID = @ID";
            return await base.Execute(sql, new { ID = id }) > 0;
        }
    }
}
