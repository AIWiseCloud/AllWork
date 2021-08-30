using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.User
{
    public partial class UserInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage ="UnionId不能为空")]
        public string UnionId
        { get; set; }

        public string OpenId
        { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }

        [Required(ErrorMessage = "昵称不能为空")]
        public string NickName
        { get; set; }

        public string Password
        { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email
        { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Required(ErrorMessage = "头像不能为空(从微信开放平台获取")]
        public string Avatar
        { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Required]
        public string Province
        { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City
        { get; set; }

        /// <summary>
        /// 区/县
        /// </summary>
        public string County
        { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        { get; set; }

        /// <summary>
        /// 用户状态(0退出, 1登录, -1锁定)
        /// </summary>
        public int UserState
        { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        [Required(ErrorMessage ="角色不能为空，多个角色以逗号分隔")]
        public string Roles
        { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
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
