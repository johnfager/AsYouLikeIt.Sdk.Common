namespace AsYouLikeIt.Sdk.Common.Models
{
    using System.Diagnostics;
    using System.Text.Json.Serialization;

    [DebuggerDisplay("{FieldName} - {Error}")]
    public class ErrorDetail
    {
        [JsonPropertyName("fieldName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FieldName { get; set; }

        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Error { get; set; }

        [JsonPropertyName("errorLine")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ErrorLine { get; set; }

        public ErrorDetail()
        {
        }

        public ErrorDetail(string fieldName)
        {
            FieldName = fieldName;
        }

        public ErrorDetail(string fieldName, string error)
        {
            FieldName = fieldName;
            Error = error;
        }

        public ErrorDetail(string fieldName, string error, long? errorLine)
        {
            FieldName = fieldName;
            Error = error;
            ErrorLine = errorLine;
        }
    }
}
