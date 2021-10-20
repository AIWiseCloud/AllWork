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

        /// <summary>
        /// 材料价格表
        /// </summary>
        /// <returns></returns>
        Task<Tuple<List<QuoteExplain>,List<GoodsQuote>>> GetGoodsQuotes();

        Task<int> UpdateQuoteExplain(QuoteExplain quoteExplain);

        Task<IEnumerable<GoodsInfo>> GetAllGoodsInfo();

        Task<Tuple<bool,string>> BatchUpdatePrice(List<GoodsInfo> goodsInfos);
    }
}
