using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AllWork.Model.RequestParams
{
    /// <summary>
    /// 修改订单收货地址的请求参数
    /// </summary>
    public class UpdateOrderAddressParams
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空")]
        public string OrderId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [Required(ErrorMessage = "收货人不能为空")]
        public string Receiver
        { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        [Required(ErrorMessage = "收货人电话不能为空")]
        public string PhoneNumber
        { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [Required(ErrorMessage = "必须提供详细收货地址")]
        public string DeliveryAddress
        { get; set; }
    }
}
