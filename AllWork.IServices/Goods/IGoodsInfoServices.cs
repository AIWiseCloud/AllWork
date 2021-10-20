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

        Task<Tuple<List<QuoteExplain>, List<GoodsQuote>>> GetGoodsQuotes();

        Task<int> UpdateQuoteExplain(QuoteExplain quoteExplain);

        Task<IEnumerable<GoodsInfo>> GetAllGoodsInfo();

        Task<OperResult> BatchUpdatePrice(List<GoodsInfo> goodsInfos);
    }
}
