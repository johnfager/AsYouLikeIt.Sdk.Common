namespace AsYouLikeIt.Sdk.Common.Extensions
{
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
    }
}
