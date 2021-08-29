using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    /// <summary>
    /// 版本管理
    /// </summary>
    public class VersionManagement
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [Required(ErrorMessage ="版本号不能为空")]
        public string VersionId
        { get; set; }

        /// <summary>
        /// 包文件Url
        /// </summary>
        [Required(ErrorMessage = "请上传更新包")]
        public string PackageUrl
        { get; set; }

        /// <summary>
        /// 版本说明
        /// </summary>
        [Required(ErrorMessage = "请输入版本说明")]
        public string FNote
        { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public string Creator
        { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime CreateDate
        { get; set; }
    }
}
