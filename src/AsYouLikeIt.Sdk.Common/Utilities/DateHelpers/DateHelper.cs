using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    internal static class DateHelper
    {
        private static void ValidateDateArrayCounts(DateTime[] start, DateTime[] end, bool completedTermsOnly)
        {
            if (start == null || end == null)
            {
                throw new ArgumentNullException("Start and end date arrays cannot be null.");
            }

            if (!completedTermsOnly && start.Length != end.Length)
            {
                throw new InvalidOperationException("The number of start and end values must match when completedTermsOnly is false.");
            }
            // verifify that the range is equal or end + 1 equal to start
            if (completedTermsOnly &&
                !(start.Length == end.Length || start.Length == end.Length + 1))
            {
                throw new InvalidOperationException("The number of start and end values must match or end can have one less value if completedTermsOnly is true.");
            }
        }

        public static List<IDateRange> GetValidatedDateRanges(DateTime[] start, DateTime[] end, bool completedTermsOnly)
        {
            // ensure not out of bounds
            ValidateDateArrayCounts(start, end, completedTermsOnly);

            var ranges = new List<IDateRange>();

            if(end.Length == 0)
            {
                // if there are no end dates, we cannot create any ranges
                return ranges;
            }

            var currentStart = start[0];
            var currentEnd = end[0];

            // We don't always take the last start date to make a range, so use the ends
            for (int i = 0; i < end.Length; i++)
            {
                var startOfTerm = start[i];
                if(startOfTerm < currentStart)
                {
                    throw new ArgumentException($"Start dates missorted. Start date {startOfTerm} cannot be earlier than the current start date {currentStart}.");
                }
                var endOfTerm = end[i];
                if (endOfTerm < currentEnd)
                {
                    throw new ArgumentException($"End dates missorted. End date {endOfTerm} cannot be earlier than the current end date {currentEnd}.");
                }

                // date range already validates the start and end dates
                ranges.Add(new DateRange(startOfTerm, endOfTerm));

                // set the outer DateTime holder to ensure the next range is valid compared to the last one
                currentStart = startOfTerm;
                currentEnd = endOfTerm;
            }

            return ranges;
        }
    }
}