namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Defaults to a lowercase HEX string
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] helper)
        {
            return ToHexString(helper, true);
        }

        public static string ToHexString(this byte[] helper, bool lowerCase)
        {
            var hexCode = "x2";
            if (!lowerCase)
            {
                hexCode = "X2";
            }
            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < helper.Length; i++)
            {
                sBuilder.Append(helper[i].ToString(hexCode));
            }
            return sBuilder.ToString();
        }

        public static byte[] Concatinate(this byte[] helper, byte[] add)
        {
            IEnumerable<byte> rv = helper.Concat(add);
            return rv.ToArray();
        }

        public static byte[] GetBytes(this string hexString)
        {
            byte[] bytes = new byte[hexString.Length * sizeof(char)];
            System.Buffer.BlockCopy(hexString.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
