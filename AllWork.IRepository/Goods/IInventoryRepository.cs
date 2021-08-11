using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IInventoryRepository:Base.IBaseRepository<Inventory>
    {
        //获取某spu下的库存商品列表
        Task<IEnumerable<Inventory>> GetInventories(string goodsId);

        //库存查询
        Task<Tuple<IEnumerable<Inventory>, int>> SearchInventories(InventoryParams inventoryParams);

        //需求项是否超出可用库存
        Task<OperResult> ComparisonActiveQuantity(List<RequireItem> requireItems);

        //获取SKU商品的可用库存
        Task<decimal> GetSKUActiveQuantity(string goodsId, string colorId, string specId);
    }
}
