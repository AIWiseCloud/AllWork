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
        public async Task<OperResult> SaveStockBill(StockBill stockBill)
        {
            var instance = await base.QueryFirst("Select * from StockBill Where BillId = @BillId", new { stockBill.BillId });
            List<Tuple<string, object>> tranitems = new List<Tuple<string, object>>();
            string msql = null;
            //保存主表
            if (instance == null)
            {
                msql = "Insert StockBill (BillId,TransTypeId,Remark,OrderId, Creator)values(@BillId,@TransTypeId,@Remark,@OrderId, @Creator)";
            }
            else
            {
                msql = "Update StockBill set OrderId = @OrderId,TransTypeId = @TransTypeId,Remark = @Remark,Creator = @Creator Where BillId = @BillId";
            }
            tranitems.Add(new Tuple<string, object>(msql, stockBill));
            //保存子表
            tranitems.Add(new Tuple<string ,object>("Delete from StockBillDetail Where BillId = @BillId", new { stockBill.BillId })); //先删除子表
            stockBill.StockBillDetail.ForEach(x =>
            {
                var isql = "Insert StockBillDetail (ID,BillId,FIndex,GoodsId,StockNumber,ColorId,SpecId,Quantity)values(@ID,@BillId,@FIndex,@GoodsId,@StockNumber,@ColorId,@SpecId,@Quantity)";
                tranitems.Add(new Tuple<string, object>(isql, x));
                //if (x.IsNew == 1)
                //{
                //    var isql = "Insert StockBillDetail (ID,BillId,FIndex,GoodsId,StockNumber,ColorId,SpecId,Quantity)values(@ID,@BillId,@FIndex,@GoodsId,@StockNumber,@ColorId,@SpecId,@Quantity)";
                //    tranitems.Add(new Tuple<string, object>(isql, x));
                //}
                //else
                //{
                //    var usql = "Update StockBillDetail set FIndex = @FIndex,GoodsId = @GoodsId,StockNumber = @StockNumber,ColorId = @ColorId,SpecId = @SpecId,Quantity = @Quantity Where ID = @ID";
                //    tranitems.Add(new Tuple<string, object>(usql, x));
                //}
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
left join GoodsColor d
on d.ID = b.ColorId
left join GoodsSpec e
on e.ID = b.SpecId
left join StockInfo f
on f.StockNumber = b.StockNumber
Where a.BillId = @BillId   order by b.FIndex ";
            var res = await base.QueryAsync<StockBillExt, StockBillDetailExt, GoodsInfo, TransTypeInfo, GoodsColor, GoodsSpec, StockInfo>(sql,
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
                        sbdetail.GoodsColor = color;
                        sbdetail.GoodsSpec = spec;
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
left join GoodsColor e on e.ID = b.ColorId
left join GoodsSpec f on f.ID = b.SpecId
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
            var res = await base.QueryPagination<StockBillExt, StockBillDetailExt, TransTypeInfo, GoodsInfo, GoodsColor, GoodsSpec, StockInfo>(sql, (sb, sbd, ti, gi, ci, si, sk) =>
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
                    sbd.GoodsSpec = si;
                    sbd.GoodsColor = ci;
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
            var items = new List<Tuple<string, object>>
            {
                new Tuple<string, object>("Delete from StockBillDetail Where BillId = @BillId", new { BillId = billId }),
                new Tuple<string, object>("Delete from StockBill Where BillId = @BillId", new { BillId = billId })
            };
            var res = await base.ExecuteTransaction(items);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };

        }

        public async Task<OperResult> AuditStockBill(string billId, int isAdit)
        {
            var paris = new Dictionary<string, object>
            {
                { "_billId", billId},
                {"isAudit", isAdit }
            };
            var res = await base.Execute<string>("AuditStockBill", paris);
            return new OperResult { Status = res == "success" };
        }

        //检查订单是否制作过不同交易单号的出库单
        public async Task<int> IsCreateOthBill(string billId, long? orderId)
        {
            var sql = "select Count(1) from StockBill where OrderId = @OrderId and BillId != @BillId";
            return await base.ExecuteScalar<int>(sql, new { OrderId = orderId, BillId = billId });
        }

        //检查出库数量是否会导致负结存(保存出库单、审核出库单、反审核入库单时均可用此检查)
        public async Task<OperResult> CheckNegativeBalance(StockBillExt stockBill)
        {
            //出入库明细转换成sql
            var strdetail = new StringBuilder();
            foreach (var item in stockBill.StockBillDetail)
            {
                strdetail.AppendFormat(" {0} Select '{1}' as StockNumber, '{2}' as ColorId, '{3}' as SpecId, {4} as Quantity ,'{5}' as GoodsId  ",
                    strdetail.Length > 0 ? " union " : string.Empty, item.StockNumber, item.ColorId, item.SpecId, item.Quantity, item.GoodsId);
            }
            //按仓库及颜色规格分组统计数量
            var strgroup = $" Select StockNumber, GoodsId, ColorId, SpecId, sum(Quantity) as Quantity from ({strdetail})g ";
            //关联库存明细比对库存
            var sql = string.Format(@" Select count(1) as RecordCount from ({0})t1 left join InventoryDetailView t2 on t1.StockNumber = t2.StockNumber 
 and t1.GoodsId = t2.GoodsId and t1.ColorId = t2.ColorId and t1.SpecId = t2.SpecId Where t1.Quantity > IFNull(t2.Quantity,0) ", strgroup);

            var res = await base.ExecuteScalar<int>(sql, new { });

            return new OperResult { Status = res == 0, ErrorMsg = res > 0 ? "此操作将导致负结存" : string.Empty };
        }


        //获取商品实际库存信息
        public async Task<decimal> GetInventoryDetail(string goodsId, string colorId, string specId, string stockNumber)
        {
            var sql = "Select Quantity from InventoryDetailView Where GoodsId = @GoodsId and ColorId = @ColorId and SpecId = @SpecId and StockNumber = @StockNumber";
            var res = await base.ExecuteScalar<decimal>(sql, new { GoodsId = goodsId, ColorId = colorId, SpecId = specId, StockNumber = stockNumber });
            return res;
        }

        //获取待发货订单列表
        public async Task<dynamic> GetToBeShipped(string orderId = "")
        {
            var str = @"Select a.OrderId,a.OrderTime ,a.DeliveryAddress ,a.Receiver
 from OrderMain a left join StockBill b on a.OrderId = b.OrderId where a.StatusId = 1 and isnull(b.BillId) ";
            ; var sql = new StringBuilder();
            if (!string.IsNullOrEmpty(orderId))
            {
                sql.AppendFormat(" Select * from ({0})t Where OrderId = {1} ", str, orderId);
            }
            else
            {
                sql.Append(str);
            }
            sql.AppendFormat(" order by OrderTime");
            var res = await base.QueryList<dynamic>(sql.ToString());
            return res;
        }
    }


}
