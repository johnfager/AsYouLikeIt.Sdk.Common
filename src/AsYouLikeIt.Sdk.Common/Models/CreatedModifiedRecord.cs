
namespace AsYouLikeIt.Sdk.Common.Models
{
    using System;
    using System.Text.Json.Serialization;

    public interface ICreatedModifiedRecord
    {
        DateTime? CreatedOnUtc { get; set; }

        string CreatedBy { get; set; }

        DateTime? ModifiedOnUtc { get; set; }

        string ModifiedBy { get; set; }
    }

    public partial class CreatedModifiedRecord : ICreatedModifiedRecord
    {
        [JsonPropertyName("createdOnUtc")]
        [JsonPropertyOrder(1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? CreatedOnUtc { get; set; }

        [JsonPropertyName("createdBy")]
        [JsonPropertyOrder(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CreatedBy { get; set; }

        [JsonPropertyName("modifiedOnUtc")]
        [JsonPropertyOrder(3)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ModifiedOnUtc { get; set; }

        [JsonPropertyName("modifiedBy")]
        [JsonPropertyOrder(4)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ModifiedBy { get; set; }
    }
}
