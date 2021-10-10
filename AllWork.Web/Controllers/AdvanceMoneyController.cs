using AllWork.IServices.Order;
using AllWork.Model;
using AllWork.Model.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 预付定金
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AdvanceMoneyController : ControllerBase
    {
        readonly IAdvanceMoneyServices _advanceMoneyServices;
        public AdvanceMoneyController(IAdvanceMoneyServices advanceMoneyServices)
        {
            _advanceMoneyServices = advanceMoneyServices;
        }

        /// <summary>
        /// 获取定金预生成信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="unionId">用户标识</param>
        /// <param name="orderAmt">订单金额</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPrebuiltInfo(long orderId, string unionId, decimal orderAmt)
        {
            var res = _advanceMoneyServices.GetPrebuiltInfo(orderId, unionId, orderAmt);
            return Ok(res);
        }

        /// <summary>
        /// 获取订单对应的定金记录
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="unionId">用户标识</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdvanceMoneys(long orderId, string unionId)
        {
            var res = await _advanceMoneyServices.GetAdvanceMoneys(orderId, unionId);
            return Ok(res);
        }

        /// <summary>
        /// 生成预付定金记录
        /// </summary>
        /// <param name="advanceMoney"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitAdvanceMoney(AdvanceMoney advanceMoney)
        {
            var result = new OperResult { Status = false };
            if(advanceMoney.PaymentWay==1 && string.IsNullOrEmpty(advanceMoney.PayVoucherUrl))
            {
                result.ErrorMsg = "支付方式为线下对公转账时请上传付款凭证";
            }
            else
            {
                var res = await _advanceMoneyServices.SubmitAdvanceMoney(advanceMoney);
                result.Status = res > 0;
            }
            return Ok(result);
        }

        /// <summary>
        /// 客服确认定金到账
        /// </summary>
        /// <param name="id">预付单号</param>
        /// <param name="userName">客服</param>
        /// <param name="isConfirm">确认(0取消确认,1确认)</param>
        /// <param name="paytime">支付日期</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> ConfirmReceipt(long id, string userName, int isConfirm, string paytime)
        {
            var res = await _advanceMoneyServices.ConfirmReceipt(id, userName, isConfirm, paytime);
            return Ok(res);
        }
    }
}
