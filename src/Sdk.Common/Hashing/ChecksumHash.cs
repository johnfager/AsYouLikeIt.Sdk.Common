
namespace Sdk.Common.Hashing
{
    using Sdk.Common.Serialization;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Text;

    public static class ChecksumHash
    {

        public static string GetMD5Hash(object instance)
        {
            return instance.GetHash<MD5CryptoServiceProvider>();
        }

        public static string GetSHA1Hash(object instance)
        {
            return instance.GetHash<SHA1CryptoServiceProvider>();
        }

        public static string GetSHA256Hash(object instance)
        {
            return instance.GetHash<SHA256CryptoServiceProvider>();
        }

        public static string GetKeyedMD5Hash(object instance, byte[] key)
        {
            return instance.GetKeyedHash<HMACMD5>(key);
        }

        public static string GetKeyedSHA1Hash(object instance, byte[] key)
        {
            return instance.GetKeyedHash<HMACSHA1>(key);
        }

        public static string GetKeyedSHA256Hash(object instance, byte[] key)
        {
            return instance.GetKeyedHash<HMACSHA256>(key);
        }

        #region helpers

        private static string GetHash<T>(this object instance) where T : HashAlgorithm, new()
        {
            var cryptoServiceProvider = new T();
            return ComputeHash(instance, cryptoServiceProvider);
        }

        private static string GetKeyedHash<T>(this object instance, byte[] key) where T : KeyedHashAlgorithm, new()
        {
            var cryptoServiceProvider = new T { Key = key };
            return ComputeHash(instance, cryptoServiceProvider);
        }

        private static string ComputeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            byte[] ser = null;
            object toSerialize = null;
            if (instance is string strInstance)
            {
                ser = Encoding.UTF8.GetBytes(strInstance);
            }
            else if (instance.GetType().IsSerializable)
            {
                toSerialize = instance;
            }
            else
            {
                try
                {
                    toSerialize = Serializer.SerializeToJson(instance);
                }
                catch (Exception)
                {
                    throw new SerializationException(string.Format("Object of type '{0}' is not marked as serializable. No hash could be created.  Simple objects can use JSON serialization. If JSON serialization cannot be performed and the object is not marked as serializable, the hash generation will fail.", instance.GetType().FullName));
                }
            }
            if (ser == null)
            {
                ser = Serializer.SerializeToByteArray(toSerialize);
            }
            var hash = cryptoServiceProvider.ComputeHash(ser);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        #endregion

    }
}
