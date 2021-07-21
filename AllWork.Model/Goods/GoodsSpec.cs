using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public class GoodsSpec
    {
        [Required]
        public String ID
        { get; set; }

        [Required]
        public string GoodsId
        { get; set; }

        [Required(ErrorMessage ="规格编号不能为空")]
        public string SpecId
        { get; set; }

        [Range(0, 10000)]
        public decimal Price
        { get; set; }

        [Range(0, 10000)]
        public decimal DiscountPrice
        { get; set; }

        public int Findex
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }
    }
}
