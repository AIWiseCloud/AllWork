using AllWork.IServices.Sys;
using AllWork.Model.Sys;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 店铺
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        readonly IShopServices _shopServices;
        public ShopController(IShopServices shopServices)
        {
            _shopServices = shopServices;
        }

        /// <summary>
        /// 保存店铺设置
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveShop([FromBody] Shop shop)
        {
            var res = await _shopServices.SaveShop(shop);
            return Ok(res);
        }

        /// <summary>
        /// 获取店铺设置
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetShop(string shopId)
        {
            var res = await _shopServices.GetShop(shopId);
            return Ok(res);
        }

        /// <summary>
        /// 获取所有店铺记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var res = await _shopServices.GetShops();
            return Ok(res);
        }

        /// <summary>
        /// 删除店铺设置
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteShop(string shopId)
        {
            var res = await _shopServices.DeleteShop(shopId);
            return Ok(res);
        }
    }
}
