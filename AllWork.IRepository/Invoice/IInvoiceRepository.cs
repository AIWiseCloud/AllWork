using AllWork.Model;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mo = AllWork.Model.Invoice;

namespace AllWork.IRepository.Invoice
{
    public interface IInvoiceRepository : Base.IBaseRepository<mo.Invoice>
    {
        /// <summary>
        /// 申请开票
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        Task<OperResult> ApplyInvoicing(mo.Invoice invoice);

        Task<mo.Invoice> GetInvoice(string id);

        //开票
        Task<int> MakeInvoice(string id, string drawer, string invoiceUrl);

        //分页显示用户发票记录
        Task<Tuple<IEnumerable<mo.Invoice>, int>> QueryUserInvoices(CommonParams commonParams);
    }
}
