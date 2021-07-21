using AllWork.IRepository.Goods;
using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Services.Goods
{
    public class GoodsCategoryServices : Base.BaseServices<GoodsCategory>, IGoodsCategoryServices
    {
        readonly IGoodsCategoryRepository _dal;

        public GoodsCategoryServices(IGoodsCategoryRepository goodsCategoryRepository)
        {
            _dal = goodsCategoryRepository;
        }

        public async Task<bool> SaveGoodsCategory(GoodsCategory goodsCategory)
        {
            var res = await _dal.SaveGoodsCategory(goodsCategory);
            return res;
        }

        public async Task<GoodsCategory> GetGoodsCategory(string categoryId)
        {
            var res = await _dal.GetGoodsCategory(categoryId);
            return res;
        }

        public async Task<bool> DeleteGoodsCategory(string categoryId)
        {
            var res = await _dal.DeleteGoodsCategory(categoryId);
            return res;
        }

        /// <summary>
        /// 获取商品下级分类
        /// </summary>
        /// <param name="parentId">上级分类ID (为空时获取1级分类，为*时所有分类, 其余情况为获取下级分类</param>
        /// <param name="onlyValidCategory">仅返回有效未作废的分类，默认不限制</param>
        /// <returns></returns>
        public async Task<IEnumerable<GoodsCategory>> GetGoodsCategories(string parentId, bool onlyValidCategory = false)
        {
            var res = await _dal.GetGoodsCategories(parentId, onlyValidCategory);
            return res;
        }
    }
}
