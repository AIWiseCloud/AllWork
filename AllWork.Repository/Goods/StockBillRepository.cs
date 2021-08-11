using AllWork.IRepository.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class StockBillRepository : Base.BaseRepository<StockBill>, IStockBillRepository
    {
        public StockBillRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<OperResult> SaveStockBill(StockBill stockBill)
        {
            var instance = await base.QueryFirst("Select * from StockBill Where BillId = @BillId", new { stockBill.BillId });
            List<Tuple<string, object>> tranitems = new List<Tuple<string, object>>();
            string msql = null;
            if (instance == null)
            {
                msql = "Insert StockBill (BillId,TransTypeId,Remark,OrderId, Creator)values(@BillId,@TransTypeId,@Remark,@OrderId, @Creator)";
            }
            else
            {
                msql = "Update StockBill set OrderId = @OrderId,TransTypeId = @TransTypeId,Remark = @Remark,Creator = @Creator Where BillId = @BillId";
            }
            tranitems.Add(new Tuple<string, object>(msql, stockBill));
            stockBill.StockBillDetail.ForEach(x =>
            {
                if (x.IsNew == 1)
                {
                    var isql = "Insert StockBillDetail (ID,BillId,FIndex,GoodsId,StockNumber,ColorId,SpecId,Quantity)values(@ID,@BillId,@FIndex,@GoodsId,@StockNumber,@ColorId,@SpecId,@Quantity)";
                    tranitems.Add(new Tuple<string, object>(isql, x));
                }
                else
                {
                    var usql = "Update StockBillDetail set FIndex = @FIndex,GoodsId = @GoodsId,StockNumber = @StockNumber,ColorId = @ColorId,SpecId = @SpecId,Quantity = @Quantity Where ID = @ID";
                    tranitems.Add(new Tuple<string, object>(usql, x));
                }
            });
            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        public async Task<StockBillExt> GetStockBill(string billId)
        {
            var pairs = new Dictionary<string, StockBillExt>();
            var sql = @"Select a.*, 
'' as pid1, b.*, 
'' as pid2, c.*,
'' as pid3, j.*, 
'' as pid4, d.*, 
'' as pid5, e.*, 
'' as pid6, f.*
from StockBill a left join StockBillDetail b
on a.BillId = b.BillId
left join GoodsInfo c
on c.GoodsId = b.GoodsId
left join TransTypeInfo j
on j.TransTypeId = a.TransTypeId
left join ColorInfo d
on d.ColorId = b.ColorId
left join SpecInfo e
on e.SpecId = b.SpecId
left join StockInfo f
on f.StockNumber = b.StockNumber
Where a.BillId = @BillId   order by b.FIndex ";
            var res = await base.QueryAsync<StockBillExt, StockBillDetailExt, GoodsInfo, TransTypeInfo, ColorInfo, SpecInfo, StockInfo>(sql,
                (sb, sbdetail, gi, trans, color, spec, stock) =>
                {
                    if (!pairs.TryGetValue(sb.BillId, out StockBillExt tempbill)) //内联变量声明的写法
                    {
                        tempbill = sb;
                        tempbill.TransType = trans;
                        pairs.Add(billId, tempbill);
                    }
                    var tempitem = tempbill.StockBillDetail.Find(x => x.ID == sbdetail.ID);
                    if (tempitem == null)
                    {
                        tempitem = sbdetail;
                        sbdetail.GoodsInfo = gi;
                        sbdetail.ColorInfo = color;
                        sbdetail.Spec = spec;
                        sbdetail.Stock = stock;
                        tempbill.StockBillDetail.Add(tempitem);
                    }
                    return tempbill;
                },
                new { BillId = billId }, "pid1,pid2,pid3,pid4,pid5,pid6");
            return pairs.Count > 0 ? pairs[billId] : null;
        }

        //查询出入库单据  (以子表行数统计记录数，数据包裹在主表中）
        public async Task<Tuple<IEnumerable<StockBillExt>, int>> SearchStockBill(StockBillParams stockBillParams)
        {
            var pairs = new Dictionary<string, StockBillExt>();
            //条件语句（取记录数和分页共同使用的部分)
            var sqlpub = new StringBuilder(string.Format(@"from StockBill a left join StockBillDetail b on a.BillId = b.BillId 
left join TransTypeInfo c on c.TransTypeId = a.TransTypeId
left join GoodsInfo d on b.GoodsId = d.GoodsId
left join ColorInfo e on e.ColorId = b.ColorId
left join SpecInfo f on f.SpecId = b.SpecId
left join StockInfo g on g.StockNumber = b.StockNumber Where 1 = 1 "));
            if (!string.IsNullOrEmpty(stockBillParams.BillId))
            {
                sqlpub.Append(" and a.BillId = @BillId ");
            }
            if (!string.IsNullOrEmpty(stockBillParams.TransTypeId))
            {
                sqlpub.Append(" and a.TransTypeId = @TransTypeId");
            }
            if (!string.IsNullOrEmpty(stockBillParams.GoodsName))
            {
                sqlpub.AppendFormat(" and d.GoodsName like '%{0}%' ", stockBillParams.GoodsName);
            }
            //排序语句
            string orderSql = string.Empty;
            if (!string.IsNullOrEmpty(stockBillParams.PageModel.OrderField))
            {
                orderSql = string.Format(" order by {0} {1} ", stockBillParams.PageModel.OrderField, stockBillParams.PageModel.OrderWay);
            }
            //取记录数的语句
            var sql1 = "Select count(a.BillId) as TotalCount " + sqlpub;
            //分页获取记录的语句(mysql有limit功能，不用借助row_number都可分页)
            var sql2 = new StringBuilder(@"Select a.*,  
'' as pid1, b.id, b.*,
'' as pid2, c.*, 
'' as pid3, d.*,
'' as pid4, e.*,
'' as pid5, f.*,
'' as pid6, g.*
");
            sql2.Append(sqlpub);
            sql2.Append(orderSql);
            sql2.Append(" limit @Skip,@PageSize");
            //完整sql
            var sql = sql1 + ";" + sql2;
            //执行查询
            var res = await base.QueryPagination<StockBillExt, StockBillDetailExt, TransTypeInfo, GoodsInfo, ColorInfo, SpecInfo, StockInfo>(sql, (sb, sbd, ti, gi, ci, si, sk) =>
            {
                if (!pairs.TryGetValue(sb.BillId, out StockBillExt tempBill))
                {
                    tempBill = sb;
                    tempBill.TransType = ti;
                    pairs.Add(sb.BillId, sb);
                }
                var tempItem = sb.StockBillDetail.Find(x => x.ID == sbd.ID);
                if (tempItem == null)
                {
                    tempItem = sbd;
                    sbd.GoodsInfo = gi;
                    sbd.Spec = si;
                    sbd.ColorInfo = ci;
                    sbd.Stock = sk;
                    tempBill.StockBillDetail.Add(tempItem);
                }
                return sb;
            }, new
            {
                stockBillParams.BillId,
                stockBillParams.TransTypeId,
                stockBillParams.PageModel.Skip,
                stockBillParams.PageModel.PageSize
            }, "pid1,pid2,pid3,pid4,pid5,pid6");

            return new Tuple<IEnumerable<StockBillExt>, int>(pairs.Values.ToList().AsEnumerable(), res.Item2);
        }

        public async Task<bool> DeleteStockBillRow(string id)
        {
            var res = await base.Execute("Delete from StockBillDetail Where ID = @ID", new { ID = id }) > 0;
            return res;
        }

        public async Task<OperResult> DeleteStockBill(string billId)
        {
            var items = new List<Tuple<string, object>>();
            items.Add(new Tuple<string, object>("Delete from StockBillDetail Where BillId = @BillId", new { BillId = billId }));
            items.Add(new Tuple<string, object>("Delete from StockBill Where BillId = @BillId", new { BillId = billId }));
            var res = await base.ExecuteTransaction(items);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };

        }

        public async Task<OperResult> AuditStockBill(string billId, int isAdit)
        {
            var paris = new Dictionary<string, object>();
            paris.Add("_billId", billId);
            paris.Add("isAudit", isAdit);
            var res = await base.Execute<string>("AuditStockBill", paris);
            return new OperResult { Status = res == "success" };
        }

        //检查订单是否制作过不同交易单号的出库单
        public async Task<int> IsCreateOthBill(string billId, long orderId)
        {
            var sql = "select Count(1) from StockBill where OrderId = @OrderId and BillId != @BillId";
            return await base.ExecuteScalar<int>(sql, new { OrderId = orderId, BillId = billId });
        }

        //检查出库数量是否会导致负结存(保存出库单、审核出库单、反审核入库单时均可用此检查)
        public async Task<OperResult> CheckNegativeBalance(StockBill stockBill)
        {
            //出入库明细转换成sql
            var strdetail = new StringBuilder();
            foreach(var item in stockBill.StockBillDetail)
            {
                strdetail.AppendFormat(" {0} Select '{1}' as StockNumber, '{1}' as ColorId, '{2}' as SpecId, {3} as Quantity   ",
                    strdetail.Length > 0 ? " union " : string.Empty, item.StockNumber, item.ColorId, item.SpecId, item.Quantity);
            }
            //按仓库及颜色规格分组统计数量
            var strgroup = $" Select StockNumber, ColorId, SpecId, sum(Quantity) as Quantity from ({strdetail})g ";
            //关联库存明细比对库存
            var sql = string.Format(@" Select count(1) as RecordCount from ({0})t1 left join InventoryDetail t2 on t1.StockNumber = t2.StockNumber
and t1.ColorId = t2.ColorId and t1.SpecId = t2.SpecId and t1.Quantity > IFNull(t1.Quantity,0) ", strgroup);
            var res = await base.ExecuteScalar<int>(sql, new { });
            return new OperResult { Status = res == 0, ErrorMsg = res > 0 ? "此操作将导致负结存" : string.Empty };

        }
    }


}
