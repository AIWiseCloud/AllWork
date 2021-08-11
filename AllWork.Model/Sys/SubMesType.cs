using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    /// <summary>
    /// 辅助信息分类
    /// </summary>
    public class SubMesType
    {
        /// <summary>
        /// 分类代码
        /// </summary>
        [Required(ErrorMessage ="代码不能为空")]
        public string ID
        { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required(ErrorMessage ="名称不能为空")]
        public string FName
        { get; set; }
    }

}
