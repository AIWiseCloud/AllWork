using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.RequestParams
{
    public class InventoryParams
    {
        public string CategoryId { get; set; }

        public string GoodsName { get; set; }

        /// <summary>
        /// 商品状态（0全部，1销售中, 2 已下架
        /// </summary>
        public int GoodsState { get; set; }

        public PageModel PageModel { get; set; }
    }
}
