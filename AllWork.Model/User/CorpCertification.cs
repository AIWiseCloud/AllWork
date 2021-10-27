using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.User
{
    /// <summary>
    /// 企业认证
    /// </summary>
    public class CorpCertification
    {
        /// <summary>
        /// 用户ID (不能重复）
        /// </summary>
        [Required(ErrorMessage = "UnionId不能为空")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Required(ErrorMessage = "联系人不能为空")]
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
        /// 统一社会信用代码
        /// </summary>
        [Required(ErrorMessage = "统一社会信用代码不能为空")]
        public string CertificateId
        { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        [Required(ErrorMessage = "请上传营业执照图片")]
        public string CertificateFront
        { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Required(ErrorMessage = "单位名称不能为空")]
        public string CorpName
        { get; set; }

        /// <summary>
        /// 单位地址
        /// </summary>
        [Required(ErrorMessage = "单位地址不能为空")]
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
