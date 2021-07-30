using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class StockBill
    {
        public StockBill()
        {
            StockBillDetail = new List<StockBillDetail>();
        }
        [Required(ErrorMessage ="交易单号不能为空")]
        public string BillId
        { get; set; }

        [Required(ErrorMessage ="业务类型不能为空")]
        public string TransTypeId
        { get; set; }

        public int BillState
        { get; set; }

        public string Remark
        { get; set; }

        public DateTime CreateDate
        { get; set; }

        public string Creator
        { get; set; }
    }

    public partial class StockBill
    {
        public TransTypeInfo TransType { get; set; }
        public List<StockBillDetail> StockBillDetail { get; set; }
    }
}
