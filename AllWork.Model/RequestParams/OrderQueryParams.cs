using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 订单查询参数
    /// </summary>
    public class OrderQueryParams
    {
        /// <summary>
        /// 查询方案(取值：0搜索订单 1全部订单 2待付款订单 3待收货订单 4待评价订单 5可售后订单
        /// </summary>
        [Range(0,5)]
        public int QueryScheme { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public string UnionId { get; set; }

        /// <summary>
        /// 查询值(搜索时提供关键字）
        /// </summary>
        public string QueryValue { get; set; }

        public PageModel PageModel { get; set; }
    }
}
