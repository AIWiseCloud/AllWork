using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class UserInfo
    {
        [Required(ErrorMessage ="UnionId不能为空")]
        public string UnionId
        { get; set; }

        [Required(ErrorMessage = "昵称不能为空")]
        public string NickName
        { get; set; }

        public string Password
        { get; set; }

        public string PhoneNumber
        { get; set; }

        public string Email
        { get; set; }

        [Required(ErrorMessage = "头像不能为空(从微信开放平台获取")]
        public string Avatar
        { get; set; }

        public string Province
        { get; set; }

        public string City
        { get; set; }

        public string County
        { get; set; }

        public string Gender
        { get; set; }

        public int UserState
        { get; set; }

        public string Roles
        { get; set; }

        public DateTime CreateDate
        { get; set; }
    }

}
