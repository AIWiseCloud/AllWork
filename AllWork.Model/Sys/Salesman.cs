namespace AllWork.Model.Sys
{
    /// <summary>
    /// 业务员
    /// </summary>
    public class Salesman
    {
        /// <summary>
        /// ID
        /// </summary>
        public string OpenUserId
        { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string ProfileImageUrl
        { get; set; }

        /// <summary>
        /// 停用
        /// </summary>
        public int IsStop
        { get; set; }
    }
}
