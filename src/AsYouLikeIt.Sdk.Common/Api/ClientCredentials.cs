using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AsYouLikeIt.Sdk.Common.Api
{
    public class ClientCredentials
    {
        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(1)]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(2)]
        public string ClientSecret { get; set; }
    }
}
