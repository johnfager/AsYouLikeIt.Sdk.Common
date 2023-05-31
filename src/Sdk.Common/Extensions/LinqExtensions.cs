namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        /// <summary>
        /// False if the <paramref name="source"/> is null. True if any items are in the IEnumerable.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AnyAndNotNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return false;
            }
            return source.Any();
        }
    }
}
