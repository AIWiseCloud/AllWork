using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 资源查询请求参数
    /// </summary>
    public class ResourceParams
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }

        public PageModel PageModel { get; set; }
    }
}
