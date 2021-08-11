using AllWork.Model.Sys;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class StockBillDetail
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Required(ErrorMessage = "ID不能为空")]
        public string ID
        { get; set; }

        /// <summary>
        /// 交易单号
        /// </summary>
        [Required(ErrorMessage = "交易单号不能为空")]
        public string BillId
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int FIndex
        { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Required(ErrorMessage = "商品ID不能为空")]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary>
        [Required(ErrorMessage = "仓库不能为空")]
        public string StockNumber
        { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        [Required(ErrorMessage = "颜色能为空")]
        public string ColorId
        { get; set; }

        /// <summary>
        /// 规格ID
        /// </summary>
        [Required(ErrorMessage = "规格不能为空")]
        public string SpecId
        { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity
        { get; set; }
    }

    public partial class StockBillDetail
    {
        public int IsNew { get; set; }
    }

    public class StockBillDetailExt : StockBillDetail
    {
        public GoodsInfo GoodsInfo { get; set; }
        public StockInfo Stock { get; set; }
        public ColorInfo ColorInfo { get; set; }
        public SpecInfo Spec { get; set; }
    }
}
