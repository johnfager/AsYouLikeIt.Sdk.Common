
namespace Sdk.Common.Extensions
{
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class StreamExtensions
    {

        public static void Write(this FileStream writer, string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            writer.Write(bytes, 0, bytes.Count());
        }

    }
}
