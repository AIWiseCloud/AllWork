using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    /// <summary>
    /// 资源设定
    /// </summary>
    public class ResourceSettings
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public string SourceId
        { get; set; }

        /// <summary>
        /// 资源主旨
        /// </summary>
        [Required(ErrorMessage ="资源主旨不能为空"),MaxLength(200,ErrorMessage ="最大长度为200")]
        public string Subject
        { get; set; }

        /// <summary>
        /// 分组码
        /// </summary>
        [Required(ErrorMessage = "分组码不能为空")]
        public string GroupNo
        { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl
        { get; set; }

        /// <summary>
        /// 导航地址
        /// </summary>
        public string Navigator
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(4000,ErrorMessage ="最大长度4000")]
        public string Remark
        { get; set; }

        /// <summary>
        /// 作废
        /// </summary>
        [Range(0,1)]
        public int IsCancellation
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int FIndex
        { get; set; }
    }
}
