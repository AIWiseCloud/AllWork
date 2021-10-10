using AllWork.IRepository.Goods;
using AllWork.Model.Goods;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class GoodsCategoryRepository : Base.BaseRepository<GoodsCategory>, IGoodsCategoryRepository
    {
        public async Task<bool> SaveGoodsCategory(GoodsCategory goodsCategory)
        {
            var instance = await base.QueryFirst("Select * from GoodsCategory Where CategoryId = @CategoryId", new { goodsCategory.CategoryId });
            if (instance == null)
            {
                var insertSql = "Insert GoodsCategory (CategoryId,CategoryName,ShopId,ImgUrl,ParentId,IsMainMaterial,Findex,IsCancellation)values(@CategoryId,@CategoryName,@ShopId,@ImgUrl,@ParentId,@IsMainMaterial, @Findex,@IsCancellation)";
                return await base.Execute(insertSql, goodsCategory) > 0;
            }
            else
            {
                var updateSql = "Update GoodsCategory set CategoryName = @CategoryName,ShopId = @ShopId,ImgUrl = @ImgUrl,ParentId = @ParentId, IsMainMaterial=@IsMainMaterial, Findex = @Findex,IsCancellation = @IsCancellation Where CategoryId = @CategoryId";
                return await base.Execute(updateSql, goodsCategory) > 0;
            }
        }

        public async Task<GoodsCategory> GetGoodsCategory(string categoryId)
        {
            var sql = "Select * from GoodsCategory Where CategoryId = @CategoryId";
            return await base.QueryFirst(sql, new { CategoryId = categoryId });
        }

        public async Task<bool> DeleteGoodsCategory(string categoryId)
        {
            var sql = "Delete from GoodsCategory Where CategoryId = @CategoryId";
            return await base.Execute(sql, new { CategoryId = categoryId }) > 0;
        }

        /// <summary>
        /// 获取商品下级分类
        /// </summary>
        /// <param name="parentId">上级分类ID (为空时获取1级分类，为*时所有分类, 其余情况为获取下级分类</param>
        /// <param name="onlyValidCategory">仅返回有效未作废的分类，默认不限制</param>
        /// <returns></returns>
        public async Task<IEnumerable<GoodsCategory>> GetGoodsCategories(string parentId, bool onlyValidCategory = false)
        {
            StringBuilder sql = new StringBuilder("Select * from GoodsCategory Where 1 = 1");
            //一级分类条件
            if (string.IsNullOrEmpty(parentId))
            {
                sql.Append(" and IFNULL(ParentId,'') = '' ");
            }
            else if (parentId != "*")//指定分类
            {
                sql.Append(" and ParentId = @ParentId");
            }
            //如果仅返回有效（未作废）的分类
            if (onlyValidCategory)
            {
                sql.Append(" and IsCancellation = 0");
            }
            //按设定索引排序
            sql.Append(" Order by Findex");
            var res = await base.QueryList(sql.ToString(), new { ParentId = parentId });

            return res;
        }

        //分类下是否存在商品
        public async Task<bool> CategoryExistGoods(string categoryId)
        {
            var sql = "Select count(*) from GoodsInfo Where CategoryId = @CategoryId";
            var res = await base.ExecuteScalar<int>(sql, new { CategoryId = categoryId });
            return res > 0;
        }

        //获取下一级分类
        public async Task<IEnumerable<GoodsCategory>> GetSubcategories(string categoryId)
        {
            string sql;
            if (string.IsNullOrEmpty(categoryId))
            {
                sql = "Select * from GoodsCategory Where IFNULL(ParentId,'') = '' ";
            }
            else
            {
                sql = "Select * from GoodsCategory Where ParentId = @CategoryId";
            }
            var res = await base.QueryList(sql, new { CategoryId = categoryId });
            return res;
        }

    }
}
