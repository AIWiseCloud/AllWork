using AllWork.IRepository.Order;
using AllWork.IServices.Order;
using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Order
{
    public class OrderServices : Base.BaseServices<OrderMain>, IOrderServices
    {
        readonly IOrderRepository _dal;
        public OrderServices(IOrderRepository orderRepository)
        {
            _dal = orderRepository;
        }

        public async Task<OperResult> GenerateOrder(OrderMain orderMain)
        {
            var res = await _dal.GenerateOrder(orderMain);
            return res;
        }

        public async Task<OrderMainExt> GetOrderInfo(long orderId)
        {
            var res = await _dal.GetOrderInfo(orderId);
            return res;
        }

        public async Task<OperResult> DeleteOrder(long orderId)
        {
            var res = await _dal.DeleteOrder(orderId);
            return res;
        }

        public async Task<OperResult> CancelOrder(long orderId)
        {
            var res = await _dal.CancelOrder(orderId);
            return res;
        }

        //订单发货
        public async Task<OperResult> DeliveryOrder(OrderDeliveryParams orderDeliveryParams)
        {
            var orderStatus = await _dal.GetOrderStatusId(orderDeliveryParams.OrderId);
            if ((orderDeliveryParams.IsDelivery == 1 && orderStatus != 1) || (orderDeliveryParams.IsDelivery == 0 && orderStatus != 2))
            {
                return new OperResult { Status = false, ErrorMsg = "当前订单状态不能执行此操作!" };
            }
            //var billId = await _dal.GetBillId(orderDeliveryParams.OrderId);
            //if(string.IsNullOrEmpty(billId) || orderDeliveryParams.BillId.CompareTo(billId) != 0)
            //{
            //    return new OperResult { Status = false, ErrorMsg = "销售出库单号不正确!" };
            //}
            var res = await _dal.DeliveryOrder(orderDeliveryParams);
            return res;
        }

        public async Task<OperResult> ConfirmPay(long orderId, int isConfirm)
        {
            var result = new OperResult { Status = false };
            var orderStatus = await _dal.GetOrderStatusId(orderId);
            if ((isConfirm == 1 && orderStatus != 0) || (isConfirm == 0 && orderStatus != 1))
            {
                result.ErrorMsg = "当前订单状态不能执行此操作!";
                return result;
            }
            var res = await _dal.ConfirmPay(orderId, isConfirm);
            result.Status = res > 0;
            return result;
        }

        //签收订单
        public async Task<int> SignatureOrder(long orderId)
        {
            var res = await _dal.SignatureOrder(orderId);
            return res;
        }

        public async Task<int> GetOrderStatusId(long orderId)
        {
            var res = await _dal.GetOrderStatusId(orderId);
            return res;
        }

        //支付成功（支付结果通知中调用)
        public async Task<bool> PaySuccess(string orderId, string tradeNo)
        {
            var res = await _dal.PaySuccess(long.Parse(orderId), tradeNo);
            return res;
        }

        public async Task<Tuple<IEnumerable<OrderMainExt>, int>> QueryOrders(OrderQueryParams orderQueryParams)
        {
            var res = await _dal.QueryOrders(orderQueryParams);
            return res;
        }

        public async Task<dynamic> GetMyTodoList(string unionId)
        {
            var res = await _dal.GetMyTodoList(unionId);
            return res;
        }

        public async Task<bool> UpdateOrderAddress(UpdateOrderAddressParams updateOrderAddressParams)
        {
            var res = await _dal.UpdateOrderAddress(updateOrderAddressParams);
            return res;
        }

        //由订单号获取对应的销售出库单号
        public async Task<string> GetBillId(long orderId)
        {
            var res = await _dal.GetBillId(orderId);
            return res;
        }

        /// <summary>
        /// 上传打款凭证
        /// </summary>
        /// <param name="orderAttach"></param>
        /// <returns></returns>
        public async Task<bool> UploadOrderAttach(OrderAttach orderAttach)
        {
            var res = await _dal.UploadOrderAttach(orderAttach);
            return res > 0;
        }

        public async Task<OperResult> AdjustOrderPrice(long orderId, int lineId, decimal newPrice)
        {
            var result = new OperResult { Status = false };
            var orderStatus = await _dal.GetOrderStatusId(orderId);
            if (orderStatus != 0)
            {
                result.ErrorMsg = "当前订单状态不能执行此操作!";
                return result;
            }

            var res = await _dal.AdjustOrderPrice(orderId, lineId, newPrice);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }
        //订单调数量
        public async Task<OperResult> AdjustOrderQuantity(long orderId, int lineId, decimal newQty)
        {
            var result = new OperResult { Status = false };
            var orderStatus = await _dal.GetOrderStatusId(orderId);
            if (orderStatus != 0)
            {
                result.ErrorMsg = "当前订单状态不能执行此操作!";
                return result;
            }

            var res = await _dal.AdjustOrderQuantity(orderId, lineId, newQty);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }
    }
}
