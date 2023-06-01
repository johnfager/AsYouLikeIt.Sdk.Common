
namespace AsYouLikeIt.Sdk.Common.Models
{
    using Newtonsoft.Json;

    public class ErrorDetail
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FieldName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? ErrorLine { get; set; }

        public ErrorDetail()
        {
        }

        public ErrorDetail(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public ErrorDetail(string fieldName, string error)
        {
            this.FieldName = fieldName;
            this.Error = error;
        }

        public ErrorDetail(string fieldName, string error, long? errorLine)
        {
            this.FieldName = fieldName;
            this.Error = error;
            this.ErrorLine = errorLine;
        }

    }
}
