
namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// DictionaryExtensions Class.
    /// </summary>
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Creates a new Dictionary based on source, without the values from the second.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Except<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (second == null || first == null) return new Dictionary<TKey, TValue>(0);

            return first.Where(x => !second.ContainsKey(x.Key)).ToDictionary(n => n.Key, n => n.Value);
        }

        /// <summary>
        /// If a key exists in a dictionary, return its value, otherwise return the default value
        /// for that type.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetWithDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.GetWithDefault(key, default(TValue));
        }

        /// <summary>
        /// If a key exists in a dictionary, return its value, otherwise return the provided default value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetWithDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            if (source.TryGetValue(key, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Creates a new Dictionary where both dictionaries intersect.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> In<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (second == null || first == null) return new Dictionary<TKey, TValue>(0);

            return first.Where(x => second.ContainsKey(x.Key)).ToDictionary(n => n.Key, n => n.Value);
        }

        /// <summary>
        /// Merge Dictionaries.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (second == null || first == null) return;

            foreach (var item in second)
            {
                if (!first.ContainsKey(item.Key))
                {
                    first.Add(item.Key, item.Value);
                }
            }
        }
    }
}
