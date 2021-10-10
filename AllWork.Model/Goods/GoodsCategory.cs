using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AllWork.Model.Goods
{
    public class GoodsCategory
    {
        /// <summary>
        /// 商品分类ID
        /// </summary>
        [Required(ErrorMessage ="商品分类代码不能为空"),MaxLength(ErrorMessage ="商品分类代码最大长度50")]
        public string CategoryId
        { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        [Required(ErrorMessage ="商品分类名称不能为空")]
        public string CategoryName
        { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [Required(ErrorMessage ="所属店铺代码不能为空")]
        public string ShopId
        { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string ImgUrl
        { get; set; }

        /// <summary>
        /// 上级分类ID
        /// </summary>
        public string ParentId
        { get; set; }

        /// <summary>
        /// 是否主材分类(0副材,1主材)
        /// </summary>
        public int IsMainMaterial
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int Findex
        { get; set; }

        /// <summary>
        /// 作废
        /// </summary>
        public int IsCancellation
        { get; set; }

        public IList<GoodsCategory> Children { get; set; }
    }

    
}
