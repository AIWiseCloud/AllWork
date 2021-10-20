using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class LoginRequestDTO
    {
        /// <summary>
        /// 登录账号（UnionId/用户账号/手机号)
        /// </summary>
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
