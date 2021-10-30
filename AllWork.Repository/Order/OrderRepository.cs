using AllWork.IRepository.Order;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Order
{
    public class OrderRepository : Base.BaseRepository<OrderMain>, IOrderRepository
    {
        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="orderMain"></param>
        /// <returns></returns>
        public async Task<OperResult> GenerateOrder(OrderMain orderMain)
        {
            //处理订单号
            var orderId = AllWork.Common.Utils.CreateUniqueId();
            orderMain.OrderId = orderId;
            for (var i = 0; i < orderMain.OrderList.Count; i++)
            {
                orderMain.OrderList[i].OrderId = orderId;
                orderMain.OrderList[i].LineId = i + 1;
            }
            List<Tuple<string, object>> tranitems = new List<Tuple<string, object>>();
            //sql(插入主表)
            var sqlmain = @"Insert OrderMain (OrderId,UnionId,DistributionMethod,Receiver,PhoneNumber,DeliveryAddress,Amount,Freight,Discount,RealPay,PaymentWay,Platform,Words,InvoiceType)
values
(@OrderId,@UnionId,@DistributionMethod,@Receiver,@PhoneNumber,@DeliveryAddress,@Amount,@Freight,@Discount,@RealPay,@PaymentWay,@Platform,@Words, @InvoiceType)";
            //sql(插入子表)
            var sqllist = @"Insert OrderList (OrderId,LineId,GoodsId,ColorId,SpecId,Quantity,UnitPrice,Packaging,Amount,Evaluate)values(@OrderId,@LineId,@GoodsId,@ColorId,@SpecId,@Quantity,@UnitPrice,@Packaging,@Amount,@Evaluate)";
            //要提交的集合
            tranitems.Add(new Tuple<string, object>(sqlmain, orderMain));
            foreach (var item in orderMain.OrderList)
            {
                tranitems.Add(new Tuple<string, object>(sqllist, item));
            }
            //加上扣减可用库存的语句
            //   tranitems.Add(new Tuple<string, object>($"CALL OrderAffectStock({orderMain.OrderId}, 1)", new { }));

            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2, IdentityKey = orderId.ToString() };
        }

        //获取订单详情
        public async Task<OrderMainExt> GetOrderInfo(long orderId)
        {
            var pairs = new Dictionary<long, OrderMainExt>();
            var sql = string.Format(@"Select a.*
,'' as id1, b.*
,'' as id2, g.*
,'' as id3, c.*
,'' as id4, d.*
,'' as id5, e.*
from OrderMain a
left join OrderList b on a.OrderId = b.OrderId
left join GoodsInfo g on g.GoodsId = b.GoodsId
left join GoodsColor c on b.GoodsId = c.GoodsId and b.ColorId = c.ID
left join GoodsSpec d on b.GoodsId = d.GoodsId and b.SpecId = d.ID
left join GoodsColorSpec e on b.GoodsId = e.GoodsId and b.ColorId = e.ColorId and b.SpecId = e.SpecId
Where a.OrderId = {0}", orderId);
            var res = await base.QueryAsync<OrderMainExt, OrderListExt, GoodsInfo, GoodsColor, GoodsSpec, GoodsColorSpec>(sql, (om, ol, gi, gc, gs, cs) =>
            {
                pairs.TryGetValue(orderId, out OrderMainExt tempOrder);
                if (tempOrder == null)
                {
                    tempOrder = om;
                    pairs.Add(orderId, om);
                }
                var tempol = tempOrder.OrderList.Find(x => x.LineId == ol.LineId);
                if (tempol == null)
                {
                    tempol = ol;
                    ol.GoodsInfo = gi;
                    ol.GoodsColor = gc;
                    ol.GoodsSpec = gs;
                    ol.GoodsColorSpec = cs;
                    tempOrder.OrderList.Add(tempol);
                }
                return om;
            }, null, "id1,id2,id3,id4,id5");
            return pairs.Values.Count > 0 ? pairs[orderId] : null;
        }

        //删除订单
        public async Task<OperResult> DeleteOrder(long orderId)
        {
            var tranitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>($"CALL OrderAffectStock({orderId}, 0)", new { }),
                new Tuple<string, object>("Delete from OrderList Where OrderId = @OrderId ", new { OrderId = orderId }),
                new Tuple<string, object>("Delete from OrderMain Where OrderId = @OrderId ", new { OrderId = orderId })
            };
            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        //取消订单
        public async Task<OperResult> CancelOrder(long orderId)
        {
            var pairs = new Dictionary<string, object>
            {
                { "_orderId", orderId},
                { "_isGenerate", 0}
            };

            var res = await base.Execute<string>("OrderAffectStock", pairs);
            return new OperResult { Status = res == "success" };
        }

        //订单发货
        public async Task<OperResult> DeliveryOrder(OrderDeliveryParams orderDeliveryParams)
        {
            var sql = new StringBuilder();
            if (orderDeliveryParams.IsDelivery == 1)
            {
                sql.Append(" Update OrderMain set StatusId = 2, LogisticsId = @LogisticsId, ExpressId = @ExpressId, DeliveryTime = current_timestamp() Where OrderId = @OrderId  ");
            }
            else
            {
                sql.Append(" Update OrderMain set StatusId = 1, LogisticsId = null, ExpressId = null, DeliveryTime = null Where OrderId = @OrderId ");
            }

            var tranItems = new List<Tuple<string, object>>
            {
                new Tuple<string,object>(sql.ToString(),orderDeliveryParams),
                new Tuple<string,object>(string.Format("  CALL AuditStockBill('{0}',{1}) ", orderDeliveryParams.BillId, orderDeliveryParams.IsDelivery),new{})
            };
            var res = await base.ExecuteTransaction(tranItems);

            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        //订单到款确认
        public async Task<int> ConfirmPay(long orderId, int isConfirm)
        {
            string sql;
            if (isConfirm == 1)
            {
                sql = "update OrderMain set StatusId = 1, PayTime = current_timestamp() where OrderId = @OrderId and StatusId = 0";
            }
            else
            {
                sql = "update OrderMain set StatusId = 0, PayTime = null where OrderId = @OrderId and StatusId = 1";
            }
            var res = await base.Execute(sql, new { OrderId = orderId });
            return res;
        }

        //签收订单
        public async Task<int> SignatureOrder(long orderId)
        {
            var sql = "update OrderMain set StatusId = 3, SigningTime = current_timestamp() where OrderId = @OrderId and StatusId = 2";
            var res = await base.Execute(sql, new { OrderId = orderId });
            return res;
        }

        public async Task<int> GetOrderStatusId(long orderId)
        {
            var res = await base.ExecuteScalar<int>("Select StatusId from OrderMain Where OrderId = @OrderId", new { OrderId = orderId });
            return res;
        }

        //支付成功后更新订单、更新销量  (在支付结果通知中调用)
        public async Task<bool> PaySuccess(long orderId, string tradeNo)
        {
            string id = orderId.ToString();
            //6开头的单号代表定金支付单号
            if (id.StartsWith('6'))
            {
                var sql1 = "update AdvanceMoney set ConfirmStatus = 1, Confirmer = 'system', PayTime = current_timestamp, TradeNo = @TradeNo where ID = @ID ";
                var sql2 = "update OrderMain a, AdvanceMoney b set a.DownPayment = a.DownPayment + b.DownPayment where a.OrderId = b.OrderId and b.ID = @ID and StatusID = 0";

                var tranitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql1,new{ID=id, TradeNo=tradeNo }),
                new Tuple<string, object>(sql2,new{ID=id })
            };
                var res = await base.ExecuteTransaction(tranitems);
                return res.Item1;
            }
            else //订单支付成功
            {
                var sql = @"update OrderMain set 
TradeNo = @TradeNo ,
PaymentChannel = 'wechat' , 
StatusId = 1, 
PayTime = current_timestamp() 
where OrderId = @OrderId  and StatusId = 0 ";
                var sql2 = @"update GoodsInfo a, OrderList b set 
SalesTimes = SalesTimes + b.Quantity 
where a.GoodsId = b.GoodsId and b.OrderId =@OrderId";
                var tranItems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql,new { OrderId = orderId, TradeNo = tradeNo }),
                new Tuple<string, object>(sql2,new { OrderId = orderId })
            };
                var res = await base.ExecuteTransaction(tranItems);
                return res.Item1;
            }
        }

        //查询订单
        public async Task<Tuple<IEnumerable<OrderMainExt>, int>> QueryOrders(OrderQueryParams orderQueryParams)
        {
            var pairs = new Dictionary<long, OrderMainExt>();
            //(1) 先写出带条件的多表查询语句，只取订单号一列并去重
            var sqlpub = new StringBuilder();
            sqlpub.Append(@" Select distinct a.OrderId
from OrderMain a left join OrderList b on a.OrderId = b.OrderId
left join GoodsInfo c on b.GoodsId = c.GoodsId
left join GoodsColor d on b.GoodsId = d.GoodsId and b.ColorId = d.ID
left join GoodsSpec e on b.GoodsId = e.GoodsId and b.SpecId = e.ID
left join GoodsColorSpec f on b.GoodsId = f.GoodsId and b.ColorId = f.ColorId and b.SpecId = f.SpecId Where (1 = 1)");
            if (orderQueryParams.UnionId != "*")
            {
                
                var unionids = @"Select UnionId from UserInfo where OpenUserId 
in(
select b.OpenUserId from UserInfo a, Salesman b where a.PhoneNumber = b.Mobile and a.UnionId=@UnionId
)
union select  @UnionId ";
                //sqlpub.Append(" and UnionId = @UnionId ");//*表示后端查询不用按用户ID过滤
                sqlpub.AppendFormat(" and UnionId in ({0}) ", unionids); //移动端可以查看自己及自己客户下的订单(2021-10-27)
            }
            //查询方案为0：按关键字搜索
            if (orderQueryParams.QueryScheme == 0)
            {
                if (!string.IsNullOrEmpty(orderQueryParams.QueryValue))
                {
                    sqlpub.AppendFormat(" and (a.OrderId = @OrderId or b.GoodsId = @GoodsId or c.ProdNumber like @ProdNumber or c.GoodsName like '%{0}%')  ", orderQueryParams.QueryValue);
                }
                if (orderQueryParams.StatusId != 9)
                {
                    sqlpub.Append(" and a.StatusId = @StatusId "); //默认9表示所有状态
                }
                if (!string.IsNullOrEmpty(orderQueryParams.StartDate) && !string.IsNullOrEmpty(orderQueryParams.EndDate))
                {
                    sqlpub.Append(" and  a.OrderTime between @StartDate and @EndDate ");
                }
            }
            //查询方案为：待付款订单
            if (orderQueryParams.QueryScheme == 2)
            {
                sqlpub.Append(" and a.StatusId = 0");
            }
            //查询方案为：待收货订单
            if (orderQueryParams.QueryScheme == 3)
            {
                sqlpub.Append(" and a.StatusId in (1, 2) ");
            }
            //查询方案为：待评价订单
            if (orderQueryParams.QueryScheme == 4)
            {
                sqlpub.Append(" and a.StatusId = 3 and b.Evaluate = 0 ");
            }
            //查询方案为：可售后订单
            if (orderQueryParams.QueryScheme == 5)
            {
                sqlpub.Append(" and a.StatusId = 3 ");//这里还要补充条件
            }
            //(2) sql排序
            string sqlorder = " Order by a.OrderId desc ";
            if (!string.IsNullOrEmpty(orderQueryParams.PageModel.OrderField))
            {
                sqlorder = string.Format(" order by {0} {1} ", orderQueryParams.PageModel.OrderField, orderQueryParams.PageModel.OrderWay);
            }
            //(3) sql语句1（求总记录数）
            var sql1 = "Select Count(OrderId) as TotalCount from (" + sqlpub + " )t "; //记录数
            //sql语句2: 重写订单主子表及关联表语句并与上面加上订单号条件，订单号取上面加了limit的去重语句
            var sql2 = string.Format(@"Select a.*,  '' as id1, b.*,'' as id2, c.*,'' as id3, d.*,'' as id4, e.*,'' as id5, f.*, '' as id6, oa.*
from 
(Select t1.*,t2.Salesman from OrderMain t1 left join UserInfo t2 on t1.UnionId = t2.UnionId) a ,
(
{0} {1} limit @Skip, @PageSize
)idtab
left join OrderList b on idtab.OrderId = b.OrderId
left join OrderAttach oa on oa.OrderId = idtab.OrderId
left join GoodsInfo c on b.GoodsId = c.GoodsId
left join GoodsColor d on b.GoodsId = d.GoodsId and b.ColorId = d.ID
left join GoodsSpec e on b.GoodsId = e.GoodsId and b.SpecId = e.ID
left join GoodsColorSpec f on b.GoodsId = f.GoodsId and b.ColorId = f.ColorId and b.SpecId = f.SpecId 
Where a.OrderId = idtab.OrderId", sqlpub, sqlorder);
            //完整sql
            var sql = sql1 + ";" + sql2;
            var res = await base.QueryPagination<OrderMainExt, OrderListExt, GoodsInfo, GoodsColor, GoodsSpec, GoodsColorSpec, OrderAttach>(sql, (om, ol, gi, gc, gs, cs, oa) =>
             {
                 pairs.TryGetValue(om.OrderId, out OrderMainExt tempOrder);
                 if (tempOrder == null)
                 {
                     tempOrder = om;
                     om.OrderAttach = oa;
                     pairs.Add(om.OrderId, tempOrder);
                 }
                 var tempitem = tempOrder.OrderList.Find(x => x.OrderId == ol.OrderId && x.LineId == ol.LineId);
                 if (tempitem == null)
                 {
                     tempitem = ol;
                     ol.GoodsInfo = gi;
                     ol.GoodsColor = gc;
                     ol.GoodsSpec = gs;
                     ol.GoodsColorSpec = cs;
                     tempOrder.OrderList.Add(tempitem);
                 }
                 return om;
             }, new
             {
                 orderQueryParams.UnionId,
                 OrderId = orderQueryParams.QueryValue,
                 CategoryId = orderQueryParams.QueryValue,
                 GoodsId = orderQueryParams.QueryValue,
                 orderQueryParams.StartDate,
                 orderQueryParams.EndDate,
                 orderQueryParams.StatusId,
                 ProdNumber = orderQueryParams.QueryValue,
                 orderQueryParams.PageModel.Skip,
                 orderQueryParams.PageModel.PageSize,
             }, "id1, id2,id3,id4,id5, id6");
            return new Tuple<IEnumerable<OrderMainExt>, int>(pairs.Values.ToList().AsEnumerable(), res.Item2);
        }

        //获取我的待办
        public async Task<dynamic> GetMyTodoList(string unionId)
        {
            var res = await base.QueryList<dynamic>($"Call GetMyTodoList('{unionId}')", new { });
            return res;
        }

        //修改订单收货信息
        public async Task<bool> UpdateOrderAddress(UpdateOrderAddressParams updateOrderAddressParams)
        {
            var sql = @"update OrderMain set Receiver = @Receiver,PhoneNumber = @PhoneNumber, DeliveryAddress = @DeliveryAddress  where OrderId = @OrderId and StatusId < 2";
            var res = await base.Execute(sql, updateOrderAddressParams) > 0;
            return res;
        }

        //由订单号获取对应的销售出库单号
        public async Task<string> GetBillId(long orderId)
        {
            var sql = string.Format("Select BillId from StockBill Where OrderId = {0} ", orderId);
            return await base.ExecuteScalar<string>(sql);
        }

        public async Task<int> UploadOrderAttach(OrderAttach orderAttach)
        {
            var instance = await base.QueryFirst<OrderAttach>("Select * from OrderAttach Where OrderId = @OrderId", orderAttach);
            string sql;
            if (instance == null)
            {
                sql = "Insert OrderAttach (OrderId,UnionId,PayVoucherUrl)values(@OrderId,@UnionId,@PayVoucherUrl)";
            }
            else
            {
                sql = "Update OrderAttach set OrderId = @OrderId,UnionId = @UnionId,PayVoucherUrl = @PayVoucherUrl Where OrderId = @OrderId";
            }
            var res = await base.Execute(sql, orderAttach);
            return res;
        }

        //订单调价
        public async Task<Tuple<bool, string>> AdjustOrderPrice(long orderId, int lineId, decimal newPrice)
        {
            var sql1 = "update OrderList set UnitPrice = @UnitPrice, Amount = Quantity * @UnitPrice where OrderId = @OrderId and LineId = @LineId ";
            var sql2 = @"update OrderMain a, (select sum(Amount)as Amount from OrderList where OrderId = @OrderId)b set
a.Amount = b.Amount, a.RealPay = b.Amount - a.Freight - a.Discount 
where a.OrderId = @OrderId";

            var transitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql1,new{UnitPrice = newPrice, OrderId= orderId,LineId=lineId}),
                new Tuple<string, object>(sql2,new{OrderId=orderId})
            };
            var res = await base.ExecuteTransaction(transitems);
            return res;
        }

        //订单调数量
        public async Task<Tuple<bool, string>> AdjustOrderQuantity(long orderId, int lineId, decimal newQty)
        {
            var sql1 = "update OrderList set Quantity = @Quantity, Amount = @Quantity * UnitPrice where OrderId = @OrderId and LineId = @LineId ";
            var sql2 = @"update OrderMain a, (select sum(Amount)as Amount from OrderList where OrderId = @OrderId)b set
a.Amount = b.Amount, a.RealPay = b.Amount - a.Freight - a.Discount 
where a.OrderId = @OrderId";

            var transitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql1,new{Quantity = newQty, OrderId= orderId,LineId=lineId}),
                new Tuple<string, object>(sql2,new{OrderId=orderId})
            };
            var res = await base.ExecuteTransaction(transitems);
            return res;
        }
    }


}
