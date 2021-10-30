using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 查询我的客户参数
    /// </summary>
    public class CustomerParams
    {
        [Required]
        public string UnionId
        { get; set; }

        public PageModel PageModel
        { get; set; }
    }
}
