using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Invoice
{
    /// <summary>
    /// 发票抬头
    /// </summary>
    public class InvoiceTitle
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [Required(ErrorMessage ="用户标识不能为空")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 发票类型( 1普通发票、2自值税专用发票)
        /// </summary>
        [Required]
        public int InvoiceType
        { get; set; }

        /// <summary>
        /// 抬头类型(抬头类型  0个人，1单位)
        /// </summary>
        [Required]
        public int TitleType
        { get; set; }

        /// <summary>
        /// 抬头名称
        /// </summary>
        [Required(ErrorMessage = "抬头名称不能为空")]
        public string TitleName
        { get; set; }

        /// <summary>
        /// 发票内容( 0商品明细 1商品类别)
        /// </summary>
        [Required]
        public int ContentType
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
        [Required(ErrorMessage = "收票人地址不能为空")]
        public string CollectorAddr
        { get; set; }

        /// <summary>
        /// 收票人邮箱
        /// </summary>
        public string CollectorMail
        { get; set; }

    }
}
