namespace AsYouLikeIt.Sdk.Common.Api
{
    using System.Text.Json.Serialization;

    public class Jwt
    {
        [JsonPropertyName("access_token")]
        [JsonPropertyOrder(1)]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        [JsonPropertyOrder(2)]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        [JsonPropertyOrder(3)]
        public long ExpiresIn { get; set; }
    }
}
