using AllWork.IServices.Invoice;
using AllWork.Model.Invoice;
using AllWork.Model.RequestParams;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AllWork.Web.Controllers
{
    /// <summary>
    /// 发票中心
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        readonly IInvoiceTitleServices _invoiceTitleServices;
        readonly IInvoiceServices _invoiceServices;
        public InvoiceController(IInvoiceTitleServices invoiceTitleServices, IInvoiceServices invoiceServices)
        {
            _invoiceTitleServices = invoiceTitleServices;
            _invoiceServices = invoiceServices;
        }

        /// <summary>
        /// 保存发票抬头
        /// </summary>
        /// <param name="invoiceTitle"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveInvoiceTitle(InvoiceTitle invoiceTitle)
        {
            if (string.IsNullOrEmpty(invoiceTitle.ID))
            {
                invoiceTitle.ID = Guid.NewGuid().ToString();
            }
            var res = await _invoiceTitleServices.SaveInvoiceTitle(invoiceTitle);
            return Ok(res);

        }

        /// <summary>
        /// 获取发票抬头信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInvoiceTitle(string id)
        {
            var res = await _invoiceTitleServices.GetInvoiceTitle(id);
            return Ok(res);
        }

        /// <summary>
        /// 删除发票抬头信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteInvoiceTitle(string id)
        {
            var res = await _invoiceTitleServices.DeleteInvoiceTitle(id);
            return Ok(res > 0);
        }

        /// <summary>
        /// 获取用户所有发票抬头记录
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInvoiceTitles(string unionId)
        {
            var res = await _invoiceTitleServices.GetInvoiceTitles(unionId);
            return Ok(res);
        }

        /// <summary>
        /// 客户提交开票申请（索取发票）
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ApplyInvoicing(Invoice invoice)
        {
            var res = await _invoiceServices.ApplyInvoicing(invoice);
            return Ok(res);
        }

        /// <summary>
        /// 获取开票记录
        /// </summary>
        /// <param name="id">记录ID或是订单号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInvoice(string id)
        {
            var res = await _invoiceServices.GetInvoice(id);
            return Ok(res);
        }

        /// <summary>
        /// 财务确认已开票
        /// </summary>
        /// <param name="id">开票申请ID</param>
        /// <param name="drawer">开票人</param>
        /// <param name="invoiceUrl">发票图片</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> MakeInvoice(string id, string drawer, string invoiceUrl)
        {
            var res = await _invoiceServices.MakeInvoice(id, drawer, invoiceUrl);
            return Ok(res);
        }

        /// <summary>
        /// 分页显示用户发票记录
        /// </summary>
        /// <param name="commonParams"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QueryUserInvoices(CommonParams commonParams)
        {
            var res = await _invoiceServices.QueryUserInvoices(commonParams);
            return Ok(res);
        }
    }
}
