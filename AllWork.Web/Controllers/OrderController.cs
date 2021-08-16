using AllWork.IServices.Goods;
using AllWork.IServices.Order;
using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        readonly IInventoryServices _inventoryServices;
        public OrderController(IOrderServices orderServices, IInventoryServices inventoryServices)
        {
            _orderServices = orderServices;
            _inventoryServices = inventoryServices;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="orderMain"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GenerateOrder(OrderMain orderMain)
        {
            //比对可用库存
            var reqitems = new List<RequireItem>();
            foreach(var item in orderMain.OrderList)
            {
                reqitems.Add(new RequireItem { GoodsId = item.GoodsId, ColorId = item.ColorId, SpecId = item.SpecId, Quantity = item.Quantity });
            }
            var checkresult = await _inventoryServices.ComparisonActiveQuantity(reqitems);
            if (!checkresult.Status)
            {
                return BadRequest(new { msg = checkresult.ErrorMsg });
            }
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
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> CancelOrder(long orderId)
        {
            var statusId = await _orderServices.GetOrderStatusId(orderId);
            if (statusId != 0 && statusId != 0)
            {
                return Ok(new OperResult { Status = false, ErrorMsg = "订单所处状态不允许取消" });
            }
            var res = await _orderServices.CancelOrder(orderId);
            return Ok(res);
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="orderDeliveryParams"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> DeliveryOrder(OrderDeliveryParams orderDeliveryParams)
        {
            var res = await _orderServices.DeliveryOrder(orderDeliveryParams);
            return Ok(res);
        }

        /// <summary>
        /// 订单签收
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> SignatureOrder(long orderId)
        {
            var res = await _orderServices.SignatureOrder(orderId);
            return Ok(res > 0);
        }

        /// <summary>
        /// 获取订单状态: 0待付款, 1待发货, 2待收货, 3已签收, -1已取消, -2已删除
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns> 0待付款, 1待发货, 2待收货, 3已签收, -1已取消, -2已删除</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderStatusId(long orderId)
        {
            var res = await _orderServices.GetOrderStatusId(orderId);
            return Ok(res);
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="orderQueryParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryOrders(OrderQueryParams orderQueryParams)
        {
            var res = await _orderServices.QueryOrders(orderQueryParams);
            return Ok(new { totalCount = res.Item2, items = res.Item1 });
        }

        /// <summary>
        /// 获取我的待办
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMyTodoList(string unionId)
        {
            var res = await _orderServices.GetMyTodoList(unionId);
            return Ok(res);
        }

        /// <summary>
        /// 修改订单收货地址信息 (未发货前）
        /// </summary>
        /// <param name="updateOrderAddressParams"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrderAddress(UpdateOrderAddressParams updateOrderAddressParams)
        {
            var statusId = await _orderServices.GetOrderStatusId(long.Parse(updateOrderAddressParams.OrderId));
            if (statusId > 1)
            {
                return BadRequest("只有未支发货前的订单才可以修改地址信息");
            }
            var res = await _orderServices.UpdateOrderAddress(updateOrderAddressParams);
            return Ok(res);
        }

        /// <summary>
        /// 由订单号获取对应的销售出库单号
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBillId(long orderId)
        {
            var res = await _orderServices.GetBillId(orderId);
            return Ok(res);
        }
    }
}
