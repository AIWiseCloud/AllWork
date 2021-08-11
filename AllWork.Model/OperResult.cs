
namespace AllWork.Model
{
    /// <summary>
    /// 操作结果类（为异步调用服务
    /// </summary>
    public class OperResult
    {
        /// <summary>
        /// 返回主键
        /// </summary>
        public string IdentityKey { get; set; }

        /// <summary>
        /// 异常输出
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
    }
}
