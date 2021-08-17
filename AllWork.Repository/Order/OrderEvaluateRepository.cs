using AllWork.IRepository.Order;
using AllWork.Model;
using AllWork.Model.Goods;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using AllWork.Model.Sys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AllWork.Repository.Order
{
    public class OrderEvaluateRepository : Base.BaseRepository<OrderEvaluate>, IOrderEvaluateRepository
    {
        public OrderEvaluateRepository(IConfiguration configuration) : base(configuration) { }

        //提交订单行的评价
        public async Task<OperResult> SubmitOrderEvaluate(OrderEvaluate orderEvaluate)
        {
            var instance = await base.QueryFirst("Select * from OrderEvaluate Where ID = @ID", orderEvaluate);
            string sql;
            if (instance == null)
            {
                sql = @"Insert OrderEvaluate (ID,OrderId,LineId,GoodsId,Content,UnionId,GoodsScore,ServiceScore,TimeScore,Images)
values
(@ID,@OrderId,@LineId,@GoodsId,@Content,@UnionId,@GoodsScore,@ServiceScore,@TimeScore,@Images)";
            }
            else
            {
                sql = @"Update OrderEvaluate set Content = @Content,UnionId = @UnionId,
GoodsScore = @GoodsScore,ServiceScore = @ServiceScore,TimeScore = @TimeScore,
Images = @Images Where ID = @ID";
            }
            //更新订单已评价标记
            string sql2 = "update OrderList set Evaluate = 1 where OrderId = @OrderId and LineId = @LineId";
            var tranitems = new List<Tuple<string, object>>
            {
                new Tuple<string, object>(sql,orderEvaluate),
                new Tuple<string, object>(sql2,new{orderEvaluate.OrderId,orderEvaluate.LineId})
            };


            var res = await base.ExecuteTransaction(tranitems);
            return new OperResult { Status = res.Item1, ErrorMsg = res.Item2 };
        }

        //店铺回复
        public async Task<int> ShopReply(string id, string reply)
        {
            var sql = "Update OrderEvaluate set ShopReply = @ShopReply, ReplyTime = current_timestamp() Where ID = @ID";
            var res = await base.Execute(sql, new { ID = id, ShopReply = reply });
            return res;
        }

        //隐藏评价
        public async Task<int> HideEvaluate(string id, int isHide)
        {
            var sql = "Update OrderEvaluate set IsHide = @IsHide Where ID = @ID";
            var res = await base.Execute(sql, new { ID = id, IsHide = isHide });
            return res;
        }

        //分页获取商品评价
        public async Task<Tuple<IEnumerable<OrderEvaluateExt>, int>> QueryGoodsEvaluates(GoodsEvaluatePraams goodsEvaluatePraams)
        {
            //(1) sql语句公共部分
            var sqlpub = new StringBuilder(@"Select {0}  from OrderEvaluate a left join UserInfo b on a.UnionId = b.UnionId 
left join OrderList c on c.OrderId = a.OrderId and c.LineId = a.LineId 
left join GoodsColorSpec d on d.GoodsId = c.GoodsId and d.ColorId = c.ColorId and d.SpecId = c.SpecId Where a.GoodsId = @GoodsId ");
            //(2) 排序sql
            string sqlorder = string.Empty;
            if (!string.IsNullOrEmpty(goodsEvaluatePraams.PageModel.OrderField))
            {
                sqlorder = string.Format(" order by {0} {1} ", goodsEvaluatePraams.PageModel.OrderField, goodsEvaluatePraams.PageModel.OrderWay);
            }
            //(3) sql语句1（求总记录数）
            var sql1 = string.Format(sqlpub.ToString(), "count(a.ID) as TotalCount");
            //sql语句2（求分页数据）
            var sql2 = string.Format(sqlpub.ToString(), " a.*, '' as id1, b.NickName ,b.Avatar ,'' as id2, d.* ") + sqlorder + " limit @Skip, @PageSize ";

            //完整sql
            var sql = sql1 + ";" + sql2;
            var res = await base.QueryPagination<OrderEvaluateExt, UserInfo, GoodsColorSpec>(sql, (oe, ui, gcs) =>
            {
                oe.UserInfo = ui;
                oe.GoodsColorSpec = gcs;
                return oe;
            }, new
            {
                GoodsId = goodsEvaluatePraams.GoodsId,
                goodsEvaluatePraams.PageModel.Skip,
                goodsEvaluatePraams.PageModel.PageSize,
            }, "id1, id2");
            return res;
        }

        //获取订单评价
        public async Task<IEnumerable<OrderEvaluate>> GetOrderEvaluates(long orderId)
        {
            var sql = "Select * from OrderEvaluate Where OrderId = @OrderId";
            var res = await base.QueryList(sql, new { OrderId = orderId });
            return res;
        }
    }
}