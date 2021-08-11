namespace AllWork.Model
{
    /// <summary>
    /// 分页参数实体
    /// </summary>
    public class PageModel
    {
        private string _orderWay = "ASC";
        private int _pageNo = 1;
        private int _pageSize = 20;

        /// <summary>
        /// 页号（1,2,3...)
        /// </summary>
        public int PageNo
        {
            get { return _pageNo; }
            set { _pageNo = value; }
        }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 排序栏位
        /// </summary>
        public string OrderField
        { get; set; }

        /// <summary>
        /// 排序方法
        /// </summary>
        public string OrderWay
        {
            get { return _orderWay; }

            set { _orderWay = value; }
        }

        /// <summary>
        /// Skip(mysql语句limit中第一个参数用
        /// </summary>
        public int Skip { get { return (PageNo - 1) * PageSize; } }
    }
}
