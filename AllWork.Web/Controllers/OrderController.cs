using AllWork.IServices.Order;
using AllWork.Model;
using AllWork.Model.Order;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly IOrderServices _orderServices;
        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="orderMain"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GenerateOrder(OrderMain orderMain)
        {
            var res = await _orderServices.GenerateOrder(orderMain);
            return Ok(res);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderInfo(long orderId)
        {
            var res = await _orderServices.GetOrderInfo(orderId);
            return Ok(res);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(long orderId)
        {
            var statusId = await _orderServices.GetOrderStatusId(orderId);
            if(statusId!=0 && statusId != -1)
            {
                return Ok(new OperResult {Status=false,ErrorMsg="订单所处状态不允许删除"});
            }
            var res = await _orderServices.DeleteOrder(orderId);
            return Ok(res);
        }

        /// <summary>
        /// 获取订单状态: 0待付款, 1待发货, 2待收货, 3已签收, -1已取消, -2已删除
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderStatusId(long orderId)
        {
            var res = await _orderServices.GetOrderStatusId(orderId);
            return Ok(res);
        }
    }
}
