using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public partial class UserInfo
    {
        [Required(ErrorMessage ="UnionId不能为空")]
        public string UnionId
        { get; set; }

        public string OpenId
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

        [Required(ErrorMessage ="角色不能为空，多个角色以逗号分隔")]
        public string Roles
        { get; set; }

        public DateTime CreateDate
        { get; set; }
    }

    public partial class UserInfo
    {
        public string[] UserRoles 
        {
            get {

                if (string.IsNullOrEmpty(this.Roles)) return "editor".Split(","); else return this.Roles.Split(","); }
        }
    }

}
