
namespace AsYouLikeIt.Sdk.Common.Models
{
    using Newtonsoft.Json;
    using AsYouLikeIt.Sdk.Common.Serialization;

    public partial class ApiError
    {
        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorCode { get; set; }

        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public string Endpoint { get; set; }

        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }

        [JsonProperty(Order = 5, NullValueHandling = NullValueHandling.Ignore)]
        public string PostBody { get; set; }

        [JsonProperty(Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        public ApiResponse ApiResponse { get; set; }

        /// <summary>
        /// Returns a JSON serialized ApiError model.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // We serialize the body to JSON and return it
            // For these exceptions, stack trace is not so relevant... more the request
            return Serializer.SerializeToJson(this, true);
        }
    }
}
