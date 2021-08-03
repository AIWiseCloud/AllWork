using AllWork.IRepository.Order;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.Order;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Repository.Order
{
    public class OrderRepository : Base.BaseRepository<OrderMain>, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {

        }

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
            var sqlmain = @"Insert OrderMain (OrderId,UnionId,DistributionMethod,Receiver,PhoneNumber,DeliveryAddress,Amount,Freight,Discount,RealPay,Platform,Words)
values
(@OrderId,@UnionId,@DistributionMethod,@Receiver,@PhoneNumber,@DeliveryAddress,@Amount,@Freight,@Discount,@RealPay,@Platform,@Words)";
            //sql(插入子表)
            var sqllist = @"Insert OrderList (OrderId,LineId,GoodsId,ColorId,SpecId,Quantity,UnitPrice,Unit,Amount,Evaluate)values(@OrderId,@LineId,@GoodsId,@ColorId,@SpecId,@Quantity,@UnitPrice,@Unit,@Amount,@Evaluate)";
            //要提交的集合
            tranitems.Add(new Tuple<string, object>(sqlmain, orderMain));
            foreach (var item in orderMain.OrderList)
            {
                tranitems.Add(new Tuple<string, object>(sqllist, item));
            }
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
from OrderMain a
left join OrderList b on a.OrderId = b.OrderId
left join GoodsInfo g on g.GoodsId = b.GoodsId
left join GoodsColor c on b.GoodsId = c.GoodsId and b.ColorId = c.ColorId
left join GoodsSpec d on b.GoodsId = d.GoodsId and b.SpecId = d.SpecId
Where a.OrderId = {0}", orderId);
            var res = await base.QueryAsync<OrderMainExt, OrderListExt, GoodsInfo, GoodsColor, GoodsSpec>(sql, (om,ol,gi,gc,gs) => {
                OrderMainExt tempOrder ;
                pairs.TryGetValue(orderId, out tempOrder);
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
                    tempOrder.OrderList.Add(tempol);
                }
                return om;
            }, null, "id1,id2,id3,id4");
            return null;
        }

        public async Task<OperResult> DeleteOrder(long orderId)
        {
            var tranitems = new List<Tuple<string, object>>();
            tranitems.Add(new Tuple<string, object>("Delete from OrderList Where OrderId = @OrderId ", new { OrderId = orderId }));
            tranitems.Add(new Tuple<string, object>("Delete from OrderMain Where OrderId = @OrderId ", new { OrderId = orderId }));
            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        public async Task<int> GetOrderStatusId(long orderId)
        {
            var res = await base.ExecuteScalar<int>("Select StatusId from OrderMain Where OrderId = @OrderId", new { OrderId = orderId });
            return res;
        }

    }
}
