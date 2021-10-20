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
    public class AdvanceMoneyServices:Base.BaseServices<AdvanceMoney>, IAdvanceMoneyServices
    {
        readonly IAdvanceMoneyRepository _dal;
        public AdvanceMoneyServices(IAdvanceMoneyRepository advanceMoneyRepository)
        {
            _dal = advanceMoneyRepository;
        }

        public AdvanceMoney GetPrebuiltInfo(long orderId, string unionId, decimal orderAmt)
        {
            var advanceMoney = new AdvanceMoney
            {
                ID = long.Parse("6"+ Common.Utils.CreateDigitSn()),//6开头表示定金记录 (在支付结果通知中要据此判断）
                OrderId = orderId,
                UnionId = unionId,
                DownPayment = orderAmt * 0.2m,
                Summary = $"支付{orderId}订单20%定金",
                PaymentWay = 0,
                PaymentChannel= "wechat"
            };
            return advanceMoney;
        }

        public async Task<IEnumerable<AdvanceMoney>> GetAdvanceMoneys(long orderId, string unionId)
        {
            var res = await _dal.GetAdvanceMoneys(orderId, unionId);
            return res;
        }

        public async Task<int> SubmitAdvanceMoney(AdvanceMoney advanceMoney)
        {
            var res = await _dal.SubmitAdvanceMoney(advanceMoney);
            return res;
        }

        public async Task<OperResult> ConfirmReceipt(long id, string userName, int isConfirm, string paytime)
        {
            var res = await _dal.ConfirmReceipt(id, userName, isConfirm, paytime);
            return res;
        }

        public async Task<Tuple<IEnumerable<AdvanceMoneyExt>, int>> QueryAdvanceMoney(AMQueryParams queryParams)
        {
            var res = await _dal.QueryAdvanceMoney(queryParams);
            return res;
        }
    }
}
