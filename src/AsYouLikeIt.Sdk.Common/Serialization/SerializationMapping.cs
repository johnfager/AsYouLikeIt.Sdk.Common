
namespace AsYouLikeIt.Sdk.Common.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public class SerializationMapping
    {
        [JsonPropertyName("assemblyPattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AssemblyPattern { get; set; }

        [JsonPropertyName("typeNamePattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TypeNamePattern { get; set; }

        [JsonPropertyName("assemblyReplacement")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AssemblyReplacement { get; set; }

        public bool AssemblyIsFullReplacement { get; set; }

        [JsonPropertyName("typeNameReplacement")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TypeNameReplacement { get; set; }

        public bool TypeNameIsFullReplacement { get; set; }
    }
}
