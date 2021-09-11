namespace AllWork.Model.RequestParams
{
    public class NewsParams:CommonParams
    {
        /// <summary>
        /// 仅显示提交状态的记录(0不是，1是
        /// </summary>
        public int OnlyShowSubmitStatus
        { get; set; }
    }
}
