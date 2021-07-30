using AllWork.Model.Sys;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class StockBillDetail
    {
        [Required(ErrorMessage = "ID不能为空")]
        public string ID
        { get; set; }

        [Required(ErrorMessage = "交易单号不能为空")]
        public string BillId
        { get; set; }

        public int FIndex
        { get; set; }

        [Required(ErrorMessage = "商品ID不能为空")]
        public string GoodsId
        { get; set; }

        [Required(ErrorMessage = "仓库不能为空")]
        public string StockNumber
        { get; set; }

        [Required(ErrorMessage = "颜色能为空")]
        public string ColorId
        { get; set; }

        [Required(ErrorMessage = "规格不能为空")]
        public string SpecId
        { get; set; }

        public decimal Quantity
        { get; set; }
    }

    public partial class StockBillDetail
    {
        public GoodsInfo GoodsInfo { get; set; }
        public StockInfo Stock { get; set; }
        public ColorInfo ColorInfo { get; set; }
        public SpecInfo Spec { get; set; }
        public int IsNew { get; set; }
    }
}
