using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Goods
{
    public interface IGoodsCategoryRepository:Base.IBaseRepository<GoodsCategory>
    {
        Task<bool> SaveGoodsCategory(GoodsCategory goodsCategory);

        Task<GoodsCategory> GetGoodsCategory(string categoryId);

        Task<bool> DeleteGoodsCategory(string categoryId);

        /// <summary>
        /// 获取商品下级分类
        /// </summary>
        /// <param name="parentId">上级分类ID (为空时获取1级分类，为*时所有分类, 其余情况为获取下级分类</param>
        /// <param name="onlyValidCategory">仅返回有效未作废的分类，默认不限制</param>
        /// <returns></returns>
        Task<IEnumerable<GoodsCategory>> GetGoodsCategories(string parentId, bool onlyValidCategory = false);
    }
}
