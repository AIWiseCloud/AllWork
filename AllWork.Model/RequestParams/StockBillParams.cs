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
        public string BillId { get; set; }

        public string TransTypeId { get; set; }

        public string GoodsName { get; set; }

        public PageModel PageModel { get; set; }
    }
}
