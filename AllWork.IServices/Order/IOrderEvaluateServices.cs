using AllWork.Model;
using AllWork.Model.Order;
using AllWork.Model.RequestParams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllWork.IServices.Order
{
    public interface IOrderEvaluateServices:Base.IBaseServices<OrderEvaluate>
    {
        //提交订单行的评价
        Task<OperResult> SubmitOrderEvaluate(OrderEvaluate orderEvaluate);

        //店铺回复
        Task<int> ShopReply(string id, string reply);

        //隐藏评论
        Task<int> HideEvaluate(string id, int isHide);

        //分页获取商品评价
        Task<Tuple<IEnumerable<OrderEvaluateExt>, int>> QueryGoodsEvaluates(GoodsEvaluatePraams goodsEvaluate);

        //获取订单评价
        Task<IEnumerable<OrderEvaluate>> GetOrderEvaluates(long orderId);
    }
}
