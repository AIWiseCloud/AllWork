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

        Task<bool> ExistOrders(string goodsId);

        Task<bool> ReleaseGoods(bool isRelease, string goodsId);

        Task<Tuple<IEnumerable<GoodsInfoExt>, int>> QueryGoods(GoodsQueryParams goodsQueryParams);

        Task<IEnumerable<GoodsInfo>> GetGoodsList(string categoryId);

        Task<IEnumerable<string>> GetSpecList(string goodsId);

        Task<IEnumerable<string>> GetGoodsBrands(string goodsId,string specName);

        Task<IEnumerable<string>> GetGoodsMatchs(string goodsId, string specName, string brandName);

        Task<GoodsSpec> GetGoodsSpec(string goodsId, string specName, string brandName, string match);
    }
}
