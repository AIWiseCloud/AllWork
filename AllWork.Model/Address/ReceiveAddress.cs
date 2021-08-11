using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Address
{
    public class ReceiveAddress
    {
        /// <summary>
        /// 主键（后台提供)
        /// </summary>
        public string AddrId
        { get; set; }

        [Required(ErrorMessage ="用户ID不能为空")]
        public string UnionId
        { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Required(ErrorMessage = "收货人姓名不能为空")]
        public string Receiver
        { get; set; }

        /// <summary>
        /// 地址标签
        /// </summary>
        public string Label
        { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号不能为空")]
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Required(ErrorMessage = "省份不能为空")]
        public string Province
        { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Required(ErrorMessage = "城市不能为空")]
        public string City
        { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        [Required(ErrorMessage = "区县不能为空")]
        public string County
        { get; set; }

        /// <summary>
        /// 详细址
        /// </summary>
        [Required(ErrorMessage = "详细地址不能为空")]
        public string DetailsAddress
        { get; set; }

        /// <summary>
        /// 是否默认址
        /// </summary>
        public int IsDefault
        { get; set; }

    }
}
