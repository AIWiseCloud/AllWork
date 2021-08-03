using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AllWork.Model.Order
{
    public partial class OrderMain
    {
        public long OrderId
        { get; set; }

        [Required(ErrorMessage ="必须暖提供用户ID")]
        public string UnionId
        { get; set; }

        [Required(ErrorMessage = "配送方式不能为空")]
        public string DistributionMethod
        { get; set; }

        [Required(ErrorMessage = "收货人名称不能为空")]
        public string Receiver
        { get; set; }

        [Required(ErrorMessage = "收货人电话不能为空")]
        public string PhoneNumber
        { get; set; }

        [Required(ErrorMessage = "必须提供详细收货地址")]
        public string DeliveryAddress
        { get; set; }

        public int StatusId
        { get; set; }

        public decimal Amount
        { get; set; }

        public decimal Freight
        { get; set; }

        public decimal Discount
        { get; set; }

        [Range(0.01,100000000)]
        public decimal RealPay
        { get; set; }

        public string LogisticsId
        { get; set; }

        public string ExpressId
        { get; set; }

        public string PaymentChannel
        { get; set; }

        public DateTime? PayTime
        { get; set; }

        public string TradeNo
        { get; set; }

        public string Platform
        { get; set; }

        public string Words
        { get; set; }

        public DateTime? OrderTime
        { get; set; }

        public DateTime? DeliveryTime
        { get; set; }

        public DateTime? SigningTime
        { get; set; }

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
    }
}
