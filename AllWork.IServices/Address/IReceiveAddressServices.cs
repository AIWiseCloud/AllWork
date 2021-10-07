using AllWork.Model;
using AllWork.Model.Address;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Address
{
    public interface IReceiveAddressServices:Base.IBaseServices<ReceiveAddress>
    {
        Task<OperResult> SaveReceiveAddress(ReceiveAddress receiveAddress);

        Task<OperResult> SetDefaultAddress(string unionId, string addrId);

        Task<OperResult> DeleteReceiveAddress(string unionId, string addrId);

        Task<IEnumerable<ReceiveAddress>> GetReceiveAddresses(string unionId);

        Task<IEnumerable<object>> GetAreas(int level, string currentId);
    }
}
