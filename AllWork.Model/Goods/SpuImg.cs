using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public class SpuImg
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID
        { get; set; }

        /// <summary>
        /// 商品iD
        /// </summary>
        [Required(ErrorMessage ="商品ID不能为空")]
        public string GoodsId
        { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int FIndex
        { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Required(ErrorMessageResourceName ="请上传图片")]
        public string ImgUrl
        { get; set; }
    }

}
