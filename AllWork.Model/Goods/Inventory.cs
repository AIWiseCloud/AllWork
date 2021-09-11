using AllWork.Model.Sys;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class Inventory 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string SkuId
        { get; set; }

        /// <summary>
        /// 商品iD
        /// </summary>
        [Required(ErrorMessage ="商品ID不能为空")]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        [Required(ErrorMessage = "颜色ID不能为空")]
        public string ColorId
        { get; set; }

        /// <summary>
        /// 规格ID
        /// </summary>
        [Required(ErrorMessage = "规格ID不能为空")]
        public string SpecId
        { get; set; }

        /// <summary>
        /// 实际库存
        /// </summary>
        public decimal Quantity
        { get; set; }

        /// <summary>
        /// 锁定库存
        /// </summary>
        public decimal LockQuantity
        { get; set; }

        /// <summary>
        /// 在途库存
        /// </summary>
        public decimal TransitQuantity
        { get; set; }

        /// <summary>
        /// 可用库存
        /// </summary>
        public decimal ActiveQuantity
        { get; set; }
    }

    public partial class Inventory
    {
        public GoodsInfo GoodsInfo { get; set; }
        public GoodsColor GoodsColor { get; set; }
        public GoodsSpec GoodsSpec { get; set; }
        public GoodsCategory GoodsCategory { get; set; }
    }


}
