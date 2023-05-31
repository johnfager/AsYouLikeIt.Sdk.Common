namespace Sdk.Common.Extensions
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class AggregateExceptionExtensions
    {


        public static Exception ExtractSingleIfAggregate(this Exception exception)
        {
            if(!(exception is AggregateException aggEx))
            {
                return exception;
            }

            Exception ex = exception;

            if (aggEx.InnerExceptions != null && aggEx.InnerExceptions.Any())
            {
                Exception nextEx = aggEx.InnerExceptions.First();
                while (true)
                {
                    if (nextEx is AggregateException thisAgg)
                    {
                        if (thisAgg.InnerExceptions != null && thisAgg.InnerExceptions.Any())
                        {
                            nextEx = thisAgg.InnerExceptions.First();
                            continue;
                        }
                        else
                        {
                            ex = nextEx;
                            break;
                        }
                    }
                    else
                    {
                        ex = nextEx;
                        break;
                    }
                }

            }
            return ex;
        }

        public static IEnumerable<Exception> ExtractIfAggregate(this AggregateException ex)
        {
            var list = new List<Exception>();
            ExtractRecursive(ex, list);
            return list;
        }

        private static void ExtractRecursive(this AggregateException ex, List<Exception> list)
        {
            if (ex.InnerExceptions != null)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    if (innerEx is AggregateException aggregateException)
                    {
                        ExtractRecursive(aggregateException, list);
                    }
                    else
                    {
                        list.Add(innerEx);
                    }
                }
            }
        }
    }
}
