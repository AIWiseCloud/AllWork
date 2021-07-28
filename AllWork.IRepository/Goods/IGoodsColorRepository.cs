using AllWork.Model.Goods;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsColorRepository:Base.IBaseRepository<GoodsColor>
    {
        Task<bool> SaveGoodsColor(GoodsColor goodsColor);

        Task<GoodsColor> GetGoodsColor(string id);

        Task<bool> DeleteGoodsColor(string id);

        Task<IEnumerable<GoodsColor>> GetGoodsColors(string goodsId);
    }
}
