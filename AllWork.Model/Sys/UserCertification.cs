using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class UserCertification
    {
        [Required(ErrorMessage ="UnionId不能为空")]
        public string UnionId
        { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        public string Name
        { get; set; }

        public int AuthType
        { get; set; }

        public int AuthState
        { get; set; }

        [Required(ErrorMessage = "证件编号不能为空")]
        public string CertificateId
        { get; set; }

        [Required(ErrorMessage = "证件图片不能为空")]
        public string CertificateFront
        { get; set; }

        public string CertificateBack
        { get; set; }

        public string CorpName
        { get; set; }

        public string CorpAddress
        { get; set; }

        [Required(ErrorMessage = "业务员不能为空")]
        public string SalesMan
        { get; set; }

        public DateTime CreateDate
        { get; set; }
    }

}
