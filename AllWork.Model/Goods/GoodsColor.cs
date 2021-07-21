using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public class GoodsColor
    {
        [Required]
        public String ID
        { get; set; }

        [Required(ErrorMessage = "商品ID不能为空")]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 颜色编号在辅助资料设置，在这里选取
        /// </summary>
        [Required(ErrorMessage = "颜色编号不能为空")]
        public string ColorId
        { get; set; }

        [Required(ErrorMessage = "商品正面图片不能为空")]
        public string ImgFront
        { get; set; }

        public string ImgBack
        { get; set; }

        public string ImgRight
        { get; set; }

        public string ImgLeft
        { get; set; }

        public int Findex
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }
    }

}
