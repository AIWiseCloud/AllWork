using AllWork.IRepository.Address;
using AllWork.Model;
using AllWork.Model.Address;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Address
{
    public class ReceiveAddressRepository : Base.BaseRepository<ReceiveAddress>, IReceiveAddressRepository
    {
        public async Task<OperResult> SaveReceiveAddress(ReceiveAddress receiveAddress)
        {
            var instance = await base.QueryFirst("Select * from ReceiveAddress Where AddrId = @AddrId", new { receiveAddress.AddrId });
            var tranitems = new List<Tuple<string, object>>();
            var insertsql = "Insert ReceiveAddress (AddrId,UnionId,Receiver,Label,PhoneNumber,Province,City,County,DetailsAddress,IsDefault)values(@AddrId,@UnionId,@Receiver,@Label,@PhoneNumber,@Province,@City,@County,@DetailsAddress,1)";
            var updatesql = "Update ReceiveAddress set Receiver = @Receiver,Label = @Label,PhoneNumber = @PhoneNumber,Province = @Province,City = @City,County = @County,DetailsAddress = @DetailsAddress,IsDefault = 1 Where AddrId = @AddrId";
            var othersql = "Update ReceiveAddress Set IsDefault = 0 Where UnionId = @UnionId and AddrId != @AddrId";
            if (instance == null)
            {
                tranitems.Add(new Tuple<string, object>(insertsql, receiveAddress));
            }
            else
            {
                tranitems.Add(new Tuple<string, object>(updatesql, receiveAddress));
            }
            tranitems.Add(new Tuple<string, object>(othersql, receiveAddress));
            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2, IdentityKey = receiveAddress.AddrId };
        }

        public async Task<OperResult> SetDefaultAddress(string unionId, string addrId)
        {
            var sql1 = "Update ReceiveAddress set IsDefault = 1 Where AddrId = @AddrId";
            var sql2 = "Update ReceiveAddress Set IsDefault = 0 Where UnionId = @UnionId and Addrid != @AddrId";
            var tranitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql1, new { AddrId = addrId }),
                new Tuple<string, object>(sql2, new { AddrId = addrId, UnionId = unionId })
            };
            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        public async Task<OperResult> DeleteReceiveAddress(string unionId, string addrId)
        {
            var sql = "Delete from ReceiveAddress Where UnionId = @UnionId and AddrId = @Addrid";
            var res = await base.Execute(sql, new { UnionId = unionId, AddrId = addrId });
            return new OperResult { Status = res > 0 };
        }

        public async Task<IEnumerable<ReceiveAddress>> GetReceiveAddresses(string unionId)
        {
            var sql = "Select * from ReceiveAddress Where UnionId = @UnionId order by IsDefault";
            var res = await base.QueryList(sql, new { UnionId = unionId });
            return res;

        }
    }
}
