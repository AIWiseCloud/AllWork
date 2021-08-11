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
    public class OrderServices:Base.BaseServices<OrderMain>,IOrderServices
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
        public async Task<int> DeliveryOrder(long orderId)
        {
            var res = await _dal.DeliveryOrder(orderId);
            return res;
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
    }
}
