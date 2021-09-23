using AllWork.IRepository.Invoice;
using AllWork.Model.Invoice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Invoice
{
    public class InvoiceTitleRepository:Base.BaseRepository<InvoiceTitle>,IInvoiceTitleRepository
    {
        public async Task<int> SaveInvoiceTitle(InvoiceTitle invoiceTitle)
        {
            var instance = await base.QueryFirst("Select * from InvoiceTitle Where ID = @ID", invoiceTitle);
            string sql;
            if (instance == null)
            {
                sql = @"Insert InvoiceTitle (ID,UnionId,InvoiceType,TitleType,TitleName,ContentType,TaxId,RegisterAddress,RegisterTel,BankName,BankAccount,Collector,CollectorPhone,CollectorAddr,
CollectorMail)values
(@ID,@UnionId,@InvoiceType,@TitleType,@TitleName,@ContentType,@TaxId,@RegisterAddress,
@RegisterTel,@BankName,@BankAccount, @Collector,@CollectorPhone,@CollectorAddr, @CollectorMail)";
            }
            else
            {
                sql = @"Update InvoiceTitle set InvoiceType = @InvoiceType,TitleType = @TitleType,TitleName = @TitleName,ContentType = @ContentType,
TaxId = @TaxId,RegisterAddress = @RegisterAddress,RegisterTel = @RegisterTel,BankName = @BankName,BankAccount = @BankAccount, Collector = @Collector,
CollectorPhone = @CollectorPhone, CollectorAddr = @CollectorAddr,
CollectorMail = @CollectorMail Where ID = @ID";
            }
            return await base.Execute(sql, invoiceTitle);
        }

        public async Task<InvoiceTitle> GetInvoiceTitle(string id)
        {
            var sql = "Select * from InvoiceTitle Where ID = @ID";
            var res = await base.QueryFirst(sql, new { ID = id });
            return res;
        }

        public async Task<int> DeleteInvoiceTitle(string id)
        {
            var sql = "Delete from InvoiceTitle Where ID = @ID";
            var res = await base.Execute(sql, new { ID = id });
            return res;
        }

        public async Task<IEnumerable<InvoiceTitle>> GetInvoiceTitles(string unionId)
        {
            var sql = "Select * from InvoiceTitle Where UnionId = @UnionId";
            var res = await base.QueryList(sql, new { UnionId = unionId });
            return res;
        }
    }
}
