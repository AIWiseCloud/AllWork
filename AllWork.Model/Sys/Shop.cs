using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class Shop 
    {
        [Required(ErrorMessage ="请设置店铺编号")]
        public string ShopId
        { get; set; }

        [Required(ErrorMessage = "店铺名称不能为空")]
        public string ShopName
        { get; set; }

        public string ImgUrl
        { get; set; }

        public string Contacter
        { get; set; }

        [Phone(ErrorMessage ="请提供正确的手机号")]
        public string PhoneNumber
        { get; set; }

        public int ListBySpuShow
        { get; set; }

        [MaxLength(1000, ErrorMessage = "最大长度为1000")]
        public string Introduction
        { get; set; }

        [MaxLength(1000, ErrorMessage = "最大长度为1000")]
        public string Announcement
        { get; set; }

        public DateTime? CreateDate
        { get; set; }
    }

}
