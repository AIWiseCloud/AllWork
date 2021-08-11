using AllWork.Model.Sys;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class GoodsSpec
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
        /// 规格编号（在辅助资料设置，此处选用)
        /// </summary>
        [Required(ErrorMessage ="规格编号不能为空")]
        public string SpecId
        { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Range(0.01, 10000)]
        public decimal Price
        { get; set; }

        /// <summary>
        /// 折扣价
        /// </summary>
        [Range(0.01, 10000)]
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

    public partial class GoodsSpec
    {
        public SpecInfo Spec { get; set; }
    }
}
