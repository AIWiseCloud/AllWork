using AllWork.IRepository.Invoice;
using AllWork.Model;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using mo = AllWork.Model.Invoice;

namespace AllWork.Repository.Invoice
{
    public class InvoiceRepository : Base.BaseRepository<mo.Invoice>, IInvoiceRepository
    {
        /// <summary>
        /// 申请开票
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public async Task<OperResult> ApplyInvoicing(mo.Invoice invoice)
        {
            var instance = await base.QueryFirst("Select * from Invoice Where ID = @ID", invoice);
            string sql;
            if (instance == null)
            {
                sql = @"Insert Invoice (ID,OrderId,InvoAmt,UnionId,StatusId,ApplyTime,InvoiceType,ContentType,TitleType,TitleName,TaxId,RegisterAddress,RegisterTel,
BankName,BankAccount,Collector,CollectorPhone,CollectorAddr,CollectorMail)values
(@ID,@OrderId,@InvoAmt,@UnionId,@StatusId,current_timestamp,@InvoiceType,@ContentType,@TitleType,@TitleName,@TaxId,@RegisterAddress,@RegisterTel,@BankName,@BankAccount,
@Collector,@CollectorPhone,@CollectorAddr,@CollectorMail)";
            }
            else
            {
                sql = @"Update Invoice set InvoAmt = @InvoAmt,UnionId = @UnionId,
InvoiceType = @InvoiceType,ContentType = @ContentType,TitleType = @TitleType,TitleName = @TitleName,TaxId = @TaxId,RegisterAddress = @RegisterAddress,RegisterTel = @RegisterTel,
BankName = @BankName,BankAccount = @BankAccount,Collector = @Collector,CollectorPhone = @CollectorPhone,CollectorAddr = @CollectorAddr,CollectorMail = @CollectorMail
 Where ID = @ID";
            }
            //更新订单开票标记
            var sql2 = "Update OrderMain set InvoiceStatus = 1 Where OrderId = @OrderId";

            var listsql = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql,invoice),
                new Tuple<string, object>(sql2,new{invoice.OrderId})
            };
            var res = await base.ExecuteTransaction(listsql);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        public async Task<mo.Invoice> GetInvoice(string id)
        {
            var res = await base.QueryFirst("Select * from Invoice Where ID = @ID or OrderId = @OrderId", new { ID = id, OrderId = id });
            return res;
        }

        //财务确认已开票
        public async Task<int> MakeInvoice(string id, string drawer, string invoiceUrl)
        {
            var sql = "Update Invoice set Drawer = @Drawer, InvoiceUrl = @InvoiceUrl, InvoiceTime = current_timestamp, StatusId=1 Where ID = @ID";
            var res = await base.Execute(sql, new { ID = id, Drawer = drawer, InvoiceUrl = invoiceUrl });
            return res;
        }

        //分页显示用户发票记录
        public async Task<Tuple<IEnumerable<mo.Invoice>, int>> QueryUserInvoices(CommonParams commonParams)
        {
            //sql公共部分
            var sqlpub = new StringBuilder(" from Invoice a ");

            //固定排序
            string sqlorder = " Order by OrderId desc ";
            //求记录数
            var sql1 = "Select Count(a.ID) as totalCount " + sqlpub.ToString();
            //获取分页数据
            var sql2 = "Select * " + sqlpub.ToString() + sqlorder + " limit @Skip, @PageSize ";
            //完整sql
            var sql = sql1 + " ; " + sql2;

            var res = await base.QueryPagination<mo.Invoice>(sql,
                new
                {
                    commonParams.PageModel.Skip,
                    commonParams.PageModel.PageSize
                });
            return res;
        }
    }
}
