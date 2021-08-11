using AllWork.Model.Goods;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Order
{
    public class OrderList
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public long OrderId
        { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        [Required]
        public int LineId
        { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        [Required]
        public string ColorId
        { get; set; }

        /// <summary>
        /// 规格ID
        /// </summary>
        [Required]
        public string SpecId
        { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Range(1,9999)]
        public decimal Quantity
        { get; set; }

        /// <summary>
        ///单价
        /// </summary>
        [Range(0.01, 9999)]
        public decimal UnitPrice
        { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount
        { get; set; }

        /// <summary>
        /// 是否评论
        /// </summary>
        public byte Evaluate
        { get; set; }

    }

    public class OrderListExt : OrderList
    {
        public GoodsInfo GoodsInfo { get; set; }

        public GoodsColor GoodsColor { get; set; }

        public GoodsSpec GoodsSpec { get; set; }

        public GoodsColorSpec GoodsColorSpec { get; set; }
    }
}
