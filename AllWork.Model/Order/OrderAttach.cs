using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Order
{
    /// <summary>
    /// 订单附件
    /// </summary>
    public class OrderAttach
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public long OrderId
        { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 支付凭证
        /// </summary>
        [Required]
        public string PayVoucherUrl
        { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? CreateDate
        { get; set; }
    }
}
