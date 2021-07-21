using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Sys
{
    public interface IShopServices : Base.IBaseServices<Shop>
    {
        Task<OperResult> SaveShop(Shop shop);

        Task<IEnumerable<Shop>> GetShops();

        Task<Shop> GetShop(string shopId);

        Task<bool> DeleteShop(string shopId);
    }
}
