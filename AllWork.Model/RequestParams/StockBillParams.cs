using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 出入库单据查询请求参数
    /// </summary>
    public class StockBillParams
    {
        /// <summary>
        /// 出入库单号
        /// </summary>
        public string BillId { get; set; }

        /// <summary>
        /// 业务类型ID
        /// </summary>
        public string TransTypeId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        public PageModel PageModel { get; set; }
    }
}
