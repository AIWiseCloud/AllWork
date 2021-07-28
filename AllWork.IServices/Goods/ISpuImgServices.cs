using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface ISpuImgServices:Base.IBaseServices<SpuImg>
    {
        Task<OperResult> SaveSpuImg(SpuImg spuImg);

        Task<SpuImg> GetSpuImg(string id);

        Task<IEnumerable<SpuImg>> GetSpuImgs (string goodsId);

        Task<bool> DeleteSpuImg(string id);
    }
}
