using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Order
{
    /// <summary>
    /// 预付定金
    /// </summary>
    public class AdvanceMoney
    {
        /// <summary>
        /// 预付单号(以6开头的数字序列）
        /// </summary>
        public long ID
        { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId
        { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UnionId
        { get; set; }

        /// <summary>
        /// 定金
        /// </summary>
        public decimal DownPayment
        { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary
        { get; set; }

        /// <summary>
        /// 支付方式(0在线支付 1对公转账)
        /// </summary>
        [Range(0, 1)]
        public int PaymentWay
        { get; set; }

        /// <summary>
        /// 支付渠道(alipay, wechatpay,unionpay)
        /// </summary>
        public string PaymentChannel
        { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime
        { get; set; }

        /// <summary>
        /// 交易流水号
        /// </summary>
        public string TradeNo
        { get; set; }

        /// <summary>
        /// 支付凭证（图)
        /// </summary>
        public string PayVoucherUrl
        { get; set; }

        /// <summary>
        /// 确认状态(0未确认 1已确认)
        /// </summary>
        [Range(0, 1)]
        public int ConfirmStatus
        { get; set; }

        /// <summary>
        /// 确认人
        /// </summary>
        public string Confirmer
        { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate
        { get; set; }

    }

    public class AdvanceMoneyExt : AdvanceMoney
    {
        public OrderMain OrderMain { get; set; } = new OrderMain { };
    }
}
