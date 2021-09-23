using AllWork.Model;
using AllWork.Model.Invoice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Invoice
{
    public interface IInvoiceTitleServices
    {
        Task<OperResult> SaveInvoiceTitle(InvoiceTitle invoiceTitle);

        Task<InvoiceTitle> GetInvoiceTitle(string id);

        Task<int> DeleteInvoiceTitle(string id);

        Task<IEnumerable<InvoiceTitle>> GetInvoiceTitles(string unionId);
    }
}
