using AllWork.IRepository.Invoice;
using AllWork.IServices.Invoice;
using AllWork.Model;
using AllWork.Model.Invoice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Invoice
{
    public class InvoiceTitleServices : Base.BaseServices<InvoiceTitle>, IInvoiceTitleServices
    {
        readonly IInvoiceTitleRepository _dal;
        public InvoiceTitleServices(IInvoiceTitleRepository invoiceTitleRepository)
        {
            _dal = invoiceTitleRepository;
        }

        public async Task<OperResult> SaveInvoiceTitle(InvoiceTitle invoiceTitle)
        {
            var result = new OperResult { Status = false, IdentityKey = invoiceTitle.ID };
            var res = await _dal.SaveInvoiceTitle(invoiceTitle);
            if (invoiceTitle.InvoiceType == 2 && (string.IsNullOrEmpty(invoiceTitle.TaxId) || string.IsNullOrEmpty(invoiceTitle.RegisterAddress) || string.IsNullOrEmpty(invoiceTitle.RegisterTel)
                || string.IsNullOrEmpty(invoiceTitle.BankName) || string.IsNullOrEmpty(invoiceTitle.BankAccount)))
            {
                result.ErrorMsg = "发票类型为增值税发票时必须完整提供公司税号、注册地址、注册电话、银行名称、银行账号";
                return result;
            }
            if (invoiceTitle.TitleType == 1 && (string.IsNullOrEmpty(invoiceTitle.TaxId) || string.IsNullOrEmpty(invoiceTitle.CollectorPhone)))
            {
                result.ErrorMsg = "抬头类型为单位时必须完整提供企业税号、收票人手机";
                return result;
            }

            result.Status = res > 0;
            return result;
        }

        public async Task<InvoiceTitle> GetInvoiceTitle(string id)
        {
            var res = await _dal.GetInvoiceTitle(id);
            return res;
        }

        public async Task<int> DeleteInvoiceTitle(string id)
        {
            var res = await _dal.DeleteInvoiceTitle(id);
            return res;
        }

        public async Task<IEnumerable<InvoiceTitle>> GetInvoiceTitles(string unionId)
        {
            var res = await _dal.GetInvoiceTitles(unionId);
            return res;
        }
    }
}
