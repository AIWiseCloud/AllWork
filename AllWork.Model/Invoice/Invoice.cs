using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Invoice
{
    /// <summary>
    /// 发票记录
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Required]
        public long OrderId
        { get; set; }

        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal InvoAmt
        { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [Required(ErrorMessage ="用户标识不能为空")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 状态(-1已取消 0申请中 1已开票)
        /// </summary>
        public int StatusId
        { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime
        { get; set; }

        /// <summary>
        /// 开票时间
        /// </summary>
        public DateTime? InvoiceTime
        { get; set; }

        /// <summary>
        /// 发票类型(1普通发票、2增值税专用发票)
        /// </summary>
        public int InvoiceType
        { get; set; }

        /// <summary>
        /// 发票内容(0商品明细 1商品类别)
        /// </summary>
        public int ContentType
        { get; set; }

        /// <summary>
        /// 抬头类型(0个人，1单位)
        /// </summary>
        public int TitleType
        { get; set; }

        /// <summary>
        /// 抬头名称
        /// </summary>
        [Required(ErrorMessage = "抬头名称不能为空")]
        public string TitleName
        { get; set; }

        /// <summary>
        /// 公司税号
        /// </summary>
        public string TaxId
        { get; set; }

        /// <summary>
        /// 注册地址
        /// </summary>
        public string RegisterAddress
        { get; set; }

        /// <summary>
        /// 注册电话
        /// </summary>
        public string RegisterTel
        { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName
        { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount
        { get; set; }

        /// <summary>
        /// 收票人
        /// </summary>
        [Required(ErrorMessage = "收票人姓名不能为空")]
        public string Collector
        { get; set; }

        /// <summary>
        /// 收票人手机
        /// </summary>
        [Required(ErrorMessage = "收票人手机不能为空")]
        public string CollectorPhone
        { get; set; }

        /// <summary>
        /// 收票人地址
        /// </summary>
        [Required(ErrorMessage ="收票人地址不能为空")]
        public string CollectorAddr
        { get; set; }

        /// <summary>
        /// 收票人邮箱
        /// </summary>
        public string CollectorMail
        { get; set; }

        /// <summary>
        /// 开票人
        /// </summary>
        public string Drawer
        { get; set; }

        /// <summary>
        /// 发票图片
        /// </summary>
        public string InvoiceUrl
        { get; set; }

    }
}
