using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Address
{
    public class ReceiveAddress
    {
        public string AddrId
        { get; set; }

        [Required(ErrorMessage ="用户ID不能为空")]
        public string UnionId
        { get; set; }

        [Required(ErrorMessage = "收货人姓名不能为空")]
        public string Receiver
        { get; set; }

        public string Label
        { get; set; }

        [Required(ErrorMessage = "手机号不能为空")]
        public string PhoneNumber
        { get; set; }

        [Required(ErrorMessage = "省份不能为空")]
        public string Province
        { get; set; }

        [Required(ErrorMessage = "城市不能为空")]
        public string City
        { get; set; }

        [Required(ErrorMessage = "区县不能为空")]
        public string County
        { get; set; }

        [Required(ErrorMessage = "详细地址不能为空")]
        public string DetailsAddress
        { get; set; }

        public int IsDefault
        { get; set; }

    }
}
