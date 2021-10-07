using AllWork.IRepository.Order;
using AllWork.Model;
using AllWork.Model.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Order
{
    public class AdvanceMoneyRepository:Base.BaseRepository<AdvanceMoney>,IAdvanceMoneyRepository
    {
        public async Task<IEnumerable<AdvanceMoney>> GetAdvanceMoneys(long orderId, string unionId)
        {
            var sql = "Select * from AdvanceMoney Where OrderId = @OrderId and UnionId = @UnionId";
            var res = await base.QueryList(sql, new { OrderId = orderId, UnionId = unionId });
            return res;
        }

        public async Task<int> SubmitAdvanceMoney(AdvanceMoney advanceMoney)
        {
            var instance = await base.QueryFirst("Select * from AdvanceMoney Where ID = @ID", new { advanceMoney.ID });
            string sql;
            if (instance == null)
            {
                sql = @"Insert AdvanceMoney (ID,OrderId,UnionId,DownPayment,summary,PaymentWay,PaymentChannel,PayTime,TradeNo,PayVoucherUrl)values
(@ID,@OrderId,@UnionId,@DownPayment,@summary,@PaymentWay,@PaymentChannel,@PayTime,@TradeNo,@PayVoucherUrl)";
            }
            else
            {
                sql = @"Update AdvanceMoney set OrderId = @OrderId,UnionId = @UnionId,DownPayment = @DownPayment,summary = @summary,PaymentWay = @PaymentWay,
PaymentChannel = @PaymentChannel,PayTime = @PayTime,TradeNo = @TradeNo,PayVoucherUrl = @PayVoucherUrl Where ID = @ID";
            }
            var res = await base.Execute(sql, advanceMoney);
            return res;
        }

        //客服手工确认到账 （在线支付成功后的自动确认写在了订单的PaySuccess中)
        public async Task<OperResult>  ConfirmReceipt(long id, string userName, int isConfirm,string paytime)
        {
            var sql1 = "update AdvanceMoney set ConfirmStatus = @ConfirmStatus, Confirmer = @Confirmer, PayTime = @PayTime where ID = @ID ";
            var sql2 = "update OrderMain a, AdvanceMoney b set a.DownPayment = a.DownPayment + b.DownPayment * @Way where a.OrderId = b.OrderId and b.ID = @ID and StatusID = 0";
            var sqlparams = new
            {
                ID = id,
                Way = isConfirm == 0 ? -1 : 1,
                ConfirmStatus = isConfirm,
                Confirmer = isConfirm == 0 ? string.Empty : userName,
                PayTime = isConfirm == 0 ? null : paytime
            };

            var tranitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql1,sqlparams),
                new Tuple<string, object>(sql2,sqlparams)
            };
            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }
    }
}
