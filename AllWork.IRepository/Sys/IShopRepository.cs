using AllWork.Model;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Sys
{
    public interface IShopRepository : Base.IBaseRepository<AllWork.Model.Sys.Shop>
    {
        Task<OperResult> SaveShop(Shop shop);

        Task<IEnumerable<Shop>> GetShops();

        Task<Shop> GetShop(string shopId);

        Task<bool> DeleteShop(string shopId);
    }
}
