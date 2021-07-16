using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class SubMessageServices:Base.BaseServices<SubMessage>,ISubMessageServices
    {
        readonly ISubMessageRepository _dal;

        public SubMessageServices(ISubMessageRepository subMessageRepository)
        {
            _dal = subMessageRepository;
        }

        public async Task<bool> SaveSubmessage(SubMessage subMessage)
        {
            var instance = await _dal.QueryFirst("Select * from SubMessage Where ID = @ID", new { ID = subMessage.ID });
            if (instance == null)
            {
                var insertSql = "Insert SubMessage (ID,ParentId,FNumber,FName,IsCancellation,FIndex,FNote)values(@ID,@ParentId,@FNumber,@FName,@IsCancellation,@FIndex,@FNote)";
                return await _dal.Execute(insertSql, subMessage) > 0;
            }
            else
            {
                var updateSql = "Update SubMessage set ParentId = @ParentId,FNumber = @FNumber,FName = @FName,IsCancellation = @IsCancellation,FIndex = @FIndex,FNote = @FNote Where ID = @ID";
                return await _dal.Execute(updateSql, subMessage) > 0;
            }
        }

        public async Task<SubMessage> GetSubMessage(string id)
        {
            var sql = "Select * from SubMessage Where ID = @ID";
            return await _dal.QueryFirst(sql, new { ID = id });
        }

        public async Task<IEnumerable<SubMessage>> GetSubMessageList(string parentId)
        {
            var sql = "Select * from SubMessage Where ParentId = @ParentId";
            return await _dal.QueryList(sql, new { ParentId = parentId });
        }

        public async Task<bool> DeleteSubMessage(string id)
        {
            var sql = "Delete from SubMessage Where ID = @ID";
            return await _dal.Execute(sql, new { ID = id })>0;
        }
    }
}
