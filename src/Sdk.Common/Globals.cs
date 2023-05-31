namespace Sdk.Common
{

    using Delegates;

    public class Globals
    {
        public static string ResourcesRootDirectory { get; set; }

        public static string SerializationMappingFilePath { get; set; }

        public static StringEncryptorDelegate StringEncryptorDelegate { get; set; }

        public static StringDecryptorDelegate StringDecryptorDelegate { get; set; }

    }
}
