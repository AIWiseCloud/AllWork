using AllWork.IRepository.Order;
using AllWork.IServices.Order;
using AllWork.Model;
using AllWork.Model.Order;
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

        public async Task<int> GetOrderStatusId(long orderId)
        {
            var res = await _dal.GetOrderStatusId(orderId);
            return res;
        }
    }
}
