using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Goods
{
    public class SpuImg
    {
        public string ID
        { get; set; }

        [Required(ErrorMessage ="商品ID不能为空")]
        public string GoodsId
        { get; set; }

        public int FIndex
        { get; set; }

        [Required(ErrorMessageResourceName ="请上传图片")]
        public string ImgUrl
        { get; set; }
    }

}
