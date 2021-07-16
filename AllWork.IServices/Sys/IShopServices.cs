using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IShopServices:Base.IBaseServices<Shop>
    {
        public Task<OperResult> SaveShop(Shop shop);

        public Task<IEnumerable<Shop>> GetShops();

        public Task<Shop> GetShop(string shopId);

        public Task<bool> DeleteShop(string shopId);
    }
}
