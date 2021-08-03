using AllWork.Model.Sys;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class Inventory 
    {
        public string SkuId
        { get; set; }

        [Required(ErrorMessage ="商品ID不能为空")]
        public string GoodsId
        { get; set; }

        [Required(ErrorMessage = "颜色ID不能为空")]
        public string ColorId
        { get; set; }

        [Required(ErrorMessage = "规格ID不能为空")]
        public string SpecId
        { get; set; }

        public decimal Quantity
        { get; set; }

        public decimal LockQuantity
        { get; set; }

        public decimal TransitQuantity
        { get; set; }

        public decimal ActiveQuantity
        { get; set; }
    }

    public partial class Inventory
    {
        public GoodsInfo GoodsInfo { get; set; }
        public ColorInfo ColorInfo { get; set; }
        public SpecInfo SpecInfo { get; set; }
        public GoodsCategory GoodsCategory { get; set; }
    }


}
