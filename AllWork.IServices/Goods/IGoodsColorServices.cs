using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsColorServices:Base.IBaseServices<GoodsColor>
    {
        Task<bool> SaveGoodsColor(GoodsColor goodsColor);

        Task<GoodsColor> GetGoodsColor(string id);

        Task<bool> DeleteGoodsColor(string id);
    }
}
