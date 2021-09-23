using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public  class GoodsSpec
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Required]
        public String ID
        { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 规格描述
        /// </summary>
        [Required(ErrorMessage ="规格描述不能为空")]
        public string SpecName
        { get; set; }

        /// <summary>
        /// 销售单位
        /// </summary>
        [Required(ErrorMessage = "销售单位不能为空")]
        public string SaleUnit
        { get; set; }

        /// <summary>
        /// 单位转换(1销售单位等于多少基本单位)
        /// </summary>
        public decimal UnitConverter
        { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        [Range(0.01, 20000)]
        public decimal Price
        { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        [Range(0.01, 20000)]
        public decimal DiscountPrice
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int Findex
        { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        { get; set; }
    }

}
