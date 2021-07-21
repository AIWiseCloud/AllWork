using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class GoodsSpecServices:Base.BaseServices<GoodsSpec>,IGoodsSpecServices
    {
        readonly IGoodsSpecRepository _dal;
        public GoodsSpecServices(IGoodsSpecRepository goodsSpecRepository)
        {
            _dal = goodsSpecRepository;
        }
        public async Task<bool> SaveGoodsSpec(GoodsSpec goodsSpec)
        {
            var res = await _dal.SaveGoodsSpec(goodsSpec);
            return res;
        }

        public async Task<GoodsSpec> GetGoodsSepc(string id)
        {
            var res = await _dal.GetGoodsSepc(id);
            return res;
        }

        public async Task<bool> DeleteGoodsSpec(string id)
        {
            var res = await _dal.DeleteGoodsSpec(id);
            return res;
        }
    }
}
