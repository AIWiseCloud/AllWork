using System;

namespace AllWork.Model.DataCenter
{
    public class AppVisits
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int ID
        { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime FDate
        { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Visits
        { get; set; }
    }
}
