using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.User
{
    /// <summary>
    /// 个人认证
    /// </summary>
    public class UserCertification
    {
        /// <summary>
        /// 用户ID (不能重复）
        /// </summary>
        [Required(ErrorMessage ="UnionId不能为空")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        public string Name
        { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// 认证状态（0 提交申请，1认证通过）
        /// </summary>
        public int AuthState
        { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "身份证号不能为空")]
        public string CertificateId
        { get; set; }

        /// <summary>
        /// 证件照(头像面)
        /// </summary>
        [Required(ErrorMessage = "请上传身份证图片")]
        public string CertificateFront
        { get; set; }

        /// <summary>
        /// 证件照（国徽面）
        /// </summary>
        public string CertificateBack
        { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string CorpName
        { get; set; }

        /// <summary>
        /// 单位地址
        /// </summary>
        public string CorpAddress
        { get; set; }

        /*
         /// <summary>
         /// 销客职员ID
         /// </summary>
         public string OpenUserId
         {
             get; set;
         }

         /// <summary>
         /// 业务员
         /// </summary>
         public string SalesMan
         { get; set; }
         */

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateDate
        { get; set; }
    }

}
