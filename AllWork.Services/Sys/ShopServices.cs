using AllWork.IRepository.Sys;
using AllWork.IServices.Sys;
using AllWork.Model;
using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Sys
{
    public class ShopServices : Base.BaseServices<Shop>, IShopServices
    {
        readonly IShopRepository _dal;

        public ShopServices(IShopRepository shopRepository)
        {
            _dal = shopRepository;
        }

        public async Task<OperResult> SaveShop(Shop shop)
        {
            var res = await _dal.SaveShop(shop);
            return res;
        }

        public async Task<IEnumerable<Shop>> GetShops()
        {
            var res = await _dal.GetShops();
            return res;
        }

        public async Task<Shop> GetShop(string shopId)
        {
            var res = await _dal.GetShop(shopId);
            return res;
        }

        public async Task<bool> DeleteShop(string shopId)
        {
            var res = await _dal.DeleteShop(shopId);
            return res;
        }
    }
}
