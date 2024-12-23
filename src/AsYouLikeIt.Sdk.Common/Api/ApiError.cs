namespace AsYouLikeIt.Sdk.Common.Api
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class ApiError
    {

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(2)]
        public string Message { get; set; }

        [JsonPropertyName("endpoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(3)]
        public string Endpoint { get; set; }

        [JsonPropertyName("method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(4)]
        public string Method { get; set; }

        [JsonPropertyName("post_body")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(5)]
        public string PostBody { get; set; }

        [JsonPropertyName("api_response")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(6)]
        public ApiResponse ApiResponse { get; set; }

        /// <summary>
        /// Returns a JSON serialized ApiError model.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // We serialize the body to JSON and return it
            // For these exceptions, stack trace is not so relevant... more the request
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
