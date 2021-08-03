using AllWork.Model.Goods;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Order
{
    public class OrderList
    {
        [Required]
        public long OrderId
        { get; set; }

        [Required]
        public int LineId
        { get; set; }

        [Required]
        public string GoodsId
        { get; set; }

        [Required]
        public string ColorId
        { get; set; }

        [Required]
        public string SpecId
        { get; set; }

        [Range(1,9999)]
        public decimal Quantity
        { get; set; }

        [Range(1, 9999)]
        public decimal UnitPrice
        { get; set; }

        public string Unit
        { get; set; }

        public decimal Amount
        { get; set; }

        public byte Evaluate
        { get; set; }

    }

    public class OrderListExt : OrderList
    {
        public GoodsInfo GoodsInfo { get; set; }

        public GoodsColor GoodsColor { get; set; }

        public GoodsSpec GoodsSpec { get; set; }
    }
}
