using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public class StockBill
    {
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

}
