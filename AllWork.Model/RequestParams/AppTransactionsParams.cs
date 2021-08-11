using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AllWork.Model.RequestParams
{
    public class AppTransactionsParams
    {
        /// <summary>
        /// 商城订单号
        /// </summary>
        [Required]
        public long OrderId { get; set; }

        /// <summary>
        /// 商品描述(127个字符以内)
        /// </summary>
        [Required(ErrorMessage ="商品名称不能为空")]
        public String GoodsName { get; set; }

        /// <summary>
        /// 订单金额(单位：分)
        /// </summary>
        [Required][Range(1,1000000000)]
        public int OrderAmount { get; set; }
    }
}
