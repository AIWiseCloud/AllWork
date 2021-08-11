using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IInventoryServices:Base.IBaseServices<Inventory>
    {
        Task<IEnumerable<Inventory>> GetInventories(string goodsId);

        Task<Tuple<IEnumerable<Inventory>, int>> SearchInventories(InventoryParams inventoryParams);

        Task<OperResult> ComparisonActiveQuantity(List<RequireItem> requireItems);

        Task<decimal> GetSKUActiveQuantity(string goodsId, string colorId, string specId);
    }
}
