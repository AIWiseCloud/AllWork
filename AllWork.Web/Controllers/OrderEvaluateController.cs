using AllWork.IServices.Order;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 商品评价
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderEvaluateController : ControllerBase
    {
        readonly IOrderEvaluateServices _orderEvaluateServices;
        public OrderEvaluateController(IOrderEvaluateServices orderEvaluateServices)
        {
            _orderEvaluateServices = orderEvaluateServices;
        }

        /// <summary>
        /// 提交订单行的评价
        /// </summary>
        /// <param name="orderEvaluate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitOrderEvaluate(OrderEvaluate orderEvaluate)
        {
            if (string.IsNullOrEmpty(orderEvaluate.ID))
            {
                orderEvaluate.ID = System.Guid.NewGuid().ToString();
            }
            var res = await _orderEvaluateServices.SubmitOrderEvaluate(orderEvaluate);
            res.IdentityKey = orderEvaluate.ID;
            return Ok(res);
        }

        /// <summary>
        /// 店铺回复
        /// </summary>
        /// <param name="id">评价ID</param>
        /// <param name="reply">评论内容</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ShopReply(string id, string reply)
        {
            var res = await _orderEvaluateServices.ShopReply(id, reply);
            return Ok(res>0);
        }

        /// <summary>
        /// 隐藏评价
        /// </summary>
        /// <param name="id">评价ID</param>
        /// <param name="isHide">是否隐藏(1隐藏,0显示)</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> HideEvaluate(string id, int isHide)
        {
            var res = await _orderEvaluateServices.HideEvaluate(id, isHide);
            return Ok(res>0);
        }

        /// <summary>
        /// 分页获取商品评价
        /// </summary>
        /// <param name="goodsEvaluatePraams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryGoodsEvaluates(GoodsEvaluatePraams goodsEvaluatePraams)
        {
            var res = await _orderEvaluateServices.QueryGoodsEvaluates(goodsEvaluatePraams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 获取订单评价
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderEvaluates(long orderId)
        {
            var res = await _orderEvaluateServices.GetOrderEvaluates(orderId);
            return Ok(res);
        }
    }
}
