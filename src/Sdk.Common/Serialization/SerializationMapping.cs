
namespace Sdk.Common.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class SerializationMapping
    {
        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public string AssemblyPattern { get; set; }

        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Ignore)]
        public string TypeNamePattern { get; set; }

        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public string AssemblyReplacement { get; set; }

        public bool AssemblyIsFullReplacement { get; set; }

        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
        public string TypeNameReplacement { get; set; }

        public bool TypeNameIsFullReplacement { get; set; }

    }
}
