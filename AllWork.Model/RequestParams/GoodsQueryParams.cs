using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 商品查询参数
    /// </summary>
    public class GoodsQueryParams
    {
        /// <summary>
        /// 查询方案：0关键字查询 1商品分类 2推荐商品 3最新商品
        /// </summary>
        [Range(0,3)]
        public int QueryScheme { get; set; }

        /// <summary>
        /// 隐藏下架商品(0显示，1隐藏)
        /// </summary>
        public int HideUnderGoods { get; set; }

        /// <summary>
        /// 查询值(与查询方案对应)
        /// </summary>
       　public string QueryValue { get; set; }

        //分页参数实体
        public PageModel PageModel { get; set; }
    }
}
