
namespace Sdk.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public interface ICreatedModifiedRecord
    {
        DateTime? CreatedOnUtc { get; set; }

        string CreatedBy { get; set; }

        DateTime? ModifiedOnUtc { get; set; }

        string ModifiedBy { get; set; }
    }

    public partial class CreatedModifiedRecord : ICreatedModifiedRecord
    {

        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedOnUtc { get; set; }

        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ModifiedOnUtc { get; set; }

        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        public string ModifiedBy { get; set; }
    }
}
