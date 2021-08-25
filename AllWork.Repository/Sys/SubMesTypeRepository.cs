using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Sys
{
    public class SubMesTypeRepository:Base.BaseRepository<SubMesType>, IRepository.Sys.ISubMesTypeRepository
    {
        public async Task<bool> SaveSubMesType(SubMesType subMesType)
        {
            var instance = await base.QueryFirst("Select * from SubMesType Where ID = @ID", new { subMesType.ID });
            if (instance == null)
            {
                var insertSql = "Insert SubMesType (ID,FName)values(@ID,@FName)";
                return await base.Execute(insertSql, subMesType) > 0;
            }
            else
            {
                var updateSql = "Update SubMesType set ID = @ID,FName = @FName Where ID = @ID";
                return await base.Execute(updateSql, subMesType) > 0;
            }
        }

        public async Task<SubMesType> GetSubMesType(string id)
        {
            var sql = "Select * from SubMesType Where ID = @ID";
            var res = await base.QueryFirst(sql, new { ID = id });
            return res;
        }

        public async Task<bool> DeleteSubMesType(string id)
        {
            var sql = "Delete from SubMesType Where ID = @ID";
            var res = await base.Execute(sql, new { ID = id }) > 0;
            return res;
        }

        public async Task<IEnumerable<SubMesType>> GetSubMesTypes()
        {
            var sql = "Select * from SubMesType";
            var res = await base.QueryList(sql);
            return res;
        }
    }
}
