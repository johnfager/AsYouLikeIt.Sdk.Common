
namespace AsYouLikeIt.Sdk.Common.Serialization
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class Serializer
    {
        private static JsonSerializerOptions _serializerOptions;

        public static bool ErrorOnMissingMember { get; set; } = false;

        public static JsonSerializerOptions SerializerOptions
        {
            get
            {
                if (_serializerOptions == null)
                {
                    _serializerOptions = GetDefaultSerializerOptions();
                }
                return _serializerOptions;
            }
            set
            {
                _serializerOptions = value;
            }
        }

        public static JsonSerializerOptions GetDefaultSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new JsonStringEnumConverter() }
            };
            return options;
        }

        public static string SerializeToJson(object obj)
        {
            return SerializeToJson(obj, false);
        }

        public static string SerializeToJson(object obj, bool indented)
        {
            var options = SerializerOptions;
            options.WriteIndented = indented;
            return JsonSerializer.Serialize(obj, options);
        }

        public static string SerializeToJson(object obj, JsonSerializerOptions jsonSerializerOptions)
        {
            return JsonSerializer.Serialize(obj, jsonSerializerOptions);
        }

        public static string SerializeToJson(object obj, bool indented, JsonSerializerOptions jsonSerializerOptions)
        {
            jsonSerializerOptions.WriteIndented = indented;
            return JsonSerializer.Serialize(obj, jsonSerializerOptions);
        }

        public static string SerializeJsonToXml(string json, string rootNode)
        {
            return SerializeJsonToXml(json, rootNode, false);
        }

        public static string SerializeJsonToXml(string json, string rootNode, bool indented)
        {
            var doc = JsonDocument.Parse(json);
            using (var stringWriter = new StringWriter())
            {
                var xmlSettings = new XmlWriterSettings() { Indent = indented };
                using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlSettings))
                {
                    // Custom implementation to convert JSON to XML
                    // This part needs to be implemented as per your requirements
                    // doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    return stringWriter.GetStringBuilder().ToString();
                }
            }
        }

        public static object DeserializeFromJson(string jsonText, Type type)
        {
            return JsonSerializer.Deserialize(jsonText, type, SerializerOptions);
        }

        public static object DeserializeFromJson(string jsonText, Type type, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize(jsonText, type, options);
        }

        public static T DeserializeFromJson<T>(string jsonText) where T : class
        {
            return JsonSerializer.Deserialize<T>(jsonText, SerializerOptions);
        }

        public static T DeserializeFromJson<T>(string jsonText, JsonSerializerOptions options) where T : class
        {
            return JsonSerializer.Deserialize<T>(jsonText, options);
        }

        // Convert an object to a byte array
        public static byte[] SerializeToByteArray(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        public static Object DeserializeFromByteArray(byte[] arrBytes)
        {
            return DeserializeFromByteArray(arrBytes, new TolerantSerializationBinder());
        }

        public static Object DeserializeFromByteArray(byte[] arrBytes, SerializationBinder serializationBinder)
        {
            using (var memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                binForm.Binder = serializationBinder;
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
