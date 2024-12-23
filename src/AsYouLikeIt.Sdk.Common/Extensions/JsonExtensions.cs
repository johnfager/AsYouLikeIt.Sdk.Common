namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    public static class JsonExtensions
    {

        public static IEnumerable<JsonNode> WalkTokens(this JsonNode node)
        {
            if (node == null)
                yield break;
            yield return node;
            if (node is JsonObject jsonObject)
            {
                foreach (var child in jsonObject)
                {
                    foreach (var childNode in child.Value.WalkTokens())
                    {
                        yield return childNode;
                    }
                }
            }
            else if (node is JsonArray jsonArray)
            {
                foreach (var child in jsonArray)
                {
                    foreach (var childNode in child.WalkTokens())
                    {
                        yield return childNode;
                    }
                }
            }
        }
    }
}
