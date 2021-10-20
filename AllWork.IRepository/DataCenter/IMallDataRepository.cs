using AllWork.Model.DataCenter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.DataCenter
{
    public interface IMallDataRepository:Base.IBaseRepository<MallData>
    {
        Task<IEnumerable<MallData>> GetMallData();
    }
}
