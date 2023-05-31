
namespace Sdk.Common.Models
{
    using Newtonsoft.Json;

    public class Jwt
    {
        [JsonProperty(Order = 1, PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(Order = 2, PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(Order = 3, PropertyName = "expires_in")]
        public long ExpiresIn { get; set; }
    }
}
