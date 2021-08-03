using AllWork.Model;
using AllWork.Model.Order;
using System.Threading.Tasks;

namespace AllWork.IServices.Order
{
    public interface IOrderServices:Base.IBaseServices<OrderMain>
    {
        Task<OperResult> GenerateOrder(OrderMain orderMain);

        Task<OrderMainExt> GetOrderInfo(long orderId);

        Task<OperResult> DeleteOrder(long orderId);

        Task<int> GetOrderStatusId(long orderId);
    }
}
