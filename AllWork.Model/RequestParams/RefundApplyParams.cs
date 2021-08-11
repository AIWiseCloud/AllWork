using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 退款申请请求参数
    /// </summary>
    public class RefundApplyParams
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        [Required(ErrorMessage ="商户订单号不能为空")]
        public string OrderId { get; set; }

        /// <summary>
        /// 退款单号：商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。
        /// </summary>
        [Required(ErrorMessage = "退款单号不能为空")]
        public string RefundId { get; set; }

        /// <summary>
        /// 订单总金额，单位为分，只能为整数
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 退款总金额，订单总金额，单位为分，只能为整数
        /// </summary>
        public int RefundFee { get; set; }
    }
}
