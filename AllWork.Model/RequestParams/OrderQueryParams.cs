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
        /// 查询关键字(QueryScheme = 0时用; 支持订单号、商品ID查询和产品编号、商品名称搜索)
        /// </summary>
        public string QueryValue { get; set; }

        /// <summary>
        /// 订单状态(QueryScheme = 0时用; 9表示所有状态) 
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// 开始日期(QueryScheme = 0时用)
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 截止日期(QueryScheme = 0时用)
        /// </summary>
        public string EndDate { get; set; }

        public PageModel PageModel { get; set; }
    }
}
