using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    /// <summary>
    /// 工程案例
    /// </summary>
    public class ProjectCase
    {
        public string ID
        { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage ="客户名称不能为空")]
        public string OrganizationName
        { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        [Required(ErrorMessage = "工程名称不能为空")]
        public string ProjectName
        { get; set; }

        /// <summary>
        /// 场地类型
        /// </summary>
        [Required(ErrorMessage = "场地类型不能为空")]
        public string SiteCategory
        { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        [Required(ErrorMessage = "区域不能为空")]
        public string Location
        { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public decimal Area
        { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Required(ErrorMessage = "请上传工程图片")]
        public string ImgUrl
        { get; set; }

        /// <summary>
        /// 合格验收日期
        /// </summary>
        public DateTime? InspectionDate
        { get; set; }

        /// <summary>
        /// 工程简介
        /// </summary>
        public string Summary
        { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime CreateDate
        { get; set; }

        /// <summary>
        /// 制单
        /// </summary>
        public string Creator
        { get; set; }

    }
}
