using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    public class GoodsEvaluatePraams
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [Required(ErrorMessage ="商品ID不能为空")]
        public string GoodsId { get; set; }

        public PageModel PageModel { get; set; }
    }
}
