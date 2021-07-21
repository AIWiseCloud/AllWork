using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsSpecRepository:Base.IBaseRepository<GoodsSpec>
    {
        Task<bool> SaveGoodsSpec(GoodsSpec goodsSpec);

        Task<GoodsSpec> GetGoodsSepc(string id);

        Task<bool> DeleteGoodsSpec(string id);
    }
}
