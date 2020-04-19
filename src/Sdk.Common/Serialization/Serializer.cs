
namespace Sdk.Common.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class Serializer
    {

        private static JsonSerializerSettings _serializerSettings;

        public static JsonSerializerSettings SerializerSettings
        {
            get
            {
                if (_serializerSettings == null)
                {
                    _serializerSettings = GetDefaultSerializerSettings();
                }
                return _serializerSettings;
            }
            set
            {
                _serializerSettings = value;
            }
        }

        public static JsonSerializerSettings GetDefaultSerializerSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
            // Look at exact serialization settings
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["jsonMissingMemberError"]) && ConfigurationManager.AppSettings["jsonMissingMemberError"].ToLower() == "true")
            {
                settings.MissingMemberHandling = MissingMemberHandling.Error;
            }
            return settings;
        }

        public static string SerializeToJson(object obj)
        {
            return SerializeToJson(obj, false);
        }

        public static string SerializeToJson(object obj, bool indented)
        {
            if (indented)
            {
                return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, SerializerSettings);
            }
            else
            {
                return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, SerializerSettings);
            }
        }

        //public static string SerializeToJson(object obj, bool indented, ReferenceLoopHandling referenceLoopHandling)
        //{
        //    SerializerSettings.ReferenceLoopHandling = referenceLoopHandling;
        //    return JsonConvert.SerializeObject(obj, SerializerSettings);
        //}

        public static string SerializeToJson(object obj, JsonSerializerSettings jsonSerializerSettings)
        {
            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }
        
        public static string SerializeToJson(object obj, bool indented, JsonSerializerSettings jsonSerializerSettings)
        {
            if (indented)
            {
                return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, jsonSerializerSettings);
            }
            else
            {
                return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, jsonSerializerSettings);
            }   
        }

        public static string SerializeJsonToXml(string json, string rootNode)
        {
            return SerializeJsonToXml(json, rootNode, false);
        }

        public static string SerializeJsonToXml(string json, string rootNode, bool indented)
        {
            var doc = JsonConvert.DeserializeXmlNode(json, rootNode);
            using (var stringWriter = new StringWriter())
            {
                var xmlSettings = new XmlWriterSettings() { Indent = indented };
                using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlSettings))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    return stringWriter.GetStringBuilder().ToString();
                }
            }
        }

        public static object DeserializeFromJson(string jsonText, Type type)
        {
            return JsonConvert.DeserializeObject(jsonText, type);
        }

        public static object DeserializeFromJson(string jsonText, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(jsonText, type, settings);
        }

        public static T DeserializeFromJson<T>(string jsonText) where T : class
        {
            var type = typeof(T);
            return (T)DeserializeFromJson(jsonText, type);
            // return DeserializeFromJson<T>(jsonText, false);
        }


        public static T DeserializeFromJson<T>(string jsonText, JsonSerializerSettings settings) where T : class
        {
            var type = typeof(T);
            return (T)DeserializeFromJson(jsonText, type, settings);
            // return DeserializeFromJson<T>(jsonText, false);
        }


        //---------------------

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
