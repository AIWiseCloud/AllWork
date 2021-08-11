using AllWork.Model;
using AllWork.Model.PostSale;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.PostSale
{
    public interface IOrderRefundsRepository : Base.IBaseRepository<OrderRefunds>
    {
        Task<List<ReturnReason>> GetReturnReasons();

        Task<OperResult> SaveOrderRefunes(OrderRefunds orderRefunds);

        Task<OrderRefunds> GetOrderRefunds(string postSaleId);

        //根据订单号获取售后服务申请单列表
        Task<List<OrderRefunds>> GetOrderRefundsList(string orderId);
    }
}
