using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class GoodsInfo
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [Required(ErrorMessage = "商品ID不能为空")]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 商品分类ID
        /// </summary>
        [Required(ErrorMessage = "商品分类ID不能为空")]
        public string CategoryId
        { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProdNumber
        { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Required(ErrorMessage = "商品名称不能为空")]
        public string GoodsName
        { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        [Required(ErrorMessage ="品牌不能为空")]
        public string Brand
        { get; set; }

        /// <summary>
        /// 配比
        /// </summary>
        [Required(ErrorMessage = "配比不能为空")]
        public string Mixture
        { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string GoodsDesc
        { get; set; }

        /// <summary>
        /// 基本单位
        /// </summary>
        [Required(ErrorMessageResourceName ="基本单位不能为空")]
        public string UnitName
        { get; set; }

        /// <summary>
        /// 基本单位单价
        /// </summary>
        public decimal BaseUnitPrice
        { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesTimes
        { get; set; }

        /// <summary>
        /// 是否推荐商品
        /// </summary>
        public byte IsRecommend
        { get; set; }

        /// <summary>
        /// 是否最新商品
        /// </summary>
        public byte IsNew
        { get; set; }

        /// <summary>
        /// 是否下架: 0发布，1下架
        /// </summary>
        public byte IsUnder
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

    public partial class GoodsInfo
    {
       
    }

    public class GoodsInfoExt : GoodsInfo
    {
        public virtual List<GoodsColor> GoodsColors { get; set; }
        public virtual List<GoodsSpec> GoodsSpecs { get; set; }
        public virtual List<SpuImg> SpuImgs { get; set; }

        public GoodsInfoExt()
        {
            GoodsColors = new List<GoodsColor>();
            GoodsSpecs = new List<GoodsSpec>();
            SpuImgs = new List<SpuImg>();
        }
    }

}
