using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.Goods
{
    public class InventoryDetail
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 库存表ID
        /// </summary>
        public string SkuId
        { get; set; }

        /// <summary>
        /// 仓库代码
        /// </summary>
        public string StockNumber
        { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity
        { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        { get; set; }
    }
}
