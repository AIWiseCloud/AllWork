using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
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

        public async Task<IEnumerable<GoodsColor>> GetGoodsColors(string goodsId)
        {
            var res = await _dal.GetGoodsColors(goodsId);
            return res;
        }

        public async Task<OperResult> DeleteGoodsColor(string id)
        {
            var hasData = await _dal.ExistInventory(id);
            if (hasData)
            {
                return new OperResult { Status = false, ErrorMsg = "已存在库存记录" };
            }
            var res = await _dal.DeleteGoodsColor(id);
            return new OperResult { Status = res };
        }

    }
}
