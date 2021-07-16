using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class SubMesTypeServices:Base.BaseServices<SubMesType>,ISubMesTypeServices
    {
        readonly ISubMesTypeRepository _dal;

        public SubMesTypeServices(ISubMesTypeRepository subMesTypeRepository)
        {
            _dal = subMesTypeRepository;
        }

        public async Task<bool> SaveSubMesType(SubMesType subMesType)
        {
            var instance = await _dal.QueryFirst("Select * from SubMesType Where ID = @ID", new { ID = subMesType.ID });
            if (instance == null)
            {
                var insertSql = "Insert SubMesType (ID,FName)values(@ID,@FName)";
                return await _dal.Execute(insertSql, subMesType) > 0;
            }
            else
            {
                var updateSql = "Update SubMesType set ID = @ID,FName = @FName Where ID = @ID";
                return await _dal.Execute(updateSql, subMesType) > 0;
            }
        }

        public async Task<SubMesType> GetSubMesType(string id)
        {
            var sql = "Select * from SubMesType Where ID = @ID";
            var res = await _dal.QueryFirst(sql, new { ID = id });
            return res ;
        }

        public async Task<bool> DeleteSubMesType(string id)
        {
            var sql = "Delete from SubMesType Where ID = @ID";
            var res = await _dal.Execute(sql, new { ID = id })>0;
            return res;
        }

        public async  Task<IEnumerable<SubMesType>> GetSubMesTypes()
        {
            var sql = "Select * from SubMesType";
            var res = await _dal.QueryList(sql);
            return res;
        }

    }
}
