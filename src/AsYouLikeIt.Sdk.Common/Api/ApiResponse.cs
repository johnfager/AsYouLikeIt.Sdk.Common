
namespace AsYouLikeIt.Sdk.Common.Api
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using AsYouLikeIt.Sdk.Common.Models;

    /// <summary>
    /// An API response message with information about the operation performed.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Whether or not the call succeeded.
        /// </summary>
        [JsonPropertyName("succeeded")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(1)]
        public bool Succeeded { get; set; }

        /// <summary>
        /// The message regarding the call, if any.
        /// </summary>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(2)]
        public string Message { get; set; }

        /// <summary>
        /// If there was an error, the error's code.
        /// </summary>
        [JsonPropertyName("error_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(4)]
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Populated if there was an error with the error's sub status code.
        /// </summary>
        [JsonPropertyName("error_sub_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(5)]
        public int? ErrorSubCode { get; set; }

        /// <summary>
        /// The type of object that the RequestReferenceKey refers to.
        /// </summary>
        [JsonPropertyName("object_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(6)]
        public virtual string ObjectType { get; set; }

        /// <summary>
        /// A reference value that contains the identifier or key for the ObjectType.
        /// </summary>
        [JsonPropertyName("return_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(7)]
        public string ReturnKey { get; set; }

        /// <summary>
        /// A redirect URL that can be followed after a request when appropriate.
        /// </summary>
        [JsonPropertyName("redirect_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(8)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// A link that provides further guidance on the response.
        /// </summary>
        [JsonPropertyName("help_link")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(9)]
        public string HelpLink { get; set; }

        /// <summary>
        /// An array of errors that may have been triggered by the request.
        /// </summary>
        [JsonPropertyName("errors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(11)]
        public List<ErrorDetail> Errors { get; set; }

        /// <summary>
        /// If multiple operations were involved, the responses to each operation are included here.
        /// </summary>
        [JsonPropertyName("sub_responses")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(12)]
        public List<ApiResponse> SubResponses { get; set; }

        /// <summary>
        /// Returns a JSON serialized ApiResponse model.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
    }

    /// <summary>
    /// An API response message with information about the operation performed that includes a property for associated data.
    /// This should be used as the response wrapper for all API calls.
    /// </summary>
    public class ApiResponse<T> : ApiResponse
    {
        [JsonPropertyName("count")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(13)]
        public int? Count { get; set; }

        /// <summary>
        /// Set to true if more data exists and a return trip is need to continue fetching data.
        /// </summary>
        [JsonPropertyName("more_data_exists")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(14)]
        public bool MoreDataExists { get; set; }

        /// <summary>
        /// The number of records on in the complete data set.
        /// </summary>
        [JsonPropertyName("data_batch_count")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(15)]
        public long? DataBatchCount { get; set; }

        /// <summary>
        /// The zero-based index of the batch requested.
        /// </summary>
        [JsonPropertyName("data_batch_index")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(16)]
        public int? DataBatchIndex { get; set; }

        /// <summary>
        /// The data associated with the ApiResponse
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(20)]
        public T Data { get; set; }
    }
}
