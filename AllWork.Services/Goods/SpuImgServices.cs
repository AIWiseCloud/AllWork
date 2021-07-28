using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class SpuImgServices : Base.BaseServices<SpuImg>, ISpuImgServices
    {
        readonly ISpuImgRepository _dal;

        public SpuImgServices(ISpuImgRepository spuImgRepository)
        {
            _dal = spuImgRepository;
        }

        public async Task<OperResult> SaveSpuImg(SpuImg spuImg)
        {
            var res = await _dal.SaveSpuImg(spuImg);
            return res;
        }

        public async Task<SpuImg> GetSpuImg(string id)
        {
            var res = await _dal.GetSpuImg(id);
            return res;
        }

        public async Task<IEnumerable<SpuImg>> GetSpuImgs(string goodsId)
        {
            var res = await _dal.GetSpuImgs(goodsId);
            return res;
        }

        public async Task<bool> DeleteSpuImg(string id)
        {
            var res = await _dal.DeleteSpuImg(id);
            return res;
        }
    }
}