using AllWork.Model.Sys;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class GoodsColor
    {
        [Required]
        public String ID
        { get; set; }

        [Required(ErrorMessage = "商品ID不能为空")]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 颜色描述
        /// </summary>
        [Required(ErrorMessage = "颜色描述不能为空")]
        public string ColorName
        { get; set; }

        /// <summary>
        /// 下面图片
        /// </summary>
        [Required(ErrorMessage = "商品正面图片不能为空")]
        public string ImgFront
        { get; set; }

        /// <summary>
        /// 背面图片
        /// </summary>
        public string ImgBack
        { get; set; }

        /// <summary>
        /// 右侧图
        /// </summary>
        public string ImgRight
        { get; set; }

        /// <summary>
        /// 左侧图
        /// </summary>
        public string ImgLeft
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int Findex
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }
    }

}
