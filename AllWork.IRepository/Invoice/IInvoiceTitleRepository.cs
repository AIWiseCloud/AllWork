using AllWork.Model.Invoice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IRepository.Invoice
{
    public interface IInvoiceTitleRepository:Base.IBaseRepository<InvoiceTitle>
    {
        Task<int> SaveInvoiceTitle(InvoiceTitle invoiceTitle);

        Task<InvoiceTitle> GetInvoiceTitle(string id);

        Task<int> DeleteInvoiceTitle(string id);

        Task<IEnumerable<InvoiceTitle>> GetInvoiceTitles(string unionId);
    }
}
