using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Order
{
    public interface IOrderRepository:Base.IBaseRepository<OrderMain>
    {
        Task<OperResult> GenerateOrder(OrderMain orderMain);

        Task<OrderMainExt> GetOrderInfo(long orderId);

        Task<OperResult> DeleteOrder(long orderId);

        //取消订单
        Task<OperResult> CancelOrder(long orderId);

        //订单发货
        Task<OperResult> DeliveryOrder(OrderDeliveryParams orderDeliveryParams);

        //签收订单
        Task<int> SignatureOrder(long orderId);

        Task<bool> PaySuccess(long orderId, string tradeNo);

        Task<int> GetOrderStatusId(long orderId);

        Task<Tuple<IEnumerable<OrderMainExt>, int>> QueryOrders(OrderQueryParams orderQueryParams);

        Task<dynamic> GetMyTodoList(string unionId);

        Task<bool> UpdateOrderAddress(UpdateOrderAddressParams updateOrderAddressParams);

        //由订单号获取对应的销售出库单号
        Task<string> GetBillId(long orderId);
    }
}
