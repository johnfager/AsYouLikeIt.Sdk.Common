
namespace Sdk.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public partial class CreatedModifiedRecord
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
