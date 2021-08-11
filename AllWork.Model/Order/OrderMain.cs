using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AllWork.Model.Order
{
    public partial class OrderMain
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId
        { get; set; }

        [Required(ErrorMessage ="必须暖提供用户ID")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 配送方式(1快递、2自提)
        /// </summary>
        [Required(ErrorMessage = "配送方式不能为空")]
        public string DistributionMethod
        { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [Required(ErrorMessage = "收货人名称不能为空")]
        public string Receiver
        { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        [Required(ErrorMessage = "收货人电话不能为空")]
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [Required(ErrorMessage = "必须提供详细收货地址")]
        public string DeliveryAddress
        { get; set; }

        /// <summary>
        /// 订单状态(取值：0待付款; 1待发货; 2待收货; 3已签收; -1已取消; -2已删除）
        /// </summary>
        public int StatusId
        { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount
        { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight
        { get; set; }

        /// <summary>
        /// 折扣额
        /// </summary>
        public decimal Discount
        { get; set; }

        /// <summary>
        /// 实际支付
        /// </summary>
        [Range(0.01,100000000)]
        public decimal RealPay
        { get; set; }

        /// <summary>
        /// 物流公司ID
        /// </summary>
        public string LogisticsId
        { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressId
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
        /// 交易单号
        /// </summary>
        public string TradeNo
        { get; set; }

        /// <summary>
        /// 下单平台(app,web,mp)
        /// </summary>
        public string Platform
        { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        public string Words
        { get; set; }

        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime? OrderTime
        { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime? DeliveryTime
        { get; set; }

        /// <summary>
        /// 签收日期
        /// </summary>
        public DateTime? SigningTime
        { get; set; }

        /// <summary>
        /// 取消日期
        /// </summary>
        public DateTime? CancelTime
        { get; set; }

    }

    public partial class OrderMain
    {
        public OrderMain()
        {
            this.OrderList = new List<OrderList>();
        }

        public virtual List<OrderList> OrderList { get; set; }
    }

    public class OrderMainExt : OrderMain
    {
        public new List<OrderListExt> OrderList { get; set; }

        public OrderMainExt()
        {
            this.OrderList = new List<OrderListExt>();
        }
    }

}
