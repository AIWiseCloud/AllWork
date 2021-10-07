using AllWork.Model;
using AllWork.Model.Address;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Address
{
    public interface IReceiveAddressRepository:Base.IBaseRepository<ReceiveAddress>
    {
        Task<OperResult> SaveReceiveAddress(ReceiveAddress receiveAddress);

        Task<OperResult> SetDefaultAddress(string unionId, string addrId);

        Task<OperResult> DeleteReceiveAddress(string unionId, string addrId);

        Task<IEnumerable<ReceiveAddress>> GetReceiveAddresses(string unionId);

        //获取某层级中特定行政区域的下级区域列表
        Task<IEnumerable<object>> GetAreas(int level, string currentId);
    }
}
