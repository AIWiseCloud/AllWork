using System;

namespace AllWork.Model.Goods
{
    public class Inventory 
    {

        public string ID
        { get; set; }

        public string GoodsId
        { get; set; }

        public string ColorId
        { get; set; }

        public string SpecId
        { get; set; }

        public string StockNumber
        { get; set; }

        public decimal Quantity
        { get; set; }

        public decimal LockQuantity
        { get; set; }

        public decimal TransitQuantity
        { get; set; }

        public decimal ActiveQuantity
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }
    }

}
