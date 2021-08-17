using AllWork.IRepository.Order;
using AllWork.IServices.Order;
using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.Services.Order
{
    public class OrderEvaluateServices:Base.BaseServices<OrderEvaluate>,IOrderEvaluateServices
    {
        readonly IOrderEvaluateRepository _dal;

        public OrderEvaluateServices(IOrderEvaluateRepository orderEvaluateRepository)
        {
            _dal = orderEvaluateRepository;
        }

        //提交订单行的评论
        public async Task<OperResult> SubmitOrderEvaluate(OrderEvaluate orderEvaluate)
        {
            var res = await _dal.SubmitOrderEvaluate(orderEvaluate);
            return res;
        }

        //店铺回复
        public async Task<int> ShopReply(string id, string reply)
        {
            var res = await _dal.ShopReply(id, reply);
            return res;
        }

        //隐藏评论
        public async Task<int> HideEvaluate(string id, int isHide)
        {
            var res = await _dal.HideEvaluate(id, isHide);
            return res;
        }

        //分页获取商品评论
        public async Task<Tuple<IEnumerable<OrderEvaluateExt>, int>> QueryGoodsEvaluates(GoodsEvaluatePraams goodsEvaluate)
        {
            var res = await _dal.QueryGoodsEvaluates(goodsEvaluate);
            return res;
        }

        //获取订单评价
        public async Task<IEnumerable<OrderEvaluate>> GetOrderEvaluates(long orderId)
        {
            var res = await _dal.GetOrderEvaluates(orderId);
            return res;
        }
    }
}
