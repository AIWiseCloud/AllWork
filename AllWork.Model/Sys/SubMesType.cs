using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class SubMesType
    {

        [Required(ErrorMessage ="代码不能为空")]
        public string ID
        { get; set; }

        [Required(ErrorMessage ="名称不能为空")]
        public string FName
        { get; set; }
    }

}
