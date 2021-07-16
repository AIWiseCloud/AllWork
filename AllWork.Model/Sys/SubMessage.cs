using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class SubMessage
    {
        public Guid ID
        { get; set; }

        [Required(ErrorMessage ="父节点代码不能为空")]
        public string ParentId
        { get; set; }

        [Required(ErrorMessage = "项目代码不能为空")]
        public string FNumber
        { get; set; }

        [Required(ErrorMessage = "项目名称不能为空")]
        public string FName
        { get; set; }

        public int IsCancellation
        { get; set; }

        public int FIndex
        { get; set; }

        public string FNote
        { get; set; }
    }
}
