using System;
using System.Collections.Generic;
using System.Text;

namespace AllWork.Model.RequestParams
{
    public class InventoryParams
    {
        /// <summary>
        /// 商品分类Id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 商品状态（0全部，1销售中, 2 已下架
        /// </summary>
        public int GoodsState { get; set; }

        /// <summary>
        /// 分页实体参数
        /// </summary>
        public PageModel PageModel { get; set; }
    }
}
