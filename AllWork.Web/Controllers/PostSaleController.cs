using AllWork.IServices.Order;
using AllWork.IServices.PostSale;
using AllWork.Model.PostSale;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 售后
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostSaleController : ControllerBase
    {
        readonly IOrderRefundsServices _orderRefundsServices;
        readonly IOrderServices _orderServices;
        public PostSaleController(IOrderRefundsServices orderRefundsServices, IOrderServices orderServices)
        {
            _orderRefundsServices = orderRefundsServices;
            _orderServices = orderServices;
        }

        /// <summary>
        /// 申请售后原因列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetReturnReasons()
        {
            var res = await _orderRefundsServices.GetReturnReasons();
            return Ok(res);
        }

        /// <summary>
        /// 用户提交售后服务申请单
        /// </summary>
        /// <param name="orderRefunds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveOrderRefunes(OrderRefunds orderRefunds)
        {
            if (string.IsNullOrEmpty(orderRefunds.PostSaleId))
            {
                orderRefunds.PostSaleId = AllWork.Common.Utils.CreateDigitSn();
            }
            if (orderRefunds.IsClosed == 1)
            {
                return BadRequest(new { msg = "此服务单已关闭" });
            }
            var orderId = long.Parse(orderRefunds.OrderId);
;            //订单状态
            var orderStatus = await _orderServices.GetOrderStatusId(orderId) ;
            //申请退款
            if (orderRefunds.CurrentType == 2)
            {
                if (orderStatus != 1)
                {
                    return BadRequest(new { msg = "订单在已支付、未发货的状态才能申请退款" });
                }
                var billid = await _orderServices.GetBillId(orderId);
                if (!string.IsNullOrEmpty(billid))
                {
                    return BadRequest(new { msg = $"订单{orderId}已拣货生成出库单{billid},不能申请退款，请联系客服处理" });
                }
            }
            //退换货
            if (orderRefunds.CurrentType == 1 || orderRefunds.CurrentType == 3)
            {
                if (orderStatus != 3)
                {
                    return BadRequest(new { msg = "要办理退换货，请先完成订单签收！" });
                }
            }
            //根据订单号获取售后服务单列表
            var bills = await _orderRefundsServices.GetOrderRefundsList(orderRefunds.OrderId);
            if (bills.Count > 0)
            {
                if (orderRefunds.CurrentType == 2 && bills.Find(x => x.PostSaleId != orderRefunds.PostSaleId && x.CurrentType == 2) != null)
                {
                    return BadRequest(new { msg = "系统已存在当前订单的退款申请单" });
                }
            }
            //若是换货，必须选择新商品颜色规格
            if(orderRefunds.CurrentType==3 && (string.IsNullOrEmpty(orderRefunds.ColorId) || string.IsNullOrEmpty(orderRefunds.SpecId)))
            {
                return BadRequest(new { msg = "请选择换货颜色规格" });
            }
            var res = await _orderRefundsServices.SaveOrderRefunes(orderRefunds);
            return Ok(res);
        }

        /// <summary>
        /// 获取售后服务申请单
        /// </summary>
        /// <param name="postSaleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderRefunds(string postSaleId)
        {
            var res = await _orderRefundsServices.GetOrderRefunds(postSaleId);
            return Ok(res);
        }
    }
}
