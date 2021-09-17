using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using System.Collections.Generic;
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
            goodsSpec.DiscountPrice = goodsSpec.Price;
            var res = await _dal.SaveGoodsSpec(goodsSpec);
            return res;
        }

        public async Task<GoodsSpec> GetGoodsSepc(string id)
        {
            var res = await _dal.GetGoodsSepc(id);
            return res;
        }

        public async Task<IEnumerable<GoodsSpec>> GetGoodsSpecs(string goodsId)
        {
            var res = await _dal.GetGoodsSpecs(goodsId);
            return res;
        }

        public async Task<OperResult> DeleteGoodsSpec(string id)
        {
            var hasDat = await _dal.ExistInventory(id);
            if (hasDat)
            {
                return new OperResult { Status = false, ErrorMsg = "已存在库存记录" };
            }
            var res = await _dal.DeleteGoodsSpec(id);
            return new OperResult { Status = res };
        }
    }
}
