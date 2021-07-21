using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsColorRepository:Base.IBaseRepository<GoodsColor>
    {
        Task<bool> SaveGoodsColor(GoodsColor goodsColor);

        Task<GoodsColor> GetGoodsColor(string id);

        Task<bool> DeleteGoodsColor(string id);
    }
}
