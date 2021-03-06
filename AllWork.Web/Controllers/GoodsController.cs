using AllWork.IServices.Goods;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Authorization;
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
        readonly ISpuImgServices _spuImgServices;
        public GoodsController(
            IGoodsCategoryServices goodsCategoryServices,
            IGoodsInfoServices goodsInfoServices,
            IGoodsColorServices goodsColorServices,
            IGoodsSpecServices goodsSpecServices,
            ISpuImgServices spuImgServices
            )
        {
            _goodsCategoryServices = goodsCategoryServices;
            _goodsInfoServices = goodsInfoServices;
            _goodsColorServices = goodsColorServices;
            _goodsSpecServices = goodsSpecServices;
            _spuImgServices = spuImgServices;
        }

        /// <summary>
        /// 保存商品分类
        /// </summary>
        /// <param name="goodsCategory"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> SaveGoodsCategory(GoodsCategory goodsCategory)
        {
            var res = await _goodsCategoryServices.SaveGoodsCategory(goodsCategory);
            return Ok(res);
        }

        /// <summary>
        /// 获取指定商品分类
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
            foreach (var item in categories)
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
                    IsMainMaterial=item.IsMainMaterial,
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
        /// <param name="parentId">上级分类parentId为空时获取第1级分类，为*时所有分类, 其余情况为获取下级分类</param>
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
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> DeleteGoodsCategory(string categoryId)
        {
            var res = await _goodsCategoryServices.DeleteGoodsCategory(categoryId);
            return Ok(res);
        }
        /// <summary>
        /// 保存商品基本信息
        /// </summary>
        /// <param name="goodsInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> SaveGoodsInfo(GoodsInfo goodsInfo)
        {
            var res = await _goodsInfoServices.SaveGoodsInfo(goodsInfo);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品完整信息
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
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> DeleteGoodsInfo(string goodsId)
        {
            var res = await _goodsInfoServices.DeleteGoodsInfo(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 复制商品
        /// </summary>
        /// <param name="goodsId">商品ID</param>
        /// <param name="newGoodsId">新商品ID</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> CopyGoodsInfo(string goodsId, string newGoodsId,string userName)
        {
            GoodsInfoExt goodsInfo = await _goodsInfoServices.GetGoodsInfo(goodsId) as GoodsInfoExt;
            goodsInfo.GoodsId = newGoodsId;
            goodsInfo.Creator = userName;
            goodsInfo.GoodsDesc +=  "(复制生成)";
            var resSave = await _goodsInfoServices.SaveGoodsInfo(goodsInfo);
            foreach (var item in goodsInfo.GoodsColors)
            {
                item.GoodsId = newGoodsId;
                item.ID = System.Guid.NewGuid().ToString();
                item.Creator = userName;
                await _goodsColorServices.SaveGoodsColor(item);
            }
            foreach(var item in goodsInfo.GoodsSpecs)
            {
                item.GoodsId = newGoodsId;
                item.ID = System.Guid.NewGuid().ToString();
                item.Creator = userName;
                await _goodsSpecServices.SaveGoodsSpec(item);
            }
            foreach (var item in goodsInfo.SpuImgs)
            {
                item.GoodsId = newGoodsId;
                item.ID = System.Guid.NewGuid().ToString();
                await _spuImgServices.SaveSpuImg(item);
            }

            return Ok(resSave);
        }

        /// <summary>
        /// 发布(上架)商品
        /// </summary>
        /// <param name="isRelease">true发布,false取消发布</param>
        /// <param name="goodsId">商品ID</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> ReleaseGoods(bool isRelease, string goodsId)
        {
            var res = await _goodsInfoServices.ReleaseGoods(isRelease, goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品列表（用于下载批量更新单价的模板)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> GetAllGoodsInfo()
        {
            var res = await _goodsInfoServices.GetAllGoodsInfo();
            return Ok(res);
        }

        /// <summary>
        /// 批量更新商品价格（用于通过Excel上传批量更新)
        /// </summary>
        /// <param name="goodsInfos"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> BatchUpdatePrice(List<GoodsInfo> goodsInfos)
        {
            var res = await _goodsInfoServices.BatchUpdatePrice(goodsInfos);
            return Ok(res);
        }

        /// <summary>
        /// 保存商品颜色与图片记录
        /// </summary>
        /// <param name="goodsColor"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
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
        /// 获取商品所有颜色图片记录
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsColors(string goodsId)
        {
            var res = await _goodsColorServices.GetGoodsColors(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 判断商品是否有SKU记录
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExistSKU(string goodsId)
        {
            var res = await _goodsInfoServices.ExistSKU(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 删除一条商品颜色记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> DeleteGoodsColor(string id)
        {
            var res = await _goodsColorServices.DeleteGoodsColor(id);
            return Ok(res);
        }

        /// <summary>
        /// 商品查询
        /// </summary>
        /// <param name="goodsQueryParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryGoods(GoodsQueryParams goodsQueryParams)
        {
            var res = await _goodsInfoServices.QueryGoods(goodsQueryParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 保存商品规格及价格记录
        /// </summary>
        /// <param name="goodsSpec"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
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
        /// 获取商品所有规格及定价记录
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsSpecs(string goodsId)
        {
            var res = await _goodsSpecServices.GetGoodsSpecs(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 删除一条商品规格与价格记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> DeleteGoodsSpec(string id)
        {
            var res = await _goodsSpecServices.DeleteGoodsSpec(id);
            return Ok(res);
        }

        /// <summary>
        /// 保存SPU图片
        /// </summary>
        /// <param name="spuImg"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> SaveSpuImg(SpuImg spuImg)
        {
            var res = await _spuImgServices.SaveSpuImg(spuImg);
            return Ok(res);
        }

        /// <summary>
        /// 获取SPU图片记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSpuImg(string id)
        {
            var res = await _spuImgServices.GetSpuImg(id);
            return Ok(res);
        }

        /// <summary>
        /// 获取商品所S有spu记录
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSpuImgs(string goodsId)
        {
            var res = await _spuImgServices.GetSpuImgs(goodsId);
            return Ok(res);
        }

        /// <summary>
        /// 删除SPU图片记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> DeleteSpuImg(string id)
        {
            var res = await _spuImgServices.DeleteSpuImg(id);
            return Ok(res);
        }

        /// <summary>
        /// 获取分类下的商品列表（商品ID,商品名称、品牌、配比、价格等）
        /// </summary>
        /// <param name="categoryId">商品分类ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsList(string categoryId)
        {
            var res = await _goodsInfoServices.GetGoodsList(categoryId);
            return Ok(res);
        }

        /// <summary>
        /// 材料报价表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGoodsQuotes()
        {
            var res = await _goodsInfoServices.GetGoodsQuotes();
            return Ok(new { QuoteTitle = res.Item1[0], Items = res.Item2 });
        }

        /// <summary>
        /// 更新报价说明
        /// </summary>
        /// <param name="quoteExplain"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(policy: "Editor")]
        public async Task<IActionResult> UpdateQuoteExplain(QuoteExplain quoteExplain)
        {
            var res = await _goodsInfoServices.UpdateQuoteExplain(quoteExplain);
            return Ok(res > 0);
        }

        
    }
}
