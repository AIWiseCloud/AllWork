using AllWork.Model;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mo = AllWork.Model.Invoice;

namespace AllWork.IServices.Invoice
{
    public interface IInvoiceServices:Base.IBaseServices<mo.Invoice>
    {
        Task<OperResult> ApplyInvoicing(mo.Invoice invoice);

        Task<mo.Invoice> GetInvoice(string id);

        //开票
        Task<int> MakeInvoice(string id, string drawer, string invoiceUrl);

        //分页显示用户发票记录
        Task<Tuple<IEnumerable<mo.Invoice>, int>> QueryUserInvoices(InvoiceQueryParams commonParams);
    }
}
