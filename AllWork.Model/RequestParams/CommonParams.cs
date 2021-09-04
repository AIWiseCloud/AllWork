namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 通用查询请求参数
    /// </summary>
    public class CommonParams
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        public PageModel PageModel { get; set; }
    }
}
