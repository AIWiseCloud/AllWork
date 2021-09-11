using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsColorServices:Base.IBaseServices<GoodsColor>
    {
        Task<bool> SaveGoodsColor(GoodsColor goodsColor);

        Task<GoodsColor> GetGoodsColor(string id);

        Task<IEnumerable<GoodsColor>> GetGoodsColors(string goodsId);

        Task<OperResult> DeleteGoodsColor(string id);
    }
}
