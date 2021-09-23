using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class Shop
    {
        /// <summary>
        /// 店铺编号
        /// </summary>
        [Required(ErrorMessage = "请设置店铺编号")]
        public string ShopId
        { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [Required(ErrorMessage = "店铺名称不能为空")]
        public string ShopName
        { get; set; }

        /// <summary>
        /// 店铺图像
        /// </summary>
        public string ImgUrl
        { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacter
        { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        //[Phone(ErrorMessage ="请提供正确的手机号")]
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// 列表按SPU显示
        /// </summary>
        public int ListBySpuShow
        { get; set; }

        /// <summary>
        /// 店铺简介
        /// </summary>
        [MaxLength(1000, ErrorMessage = "最大长度为1000")]
        public string Introduction
        { get; set; }

        /// <summary>
        /// 店铺通知
        /// </summary>
        [MaxLength(1000, ErrorMessage = "最大长度为1000")]
        public string Announcement
        { get; set; }

        /// <summary>
        /// 收款人户名
        /// </summary>
        public string AccountName
        { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo
        { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string DepositBank
        { get; set; }

        /// <summary>
        /// 银联号
        /// </summary>
        public string CnapsCode
        { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate
        { get; set; }
    }

}
