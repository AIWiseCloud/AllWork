using Newtonsoft.Json;

namespace AllWork.Web.Auth
{
    public class TokenManagement
    {

        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// 发行人
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅人
        /// </summary>
        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; }
    }
}
