using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.RequestParams
{
    public class UserParams:CommonParams,IDataRange
    {
        /// <summary>
        /// 开始日期(QueryScheme = 0时用)
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 截止日期(QueryScheme = 0时用)
        /// </summary>
        public string EndDate { get; set; }
    }
}
