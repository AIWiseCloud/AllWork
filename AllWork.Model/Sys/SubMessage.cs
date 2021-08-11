using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    /// <summary>
    /// 辅助信息
    /// </summary>
    public class SubMessage
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 上级代码
        /// </summary>
        [Required(ErrorMessage ="父节点代码不能为空")]
        public string ParentId
        { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>
        [Required(ErrorMessage = "项目代码不能为空")]
        public string FNumber
        { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Required(ErrorMessage = "项目名称不能为空")]
        public string FName
        { get; set; }

        /// <summary>
        /// 是否作废
        /// </summary>
        public int IsCancellation
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int FIndex
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string FNote
        { get; set; }
    }
}
