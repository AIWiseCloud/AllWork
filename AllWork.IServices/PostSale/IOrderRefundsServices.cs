using AllWork.Model;
using AllWork.Model.PostSale;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.PostSale
{
    public interface IOrderRefundsServices:Base.IBaseServices<OrderRefunds>
    {
        Task<List<ReturnReason>> GetReturnReasons();

        Task<OperResult> SaveOrderRefunes(OrderRefunds orderRefunds);

        //根据订单号获取售后服务申请单列表
        Task<List<OrderRefunds>> GetOrderRefundsList(string orderId);

        Task<OrderRefunds> GetOrderRefunds(string postSaleId);


    }
}
