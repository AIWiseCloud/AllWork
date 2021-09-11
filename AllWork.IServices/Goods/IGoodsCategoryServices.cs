using AllWork.Model;
using AllWork.Model.Goods;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.IServices.Goods
{
    public interface IGoodsCategoryServices:Base.IBaseServices<GoodsCategory>
    {
        Task<bool> SaveGoodsCategory(GoodsCategory goodsCategory);

        Task<GoodsCategory> GetGoodsCategory(string categoryId);

        Task<OperResult> DeleteGoodsCategory(string categoryId);

        /// <summary>
        /// 获取商品下级分类
        /// </summary>
        /// <param name="parentId">上级分类ID (为空时获取1级分类，为*时所有分类, 其余情况为获取下级分类</param>
        /// <param name="onlyValidCategory">仅返回有效未作废的分类，默认不限制</param>
        /// <returns></returns>
        Task<IEnumerable<GoodsCategory>> GetGoodsCategories(string parentId, bool onlyValidCategory=false);

        Task<IEnumerable<GoodsCategory>> GetSubcategories(string categoryId);

    }
}
