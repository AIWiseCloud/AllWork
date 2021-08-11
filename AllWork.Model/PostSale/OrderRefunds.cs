using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.PostSale
{
    public class OrderRefunds
    {
        /// <summary>
        /// 售后服务单号
        /// </summary>
        public string PostSaleId
        { get; set; }


        /// <summary>
        /// 业务类型（1退货,2退款, 3换货
        /// </summary>
        [Range(1,3)]
        public int CurrentType
        { get; set; }

        /// <summary>
        /// 订单支付渠道
        /// </summary>
        [Required(ErrorMessage ="订单支付渠道不能为空")]
        public string PaymentChannel
        { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空")]
        public string OrderId
        { get; set; }

        /// <summary>
        /// 订单行号
        /// </summary>
        public int OrderLineId
        { get; set; }

        /// <summary>
        /// 换货颜色ID
        /// </summary>
        public string ColorId
        { get; set; }

        /// <summary>
        /// 换货规格ID
        /// </summary>
        public string SpecId
        { get; set; }

        /// <summary>
        /// 管理员退款方向
        /// </summary>
        public int RefundTo
        { get; set; }

        /// <summary>
        /// 售后原因ID
        /// </summary>
        [Required(ErrorMessage = "售后原因不能为空")]
        public string RefundResonId
        { get; set; }

        /// <summary>
        /// 售后数量
        /// </summary>
        public int BackQty
        { get; set; }

        /// <summary>
        /// 售后金额
        /// </summary>
        public decimal BackMoney
        { get; set; }

        /// <summary>
        /// 承运商代码
        /// </summary>
        public string LogisticsId
        { get; set; }

        /// <summary>
        /// 用户退货物流单号
        /// </summary>
        public string ExpressId
        { get; set; }

        /// <summary>
        /// 图片凭证路径
        /// </summary>
        public string Images
        { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string RefundUser
        { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string RefundUserPhone
        { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string RefundAddress
        { get; set; }

        /// <summary>
        /// 管理员退款流水号
        /// </summary>
        public string RefundTradeNo
        { get; set; }

        /// <summary>
        /// 管理员备注
        /// </summary>
        public string RefundRemark
        { get; set; }

        /// <summary>
        /// 结案时间（如退款、取消、关闭、完成时间）
        /// </summary>
        public DateTime? RefundTime
        { get; set; }

        /// <summary>
        /// 店铺拒绝(0接受，1拒绝)
        /// </summary>
        public int ShopIsReject
        { get; set; }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string ShopRejectReason
        { get; set; }

        /// <summary>
        /// 售后服务状态(-1取消,0用户提交,1商家审核(完成)
        /// </summary>
        public int RefundStatus
        { get; set; }

        /// <summary>
        /// 退货方式(1上门取件，2快递至卖家)
        /// </summary>
        public int BackMode
        { get; set; }

        /// <summary>
        /// 取件地址
        /// </summary>
        public string PickUpAddress
        { get; set; }

        /// <summary>
        /// 关闭状态(0未关闭，1已关闭；变动规则：服务完成后自动关闭、或是申请不合理店铺主动关闭服务单)
        /// </summary>
        public int IsClosed
        { get; set; }

        /// <summary>
        /// 服务单评价状态(0待评价，1已评价)
        /// </summary>
        public int EvaluateState
        { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreateTime
        { get; set; }
    }

}
