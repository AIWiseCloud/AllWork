using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public GoodsController(IGoodsCategoryServices goodsCategoryServices)
        {
            _goodsCategoryServices = goodsCategoryServices;
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
        /// 获取商品分类列表
        /// </summary>
        /// <param name="parentId">>上级分类ID (为空时获取1级分类，为*时所有分类, 其余情况为获取下级分类</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsCategories(string parentId)
        {
            var res = await _goodsCategoryServices.GetGoodsCategories(parentId);
            return Ok(res);
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
    }
}
