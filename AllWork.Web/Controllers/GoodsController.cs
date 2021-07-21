using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 商品服务
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        readonly IGoodsCategoryServices _goodsCategoryServices;
        readonly IGoodsInfoServices _goodsInfoServices;
        readonly IGoodsColorServices _goodsColorServices;
        readonly IGoodsSpecServices _goodsSpecServices;
        public GoodsController(
            IGoodsCategoryServices goodsCategoryServices,
            IGoodsInfoServices goodsInfoServices,
            IGoodsColorServices goodsColorServices,
            IGoodsSpecServices goodsSpecServices
            )
        {
            _goodsCategoryServices = goodsCategoryServices;
            _goodsInfoServices = goodsInfoServices;
            _goodsColorServices = goodsColorServices;
            _goodsSpecServices = goodsSpecServices;
        }

        /// <summary>
        /// 保存商品分类
        /// </summary>
        /// <param name="goodsCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveGoodsCategory(GoodsCategory goodsCategory)
        {
            var res = await _goodsCategoryServices.SaveGoodsCategory(goodsCategory);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品分类记录
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsCategory(string categoryId)
        {
            var res = await _goodsCategoryServices.GetGoodsCategory(categoryId);
            return Ok(res);
        }

        /// <summary>
        /// 递归某商品分类产生该分类的树形
        /// </summary>
        /// <param name="parentCategory"></param>
        /// <param name="categories"></param>
        void ToTreeItems(GoodsCategory parentCategory, IEnumerable<GoodsCategory> categories)
        {
            parentCategory.Children = new List<GoodsCategory>();
            //下一级分类
            var items = new List<GoodsCategory>();
            foreach(var item in categories)
            {
                if (item.ParentId == parentCategory.CategoryId)
                {
                    items.Add(item);
                }
            }
            items.ForEach(item =>
            {
                var category = new GoodsCategory
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    ImgUrl = item.ImgUrl,
                    ParentId = item.ParentId,
                    Findex = item.Findex,
                    ShopId = item.ShopId,
                    IsCancellation = item.IsCancellation
                };
                parentCategory.Children.Add(category);
                //往下递归
                ToTreeItems(item, categories);
            });
        }

        /// <summary>
        /// 获取商品分类列表
        /// </summary>
        /// <param name="parentId">上级分类parentId为空时获取1级分类，为*时所有分类, 其余情况为获取下级分类</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsCategories(string parentId)
        {
            var res = await _goodsCategoryServices.GetGoodsCategories(parentId);

            if (parentId == "*")
            {
               
                var aitems = new List<GoodsCategory>();//第一级分类
                foreach (var item in res)
                {
                    if (string.IsNullOrEmpty(item.ParentId))
                    {
                        aitems.Add(item);
                    }
                }
                //为所有第一级分类递归获取所有子分类
                foreach (var item in aitems)
                {
                    ToTreeItems(item, res);
                }
                return Ok(aitems);
            }
            else
            {
                return Ok(res);
            }
            
        }

        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteGoodsCategory(string categoryId)
        {
            var res = await _goodsCategoryServices.DeleteGoodsCategory(categoryId);
            return Ok(res);
        }
        /// <summary>
        /// 保存商品信息
        /// </summary>
        /// <param name="goodsInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveGoodsInfo(GoodsInfo goodsInfo)
        {
            var res = await _goodsInfoServices.SaveGoodsInfo(goodsInfo);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsInfo(string goodsId)
        {
            var res = await _goodsInfoServices.GetGoodsInfo(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 删除商品信息(一并删除颜色及规格设置）
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteGoodsInfo(string goodsId)
        {
            var res = await _goodsInfoServices.DeleteGoodsInfo(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 保存商品颜色与图片记录
        /// </summary>
        /// <param name="goodsColor"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveGoodsColor(GoodsColor goodsColor)
        {
            var res = await _goodsColorServices.SaveGoodsColor(goodsColor);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品颜色与图片记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsColor(string id)
        {
            var res = await _goodsColorServices.GetGoodsColor(id);
            return Ok(res);
        }

        /// <summary>
        /// 删除一条商品颜色记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteGoodsColor(string id)
        {
            var res = await _goodsColorServices.DeleteGoodsColor(id);
            return Ok(res);
        }

        /// <summary>
        /// 保存商品规格及价格记录
        /// </summary>
        /// <param name="goodsSpec"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveGoodsSpec(GoodsSpec goodsSpec)
        {
            var res = await _goodsSpecServices.SaveGoodsSpec(goodsSpec);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品规格与价格记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsSpec(string id)
        {
            var res = await _goodsSpecServices.GetGoodsSepc(id);
            return Ok(res);
        }

        /// <summary>
        /// 删除一条商品规格与价格记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteGoodsSpec(string id)
        {
            var res = await _goodsSpecServices.DeleteGoodsSpec(id);
            return Ok(res);
        }

    }
}
