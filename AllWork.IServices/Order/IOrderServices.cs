using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Order
{
    public interface IOrderServices:Base.IBaseServices<OrderMain>
    {
        Task<OperResult> GenerateOrder(OrderMain orderMain);

        Task<OrderMainExt> GetOrderInfo(long orderId);

        Task<OperResult> DeleteOrder(long orderId);

        Task<OperResult> CancelOrder(long orderId);

        //订单发货
        Task<int> DeliveryOrder(long orderId);

        //签收订单
        Task<int> SignatureOrder(long orderId);

        Task<int> GetOrderStatusId(long orderId);

        Task<bool> PaySuccess(string orderId, string tradNo);

        Task<Tuple<IEnumerable<OrderMainExt>,int>> QueryOrders(OrderQueryParams orderQueryParams);

        Task<dynamic> GetMyTodoList(string unionId);

        Task<bool> UpdateOrderAddress(UpdateOrderAddressParams updateOrderAddressParams);
    }
}
