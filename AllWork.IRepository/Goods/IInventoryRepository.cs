using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using AllWork.Model.ViewModel;
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
    }
}
