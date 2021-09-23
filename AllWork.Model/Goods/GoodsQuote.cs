using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    /// <summary>
    /// 材料报价（用于打印预览)
    /// </summary>
    public class GoodsQuote
    {
        /// <summary>
        /// 材料分类ID
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 材料名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 配比
        /// </summary>
        public string Mixture { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 盛天报价
        /// </summary>
        public decimal PriceST { get; set; }
        /// <summary>
        /// 盛望报价
        /// </summary>
        public decimal PriceSW { get; set; }
        /// <summary>
        /// 倚天剑报价
        /// </summary>
        public decimal PriceYTJ { get; set; }
        /// <summary>
        /// 包装规格
        /// </summary>
        public string SpecName { get; set; }
        /// <summary>
        /// 材料大类
        /// </summary>
        public string ParentName { get; set; }
    }
}
