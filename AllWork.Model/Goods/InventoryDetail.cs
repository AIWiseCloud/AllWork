using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.Goods
{
    public class InventoryDetail
    {
        public string ID
        { get; set; }

        public string SkuId
        { get; set; }

        public string StockNumber
        { get; set; }

        public decimal Quantity
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }
    }
}
