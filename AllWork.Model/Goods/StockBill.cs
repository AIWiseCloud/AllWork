using AllWork.Model.Sys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public partial class StockBill
    {
        /// <summary>
        /// 交易单号
        /// </summary>
        [Required(ErrorMessage ="交易单号不能为空")]
        public string BillId
        { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [Required(ErrorMessage ="业务类型不能为空")]
        public string TransTypeId
        { get; set; }

        /// <summary>
        /// 单据状态(0待审, 1已审, -1作废)
        /// </summary>
        [Range(-1,1)]
        public int BillState
        { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
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

    public partial class StockBill
    {
        public virtual List<StockBillDetail> StockBillDetail { get; set; }

        public StockBill()
        {
            this.StockBillDetail = new List<StockBillDetail>();
        }
    }

    public class StockBillExt : StockBill
    {
        public TransTypeInfo TransType { get; set; }

        public new List<StockBillDetailExt> StockBillDetail { get; set; }

        public StockBillExt()
        {
            this.StockBillDetail = new List<StockBillDetailExt>();
        }
    }

}
