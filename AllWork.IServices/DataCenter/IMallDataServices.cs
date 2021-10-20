using AllWork.Model.DataCenter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.DataCenter
{
    public interface IMallDataServices:Base.IBaseServices<MallData>
    {
        Task<IEnumerable<MallData>> GetMallData();
    }
}
