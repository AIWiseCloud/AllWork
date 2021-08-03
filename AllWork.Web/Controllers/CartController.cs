using AllWork.IServices.ShopCart;
using AllWork.Model.ShopCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 购物车
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController][Authorize]
    public class CartController : ControllerBase
    {
        readonly ICartServices _cartServices;
        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var res = await _cartServices.SaveCart(cart);
            return Ok(res);
        }

        /// <summary>
        /// 获取用户购物车列表
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCarts(string unionId)
        {
            var res = await _cartServices.GetCarts(unionId);
            return Ok(res);
        }

        /// <summary>
        /// 修改购物车数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditCartQuantity(string id,int quantity)
        {
            if(quantity<1 || quantity > 999)
            {
                return BadRequest("数量不合法!");
            }
            var res = await _cartServices.EditCartQuantity(id, quantity);
            return Ok(res);
        }

        /// <summary>
        /// 切换购物车项目选中状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ChangeCartItemSelected(string id,int selected)
        {
            var res = await _cartServices.ChangeCartItemSelected(id, selected);
            return Ok(res);
        }

        /// <summary>
        /// 批量删除购物车项
        /// </summary>
        /// <param name="cartIdList">购物车主键数组</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCartItems(IList<string> cartIdList)
        {
            var res = await _cartServices.DeleteCartItems(cartIdList);
            return Ok(res);
        }
    }
}
