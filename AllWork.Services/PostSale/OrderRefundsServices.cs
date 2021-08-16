using AllWork.IRepository.PostSale;
using AllWork.IServices.PostSale;
using AllWork.Model;
using AllWork.Model.PostSale;
using AllWork.Model.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.PostSale
{
    public class OrderRefundsServices:Base.BaseServices<OrderRefunds>,IOrderRefundsServices
    {
        readonly IOrderRefundsRepository _dal;
        public OrderRefundsServices(IOrderRefundsRepository orderRefundsRepository)
        {
            _dal = orderRefundsRepository;
        }

        public async Task<List<ReturnReason>> GetReturnReasons()
        {
            var res = await _dal.GetReturnReasons();
            return res;
        }

        /// <summary>
        /// 用户提交售后服务单
        /// </summary>
        /// <param name="orderRefunds"></param>
        /// <returns></returns>
        public async Task<OperResult> SaveOrderRefunes(OrderRefunds orderRefunds)
        {
            var res = await _dal.SaveOrderRefunes(orderRefunds);
            return res;
        }

        //根据订单号获取售后服务申请单列表
        public async Task<List<OrderRefunds>> GetOrderRefundsList(string orderId)
        {
            var res = await _dal.GetOrderRefundsList(orderId);
            return res;
        }

        public async Task<OrderRefunds> GetOrderRefunds(string postSaleId)
        {
            var res = await _dal.GetOrderRefunds(postSaleId);
            return res;
        }
    }
}
