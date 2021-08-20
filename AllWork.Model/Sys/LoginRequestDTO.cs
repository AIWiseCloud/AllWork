using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AllWork.Model.Sys
{
    public class LoginRequestDTO
    {
        /// <summary>
        /// 用户名(userName或者unionId)
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
