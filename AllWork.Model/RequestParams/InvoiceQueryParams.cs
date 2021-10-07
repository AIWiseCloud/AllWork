using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.RequestParams
{
    public class InvoiceQueryParams:CommonParams
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required(ErrorMessage ="用户标识不能为空")]
        public string UnionId { get; set; }
    }
}
