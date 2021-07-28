using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface ISpuImgRepository:Base.IBaseRepository<SpuImg>
    {
        Task<OperResult> SaveSpuImg(SpuImg spuImg);

        Task<SpuImg> GetSpuImg(string id);

        Task<bool> DeleteSpuImg(string id);

        Task<IEnumerable<SpuImg>> GetSpuImgs(string goodsId);
    }
}
