
namespace Sdk.Common.Models
{
    using Sdk.Common.Utilities;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using Sdk.Common.Serialization;

    /// <summary>
    /// An API response message with information about the operation performed.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Whether or not the call succeeded.
        /// </summary>
        [JsonProperty(Order = 1)]
        public bool Succeeded { get; set; }

        /// <summary>
        /// The message regarding the call, if any.
        /// </summary>
        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// If there was an error, the error's code.
        /// </summary>
        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Populated if there was an error with the error's sub status code.
        /// </summary>
        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        public int? ErrorSubCode { get; set; }

        /// <summary>
        /// The type of object that the RequestReferenceKey refers to.
        /// </summary>
        [JsonProperty(Order = 5, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string ObjectType { get; set; }

        /// <summary>
        /// A reference to the request for the purposes of record keeping that should be populated only if appropriate.
        /// </summary>
        [JsonProperty(Order = 6, NullValueHandling = NullValueHandling.Ignore)]
        public string EventIdentifier { get; set; }

        /// <summary>
        /// A reference value that contains the identifer or key for the ObjectType.
        /// </summary>
        [JsonProperty(Order = 7, NullValueHandling = NullValueHandling.Ignore)]
        public string ReturnKey { get; set; }

        /// <summary>
        /// A redirect URL that can be followed after a request when appropriate.
        /// </summary>
        [JsonProperty(Order = 8, NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// A link that provides further guidance on the response.
        /// </summary>
        [JsonProperty(Order = 9, NullValueHandling = NullValueHandling.Ignore)]
        public string HelpLink { get; set; }

        /// <summary>
        /// An array of errors that may have been triggered by the request.
        /// </summary>
        [JsonProperty(Order = 10, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ErrorDetail> Errors { get; set; }

        /// <summary>
        /// If multiple operations were involved, the responses to each operation are included here.
        /// </summary>
        [JsonProperty(Order = 11, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ApiResponse> SubResponses { get; set; }

        /// <summary>
        /// Returns a JSON serialized ApiResponse model.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Serializer.SerializeToJson(this, true);
        }
    }

    /// <summary>
    /// An API response message with information about the operation performed that includes a property for associated data.
    /// This should be used as the response wrapper for all API calls.
    /// </summary>
    public class ApiResponse<T> : ApiResponse 
    {
        /// <summary>
        /// The data associated with the ApiResponse
        /// </summary>
        public T Data { get; set; }
    }
}
