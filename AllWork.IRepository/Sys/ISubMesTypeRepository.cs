using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface ISubMesTypeRepository:Base.IBaseRepository<SubMesType>
    {
        Task<bool> SaveSubMesType(SubMesType subMesType);

        Task<SubMesType> GetSubMesType(string id);

        Task<bool> DeleteSubMesType(string id);

        Task<IEnumerable<SubMesType>> GetSubMesTypes();
    }
}
