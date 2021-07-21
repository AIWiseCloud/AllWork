using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class GoodsInfo
    {
        [Required(ErrorMessage = "商品ID不能为空")]
        public string GoodsId
        { get; set; }

        [Required(ErrorMessage = "商品分类ID不能为空")]
        public string CategoryId
        { get; set; }

        public string ProdNumber
        { get; set; }

        [Required(ErrorMessage = "商品名称不能为空")]
        public string GoodsName
        { get; set; }

        public string GoodsDesc
        { get; set; }

        public int SalesTimes
        { get; set; }

        public byte IsRecommend
        { get; set; }

        public byte IsNew
        { get; set; }

        public byte IsUnder
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }

    }   

    public partial class GoodsInfo
    {
        public virtual IEnumerable<GoodsColor> GoodsColors { get; set; }
        public virtual IEnumerable<GoodsSpec> GoodsSpecs { get; set; }

        public GoodsInfo()
        {
            GoodsColors = new List<GoodsColor>();
            GoodsSpecs = new List<GoodsSpec>();
        }
    }

}
