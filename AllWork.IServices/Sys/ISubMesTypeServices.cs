using AllWork.Model;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface ISubMesTypeServices:Base.IBaseServices<SubMesType>
    {
        Task<bool> SaveSubMesType(SubMesType subMesType);

        Task<SubMesType> GetSubMesType(string id);

        Task<bool> DeleteSubMesType(string id);

        Task<IEnumerable<SubMesType>> GetSubMesTypes();
    }
}
