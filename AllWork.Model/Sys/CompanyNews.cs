using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class CompanyNews
    {
        /// <summary>
        /// 新闻ID
        /// </summary>
        public string NewsId
        { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage ="标题不能为空")]
        public string Title
        { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        [Required(ErrorMessage = "请上传封面图片")]
        public string ImgUrl
        { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [Required(ErrorMessage = "正文不能为空")]
        public string Body
        { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source
        { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public string Creator
        { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateDate
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int FIndex
        { get; set; }

        /// <summary>
        /// 状态(-1下架，0草稿, 1提交审核, 2审核通过）
        /// </summary>
        public int StatusId
        { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int AmountReading
        { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int NumberLike
        { get; set; }
    }

}
