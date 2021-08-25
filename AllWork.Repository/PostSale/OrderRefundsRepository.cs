using AllWork.IRepository.PostSale;
using AllWork.Model;
using AllWork.Model.PostSale;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.PostSale
{
    public class OrderRefundsRepository:Base.BaseRepository<OrderRefunds>,IOrderRefundsRepository
    {
        public async Task<List<ReturnReason>> GetReturnReasons()
        {
            var res = await base.QueryList<ReturnReason>("Select * from ReturnReason order by FIndex");
            return res;
        }

        public async Task<OperResult> SaveOrderRefunes(OrderRefunds orderRefunds)
        {
            var instance = await base.QueryFirst("Select * from OrderRefunds Where PostSaleId = @PostSaleId", orderRefunds);
            string sql;
            if (instance == null)
            {
                sql = @"Insert OrderRefunds (PostSaleId,CurrentType,PaymentChannel,OrderId,OrderLineId,ColorId,SpecId,RefundTo,RefundResonId,BackQty,BackMoney,LogisticsId,
ExpressId,Images,RefundUser,RefundUserPhone,RefundAddress,BackMode,PickUpAddress)
values
(@PostSaleId,@CurrentType,@PaymentChannel,@OrderId,@OrderLineId,@ColorId,@SpecId,@RefundTo,@RefundResonId,@BackQty,@BackMoney,
@LogisticsId,@ExpressId,@Images,@RefundUser,@RefundUserPhone,@RefundAddress,@BackMode,@PickUpAddress )";
            }
            else
            {
                sql = @"Update OrderRefunds set PaymentChannel = @PaymentChannel,OrderId = @OrderId,OrderLineId = @OrderLineId,ColorId = @ColorId,SpecId = @SpecId,
RefundTo = @RefundTo,RefundResonId = @RefundResonId,BackQty = @BackQty,BackMoney = @BackMoney,LogisticsId = @LogisticsId,ExpressId = @ExpressId,Images = @Images,
RefundUser = @RefundUser,RefundUserPhone = @RefundUserPhone,RefundAddress = @RefundAddress,RefundRemark = @RefundRemark,BackMode = @BackMode,PickUpAddress = @PickUpAddress
Where PostSaleId = @PostSaleId";
            }
            var res = await base.Execute(sql, orderRefunds);
            return new OperResult { Status = res > 0, IdentityKey = orderRefunds.PostSaleId };
        }

        //根据订单号获取售后服务申请单列表
        public async Task<List<OrderRefunds>> GetOrderRefundsList(string orderId)
        {
            var res = await base.QueryList("Select * from OrderRefunds Where OrderId = @OrderId", new { OrderId = orderId });
            return res;
        }

        public async Task<OrderRefunds> GetOrderRefunds(string postSaleId)
        {
            var res = await base.QueryFirst("Select * from OrderRefunds Where PostSaleId = @PostSaleId ", new { PostSaleId = postSaleId });
            return res;
        }
    }
}
