using AllWork.IRepository.Goods;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Goods
{
    public class StockBillRepository : Base.BaseRepository<StockBill>, IStockBillRepository
    {
        public StockBillRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<OperResult> SaveStockBill(StockBill stockBill)
        {
            var instance = await base.QueryFirst("Select * from StockBill Where BillId = @BillId", new { BillId = stockBill.BillId });
            List<Tuple<string, object>> tranitems = new List<Tuple<string, object>>();
            string msql = null;
            if (instance == null)
            {
                msql = "Insert StockBill (BillId,TransTypeId,Remark,Creator)values(@BillId,@TransTypeId,@Remark,@Creator)";
            }
            else
            {
                msql = "Update StockBill set BillId = @BillId,TransTypeId = @TransTypeId,Remark = @Remark,Creator = @Creator Where BillId = @BillId";
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

        public async Task<StockBill> GetStockBill(string billId)
        {
            var pairs = new Dictionary<string, StockBill>();
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
            var res = await base.QueryAsync<StockBill, StockBillDetail, GoodsInfo, TransTypeInfo, ColorInfo, SpecInfo, StockInfo>(sql,
                (sb, sbdetail, gi, trans, color, spec, stock) =>
                {
                    StockBill tempbill;
                    if (!pairs.TryGetValue(sb.BillId, out tempbill))
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

        public async Task<Tuple<IEnumerable<StockBill>, int>> SearchStockBill(StockBillParams stockBillParams)
        {
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
                orderSql = string.Format(" order by {0} {1} ",stockBillParams.PageModel.OrderField,stockBillParams.PageModel.OrderWay);
            }
            //取记录数的语句
            var sql1 = "Select count(a.BillId) as TotalCount " + sqlpub;
            //分页获取记录的语句(mysql有limit功能，不用借助row_number都可分页)
            var sql2 = new StringBuilder(@"Select a.BillId,a.Remark, a.CreateDate,  
'' as id1, b.id, b.FIndex, b.GoodsId, b.ColorId, b.SpecId, b.Quantity,
'' as id2, c.*, 
'' as id3, d.GoodsName, d.UnitName,
'' as id4, e.*,
'' as id5, f.*,
'' as id6, g.*
");
            sql2.Append(sqlpub);
            sql2.Append(orderSql);
            sql2.Append(" limit @Skip,@PageSize");
            //完整sql
            var sql = sql1 + ";" + sql2;
            //执行查询
            var res = await base.QueryPagination<StockBill, StockBillDetail, TransTypeInfo, GoodsInfo, ColorInfo, SpecInfo, StockInfo>(sql, (sb, sbd, ti, gi, ci, si, sk) =>
            {
                sb.TransType = ti;
                sb.StockBillDetail.Add(sbd);
                sbd.GoodsInfo = gi;
                sbd.ColorInfo = ci;
                sbd.Spec = si;
                sbd.Stock = sk;
                return sb;
            }, new
            {
                BillId = stockBillParams.BillId,
                TransTypeId = stockBillParams.TransTypeId,
                Skip = stockBillParams.PageModel.Skip,
                PageSize = stockBillParams.PageModel.PageSize
            }, "id1,id2,id3,id4,id5,id6");
            return res;
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
    }


}
