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
            var res = await _dal.SaveSubMesType(subMesType);
            return res;
        }

        public async Task<SubMesType> GetSubMesType(string id)
        {
            var res = await _dal.GetSubMesType(id);
            return res;
        }

        public async Task<bool> DeleteSubMesType(string id)
        {
            var res = await _dal.DeleteSubMesType(id);
            return res;
        }

        public async  Task<IEnumerable<SubMesType>> GetSubMesTypes()
        {
            var res = await _dal.GetSubMesTypes();
            return res;
        }

    }
}
