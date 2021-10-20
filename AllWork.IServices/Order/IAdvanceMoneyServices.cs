using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Order
{
    public interface IAdvanceMoneyServices:Base.IBaseServices<AdvanceMoney>
    {
        AdvanceMoney GetPrebuiltInfo(long orderId, string unionId, decimal orderAmt);

        Task<IEnumerable<AdvanceMoney>> GetAdvanceMoneys(long orderId, string unionId);

        Task<int> SubmitAdvanceMoney(AdvanceMoney advanceMoney);

        Task<OperResult> ConfirmReceipt(long id, string userName, int isConfirm, string paytime);

        Task<Tuple<IEnumerable<AdvanceMoneyExt>, int>> QueryAdvanceMoney(AMQueryParams queryParams);
    }
}
