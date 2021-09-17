using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsInfoServices
    {
        Task<bool> SaveGoodsInfo(GoodsInfo goodsInfo);

        Task<GoodsInfo> GetGoodsInfo(string goodsId);

        Task<bool> ExistSKU(string goodsId);

        Task<OperResult> DeleteGoodsInfo(string goodsId);

        Task<OperResult> ReleaseGoods(bool isRelease, string goodsId);

        Task<Tuple<IEnumerable<GoodsInfoExt>, int>> QueryGoods(GoodsQueryParams goodsQueryParams);

        Task<IEnumerable<GoodsInfo>> GetGoodsList(string categoryId);

        Task<IEnumerable<string>> GetSpecList(string goodsId);

        Task<IEnumerable<string>> GetGoodsBrands(string goodsId, string specName);

        Task<IEnumerable<string>> GetGoodsMatchs(string goodsId, string specName, string brandName);

        Task<GoodsSpec> GetGoodsSpec(string goodsId, string sepcName, string brandName, string match);
    }
}
