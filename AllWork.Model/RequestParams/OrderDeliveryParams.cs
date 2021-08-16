namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 订单发货参数
    /// </summary>
    public class OrderDeliveryParams
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 发货动作（1发货,0取消发货)
        /// </summary>
        public int IsDelivery { get; set; }

        /// <summary>
        /// 销售出库单号
        /// </summary>
        public string BillId { get; set; }

        /// <summary>
        /// 物流公司ID
        /// </summary>
        public string LogisticsId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressId { get; set; }
    }
}
