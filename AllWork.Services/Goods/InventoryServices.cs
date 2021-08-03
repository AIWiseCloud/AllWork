using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class InventoryServices:Base.BaseServices<Inventory>,IInventoryServices
    {
        readonly IInventoryRepository _dal;
        public InventoryServices(IInventoryRepository inventoryRepository)
        {
            _dal = inventoryRepository;
        }
        public async Task<IEnumerable<Inventory>> GetInventories(string goodsId)
        {
            var res = await _dal.GetInventories(goodsId);
            return res;
        }

        public async Task<Tuple<IEnumerable<Inventory>, int>> SearchInventories(InventoryParams inventoryParams)
        {
            var res = await _dal.SearchInventories(inventoryParams);
            return res;
        }
    }
}
