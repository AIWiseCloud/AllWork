using AllWork.Model.Goods;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.ShopCart
{
    public class Cart
    {
        public string ID
        { get; set; }

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

        [Range(1, 100000)]
        public int Quantity
        { get; set; }

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
    }
}
