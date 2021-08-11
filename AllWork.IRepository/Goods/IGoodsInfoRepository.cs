using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsInfoRepository:Base.IBaseRepository<GoodsInfo>
    {
        Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo);

        Task<GoodsInfoExt> GetGoodsInfo(string goodsId);

        Task<bool> DeleteGoodsInfo(string goodsId);

        Task<bool> ExistSKU(string goodsId);

        Task<Tuple<IEnumerable<GoodsInfoExt>, int>> QueryGoods(GoodsQueryParams goodsQueryParams);
    }
}
