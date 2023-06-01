namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

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

        public static IQueryable<TSource> WhereLike<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, string>> valueSelector,
            string value,
            char wildcard)
        {
            return source.Where(BuildLikeExpression(valueSelector, value, wildcard));
        }

        public static Expression<Func<TElement, bool>> BuildLikeExpression<TElement>(
            Expression<Func<TElement, string>> valueSelector,
            string value,
            char wildcard)
        {
            if (valueSelector == null)
                throw new ArgumentNullException("valueSelector");

            var method = GetLikeMethod(value, wildcard);

            value = value.Trim(wildcard);
            var body = Expression.Call(valueSelector.Body, method, Expression.Constant(value));

            var parameter = valueSelector.Parameters.Single();
            return Expression.Lambda<Func<TElement, bool>>(body, parameter);
        }

        private static MethodInfo GetLikeMethod(string value, char wildcard)
        {
            var methodName = "Contains";

            var textLength = value.Length;
            value = value.TrimEnd(wildcard);
            if (textLength > value.Length)
            {
                methodName = "StartsWith";
                textLength = value.Length;
            }

            value = value.TrimStart(wildcard);
            if (textLength > value.Length)
            {
                methodName = (methodName == "StartsWith") ? "Contains" : "EndsWith";
                textLength = value.Length;
            }

            var stringType = typeof(string);
            return stringType.GetMethod(methodName, new Type[] { stringType });
        }
    }
}
