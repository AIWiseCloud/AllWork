namespace AllWork.Model.RequestParams
{
    public class UserCertificationParams:CommonParams,IDataRange
    {
        /// <summary>
        /// 开始日期(QueryScheme = 0时用)
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 截止日期(QueryScheme = 0时用)
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 认证类型(-1全部,0个人认证，1企业认证）
        /// </summary>
        public int CertificateType { get; set; }
    }
}
