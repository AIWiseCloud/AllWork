using AllWork.IRepository.Invoice;
using AllWork.IServices.Invoice;
using AllWork.Model;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mo = AllWork.Model.Invoice;

namespace AllWork.Services.Invoice
{
    public class InvoiceServices:Base.BaseServices<mo.Invoice>,IInvoiceServices
    {
        readonly IInvoiceRepository _dal;
        public InvoiceServices(IInvoiceRepository invoiceRepository)
        {
            _dal = invoiceRepository;
        }

        //申请开票
        public async Task<OperResult> ApplyInvoicing(mo.Invoice invoice)
        {
            if (string.IsNullOrEmpty(invoice.ID))
            {
                invoice.ID = Guid.NewGuid().ToString();
            }
            var res = await _dal.ApplyInvoicing(invoice);
            res.IdentityKey = invoice.ID;
            return res;
        }

        public async Task<mo.Invoice> GetInvoice(string id)
        {
            var res = await _dal.GetInvoice(id);
            return res;
        }

        //开票
        public async Task<int> MakeInvoice(string id, string drawer, string invoiceUrl)
        {
            var res = await _dal.MakeInvoice(id, drawer, invoiceUrl);
            return res;
        }

        //分页显示用户发票记录
        public async Task<Tuple<IEnumerable<mo.Invoice>, int>> QueryUserInvoices(InvoiceQueryParams commonParams)
        {
            var res = await _dal.QueryUserInvoices(commonParams);
            return res;
        }
    }
}
