using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsSpecServices:Base.IBaseServices<GoodsSpec>
    {
        Task<bool> SaveGoodsSpec(GoodsSpec goodsSpec);

        Task<GoodsSpec> GetGoodsSepc(string id);

        Task<IEnumerable<GoodsSpec>> GetGoodsSpecs(string goodsId);

        Task<OperResult> DeleteGoodsSpec(string id);
    }
}
