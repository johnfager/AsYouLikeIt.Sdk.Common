
namespace AsYouLikeIt.Sdk.Common.Serialization
{
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using Utilities;

    public class TolerantSerializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        private static object _lock;

        private static bool _assemblyMapLoaded;

        private static List<SerializationMapping> _serializationMapping;

        private static List<SerializationMapping> SerializationMappings
        {
            get
            {
                if (!_assemblyMapLoaded)
                {
                    LoadSerializationMapping();
                }
                return _serializationMapping;
            }
        }

        private static string _assemblyDirectoryPath;

        public static string AssemblyDirectory
        {
            get
            {
                if (_assemblyDirectoryPath == null)
                {
                    lock (_lock)
                    {
                        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                        UriBuilder uri = new UriBuilder(codeBase);
                        string path = Uri.UnescapeDataString(uri.Path);
                        _assemblyDirectoryPath = Path.GetDirectoryName(path);
                    }
                }
                return _assemblyDirectoryPath;
            }
        }

        private static string MapPath(string virtualPath)
        {
            var rootDir = Globals.ResourcesRootDirectory;
            if (string.IsNullOrEmpty(rootDir))
            {
                rootDir = AssemblyDirectory;
                if (virtualPath.StartsWith("~/"))
                {
                    var di = new DirectoryInfo(rootDir);
                    if (di.Parent != null)
                    {
                        rootDir = di.Parent.FullName;
                    }
                }
            }
            if (!Directory.Exists(rootDir))
            {
                throw new InvalidOperationException("appSettings must contain a valid directory path for the 'resourcesRoot' value. Root directory path is set as:\r\n" + rootDir);
            }
            if (string.IsNullOrWhiteSpace(virtualPath))
            {
                return rootDir;
            }
            if (virtualPath.StartsWith("~/"))
            {
                virtualPath = virtualPath.Substring(1);
            }
            virtualPath = virtualPath.SwitchForwardSlashToBackSlash();
            var path = Format.PathMerge(rootDir.StripTrailingBackslashesAll(), virtualPath.StripLeadingSlash().StripTrailingSlashesAll());
            return path;
        }


        private static void LoadSerializationMapping()
        {
            if (_lock == null)
            {
                _lock = new object();
            }
            lock (_lock)
            {
                var filePath = Globals.SerializationMappingFilePath;
                if (!string.IsNullOrEmpty(filePath))
                {
                    if (filePath.StartsWith("~/"))
                    {
                        filePath = MapPath(filePath);
                    }
                    var fi = new FileInfo(filePath);
                    if (!fi.Exists)
                    {
                        throw new FileNotFoundException(string.Format("File '{0}' could not be found and is required due to appSettings key serializationMappingFilePath.", fi.FullName));
                    }
                    var json = File.ReadAllText(fi.FullName);
                    try
                    {
                        _serializationMapping = Serializer.DeserializeFromJson<List<SerializationMapping>>(json);
                    }
                    catch (Exception ex)
                    {
                        throw new FormatException(string.Format("File '{0}' could not be deserialized into a List<SerializationMapping>. Values were:\n" + json, fi.FullName), ex);
                    }
                    foreach (var mapping in _serializationMapping)
                    {
                        if (string.IsNullOrEmpty(mapping.AssemblyPattern))
                        {
                            throw new FormatException(string.Format("File '{0}' is missing required 'assemblyPattern' value in one or more entries. Values were:\n" + json, fi.FullName));
                        }
                        if (string.IsNullOrEmpty(mapping.AssemblyPattern))
                        {
                            throw new FormatException(string.Format("File '{0}' is missing required 'typePattern' value in one or more entries. Values were:\n" + json, fi.FullName));
                        }
                        if (string.IsNullOrEmpty(mapping.AssemblyReplacement + mapping.TypeNameReplacement))
                        {
                            throw new FormatException(string.Format("File '{0}' is missing required 'AssemblyReplacement' or 'TypeNameReplacement' values in one or more entries. Values were:\n" + json, fi.FullName));
                        }
                    }
                }
                _assemblyMapLoaded = true;
            }
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (SerializationMappings != null)
            {
                foreach (var mapping in SerializationMappings)
                {
                    // TODO: Loop and parse RegEx matches
                    if (Regex.IsMatch(assemblyName, mapping.AssemblyPattern) && Regex.IsMatch(typeName, mapping.TypeNamePattern))
                    {
                        if (!string.IsNullOrEmpty(mapping.AssemblyReplacement))
                        {
                            if (Regex.IsMatch(assemblyName, mapping.AssemblyPattern) && mapping.AssemblyIsFullReplacement)
                            {
                                assemblyName = mapping.AssemblyReplacement;
                            }
                            else
                            {
                                assemblyName = Regex.Replace(assemblyName, mapping.AssemblyPattern, mapping.AssemblyReplacement);
                            }
                        }
                        if (string.IsNullOrEmpty(mapping.TypeNameReplacement))
                        {
                            typeName = Regex.Replace(typeName, mapping.AssemblyPattern, mapping.AssemblyReplacement);
                        }
                        else if (mapping.TypeNameIsFullReplacement)
                        {
                            typeName = mapping.TypeNameReplacement;
                        }
                        else
                        {
                            typeName = Regex.Replace(typeName, mapping.TypeNamePattern, mapping.TypeNameReplacement);
                        }
                    }
                }
            }

            // Get the type using the typeName and assemblyName
            Type typeToDeserialize = Type.GetType($"{typeName}, {assemblyName}");

            return typeToDeserialize ?? throw new SerializationException($"TolerantSerialiazationBinder: Unable to find assembly '{assemblyName}' with type '{typeName}'.");
        }
    }
}
