using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Order
{
    public interface IAdvanceMoneyRepository:Base.IBaseRepository<AdvanceMoney>
    {
        /// <summary>
        /// 获取订单的定金支付记录
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="unionId"></param>
        /// <returns></returns>
        Task<IEnumerable<AdvanceMoney>> GetAdvanceMoneys(long orderId, string unionId);

        Task<int> SubmitAdvanceMoney(AdvanceMoney advanceMoney);

        Task<OperResult> ConfirmReceipt(long id, string userName, int isConfirm,string paytime);

        Task<Tuple<IEnumerable<AdvanceMoneyExt>, int>> QueryAdvanceMoney(AMQueryParams queryParams);
    }
}
