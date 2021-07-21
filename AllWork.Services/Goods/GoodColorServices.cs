using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class GoodColorServices:Base.BaseServices<GoodsColor>,IGoodsColorServices
    {
        readonly IGoodsColorRepository _dal;

        public GoodColorServices(IGoodsColorRepository goodsColorRepository)
        {
            _dal = goodsColorRepository;
        }

        public async Task<bool> SaveGoodsColor(GoodsColor goodsColor)
        {
            var res= await _dal.SaveGoodsColor(goodsColor);
            return res;
        }

        public async Task<GoodsColor> GetGoodsColor(string id)
        {
            var res = await _dal.GetGoodsColor(id);
            return res;
        }

        public async Task<bool> DeleteGoodsColor(string id)
        {
            var res = await _dal.DeleteGoodsColor(id);
            return res;
        }

    }
}
