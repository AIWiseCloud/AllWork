using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsSpecServices:Base.IBaseServices<GoodsSpec>
    {
        Task<bool> SaveGoodsSpec(GoodsSpec goodsSpec);

        Task<GoodsSpec> GetGoodsSepc(string id);

        Task<bool> DeleteGoodsSpec(string id);
    }
}
