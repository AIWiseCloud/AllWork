using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AllWork.Model.Goods
{
    public class GoodsCategory
    {
        [Required(ErrorMessage ="商品分类代码不能为空"),MaxLength(ErrorMessage ="商品分类代码最大长度50")]
        public string CategoryId
        { get; set; }

        [Required(ErrorMessage ="商品分类名称不能为空")]
        public string CategoryName
        { get; set; }

        [Required(ErrorMessage ="所属店铺代码不能为空")]
        public string ShopId
        { get; set; }

        public string ImgUrl
        { get; set; }

        public string ParentId
        { get; set; }

        public int Findex
        { get; set; }

        public int IsCancellation
        { get; set; }

        public IList<GoodsCategory> Children { get; set; }
    }

    
}
