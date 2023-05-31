
namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    public static class JsonExtensions
    {
        /*
            var CWFile = JToken.Parse(JSON)
            var arrayProperties = CWFile.WalkTokens().OfType<JProperty>().Where(prop => prop.Value.Type == JTokenType.Array);
        */

        public static IEnumerable<JToken> WalkTokens(this JToken node)
        {
            if (node == null)
                yield break;
            yield return node;
            foreach (var child in node.Children())
                foreach (var childNode in child.WalkTokens())
                    yield return childNode;
        }
    }

}
