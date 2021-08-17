using AllWork.Model.Goods;
using AllWork.Model.Sys;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Order
{
    /// <summary>
    /// 订单评价
    /// </summary>
    public class OrderEvaluate
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId
        { get; set; }

        /// <summary>
        /// 订单行号
        /// </summary>
        [Range(1,100)]
        public int LineId
        { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Required]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [MinLength(10,ErrorMessage ="请提供10字以上评价")]
        [MaxLength(1000,ErrorMessage ="评价不超过1000字")]
        public string Content
        { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required(ErrorMessage ="用户ID不能为空")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 商品评分
        /// </summary>
        [Range(0,5)]
        public int GoodsScore
        { get; set; }

        /// <summary>
        /// 服务评分
        /// </summary>
        [Range(0, 5)]
        public int ServiceScore
        { get; set; }

        /// <summary>
        /// 时效评分
        /// </summary>
        [Range(0, 5)]
        public int TimeScore
        { get; set; }

        /// <summary>
        /// 店铺回复
        /// </summary>
        [MaxLength(1000)]
        [MinLength(1)]
        public string ShopReply
        { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? ReplyTime
        { get; set; }

        /// <summary>
        /// 图片/视频地址
        /// </summary>
        [MaxLength(2000,ErrorMessage ="图片视频地址长度不能超过2000")]
        public string Images
        { get; set; }

        /// <summary>
        /// 隐藏(1隐藏，0显示)
        /// </summary>
        public int IsHide
        { get; set; }
    }

    public class OrderEvaluateExt : OrderEvaluate
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 颜色规格
        /// </summary>
        public GoodsColorSpec GoodsColorSpec { get; set; }
    }

}
