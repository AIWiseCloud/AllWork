using AllWork.Model.Goods;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.ShopCart
{
    public class Cart
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required(ErrorMessage = "unionId字段不能为空!")]
        public string UnionId
        { get; set; }

        [Required(ErrorMessage = "商品ID不能为空!")]
        public string GoodsId
        { get; set; }

        [Required(ErrorMessage = "颜色ID不能为空!")]
        public string ColorId
        { get; set; }

        [Required(ErrorMessage = "规格ID不能为空!")]
        public string SpecId
        { get; set; }

        /// <summary>
        /// 购物车数量
        /// </summary>
        [Range(1, 100000)]
        public int Quantity
        { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public byte Selected
        { get; set; }

        public DateTime CreateDate
        { get; set; }
    }

    public class CartEx : Cart
    {
        public GoodsInfo GoodsInfo { get; set; }
        public GoodsColor GoodsColor { get; set; }
        public GoodsSpec GoodsSpec { get; set; }
        public Inventory Inventory { get; set; }
    }
}
