using AllWork.IRepository.Address;
using AllWork.IServices.Address;
using AllWork.Model;
using AllWork.Model.Address;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Address
{
    public class ReceiveAddressServices : Base.BaseServices<ReceiveAddress>, IReceiveAddressServices
    {
        readonly IReceiveAddressRepository _dal;
        public ReceiveAddressServices(IReceiveAddressRepository receiveAddressRepository)
        {
            _dal = receiveAddressRepository;
        }
        public async Task<OperResult> SaveReceiveAddress(ReceiveAddress receiveAddress)
        {
            var res = await _dal.SaveReceiveAddress(receiveAddress);
            return res;
        }

        public async Task<OperResult> SetDefaultAddress(string unionId, string addrId)
        {
            var res = await _dal.SetDefaultAddress(unionId, addrId);
            return res;
        }

        public async Task<OperResult> DeleteReceiveAddress(string unionId, string addrId)
        {
            var res = await _dal.DeleteReceiveAddress(unionId, addrId);
            return res;
        }

        public async Task<IEnumerable<ReceiveAddress>> GetReceiveAddresses(string unionId)
        {
            var res = await _dal.GetReceiveAddresses(unionId);
            return res;
        }
    }
}
