using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{

    /// <summary>
    /// Base provider to keep all term providers consistent for functionality and testing. 
    /// Shares common logic and avoids code duplication.
    /// </summary>
    public abstract class DatePeriodProviderBase : IDatePeriodProvider
    {

        private readonly DatePeriodType _datePeriodType;

        public DatePeriodProviderBase(DatePeriodType datePeriodType)
        {
            _datePeriodType = datePeriodType;
        }

        #region needed implementations per provider

        public abstract DateTime GetStartOfCurrent(DateTime date);

        public abstract DateTime Increment(DateTime date, int units);

        /// <summary>
        /// Keeps providers from messing up basing on the start of the current term.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        protected abstract DateTime SetToEndTerm(DateTime date);

        #endregion

        #region shared functionality based on the implementations

        public DatePeriodType DatePeriodType => _datePeriodType;

        public DateTime GetEndOfCurrent(DateTime date)
        {
            // force everything flowing from start of current
            var startOfCurrent = GetStartOfCurrent(date);
            return SetToEndTerm(startOfCurrent);
        }

        public DateTime GetStartOfNext(DateTime date) => Increment(GetStartOfCurrent(date), 1);

        // NOTE: // for the end, must increment first and then get the end of current term (i.e. one month's end might be the 28th or 30th, so incrementing after getting the end fails in some circumstances)
        public DateTime GetEndOfPrevious(DateTime date) => GetEndOfCurrent(Increment(date, -1)); 

        public HashSet<DateTime> GetStartingDates(DateTime startDate, DateTime endDate, bool trimIncompleteStartingTerms)
        {
            var dates = new HashSet<DateTime>();

            // Set the point to work with at the beggining of the current term
            var nextDate = GetStartOfCurrent(startDate);

            // note it's possible that the start date is the start of the current term
            // if trimIncompleteStartingTerms is true, and the nextDate is before the startDate, we need to move to the next term
            if (trimIncompleteStartingTerms && nextDate < startDate)
            {
                nextDate = GetStartOfNext(nextDate);
            }

            while (nextDate <= endDate)
            {
                dates.Add(nextDate);
                nextDate = GetStartOfNext(nextDate);
            }
            return dates;
        }

        public HashSet<DateTime> GetEndingDates(DateTime startDate, DateTime endDate, bool trimIncompleteEndingTerms)
        {
            var nextDate = GetEndOfCurrent(startDate);

            // if trimIncompleteEndingTerms is true, we need to find the last completed week. otherwise, we can use the endDate directly.
            var lastDate = trimIncompleteEndingTerms ?
                GetEndOfPrevious(endDate) :
                GetEndOfCurrent(endDate);

            var dates = new HashSet<DateTime>();

            while (nextDate <= lastDate)
            {
                dates.Add(nextDate);

                // move to the next 
                nextDate = GetEndOfCurrent(GetStartOfNext(nextDate));
            }
            return dates;
        }

        /// <summary>
        /// Gets a range of terms based on the start and end dates provided.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="trimIncompleteStartingTerms">If true, the term given will be after the <paramref name="startDate"/.></param>
        /// <param name="trimIncompleteEndingTerms">If true, the terms will end on the last completed term before the <paramref name="endDate"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<IDateRange> GetTermRanges(DateTime startDate, DateTime endDate, bool trimIncompleteStartingTerms, bool trimIncompleteEndingTerms)
        {
            var ranges = new List<IDateRange>();
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
            }

            var starts = GetStartingDates(startDate, endDate, trimIncompleteStartingTerms).ToArray();
            var ends = GetEndingDates(startDate, endDate, trimIncompleteEndingTerms).ToArray();

            var validatedRanges = GetValidatedDateRanges(starts, ends, trimIncompleteStartingTerms, trimIncompleteEndingTerms);
            return validatedRanges;
        }

        #endregion

        #region helpers

        private void ValidateDateArrayCounts(DateTime[] start, DateTime[] end, bool trimIncompleteStartingTerms, bool trimIncompleteEndingTerms)
        {
            if (start == null || end == null)
            {
                throw new ArgumentNullException("Start and end date arrays cannot be null.");
            }

            // either could be lesser in max length based on the flags

            if (!trimIncompleteStartingTerms && !trimIncompleteEndingTerms && start.Length != end.Length)
            {
                throw new InvalidOperationException("The number of start and end values must match when trimIncompleteStartingTerms and trimIncompleteEndingTerms are false.");
            }

            // verifify that the range is equal or end + 1 equal to start
            if (!trimIncompleteStartingTerms && trimIncompleteEndingTerms &&
                !(start.Length == end.Length || start.Length == end.Length + 1))
            {
                throw new InvalidOperationException("The number of start and end values must match or end can have one less value if trimIncompleteStartingTerms is false and trimIncompleteEndingTerms is true.");
            }
            else if (trimIncompleteStartingTerms && !trimIncompleteEndingTerms &&
                !(start.Length == end.Length || start.Length + 1 == end.Length))
            {
                throw new InvalidOperationException("The number of start and end values must match or start can have one less value if trimIncompleteStartingTerms is false and trimIncompleteEndingTerms is true.");
            }
            else if (trimIncompleteStartingTerms && trimIncompleteEndingTerms &&
                !(start.Length == end.Length || start.Length + 1 == end.Length || start.Length == end.Length + 1))
            {
                throw new InvalidOperationException("The number of start and end values must match or one can have one less value if both trimIncompleteStartingTerms and trimIncompleteEndingTerms are true.");
            }
        }

        protected List<IDateRange> GetValidatedDateRanges(DateTime[] start, DateTime[] end, bool trimIncompleteStartingTerms, bool trimIncompleteEndingTerms)
        {
            // ensure not out of bounds
            ValidateDateArrayCounts(start, end, trimIncompleteStartingTerms, trimIncompleteEndingTerms);

            var ranges = new List<IDateRange>();

            if (end.Length == 0 || start.Length == 0)
            {
                // if there aren't values in one, we cannot create any ranges
                return ranges;
            }

            var currentStart = start[0];

            // create an array of the end dates that start only after the first start
            var inRangeEnd = end.Where(e => e >= currentStart).ToArray();
            if (inRangeEnd.Length == 0)
            {
                // if there are no end dates in range, we cannot create any ranges
                return ranges;
            }

            var currentEnd = inRangeEnd[0];

            // We don't always take the last start date to make a range, so use the ends
            for (int i = 0; i < inRangeEnd.Length; i++)
            {
                var startOfTerm = start[i];
                if (startOfTerm < currentStart)
                {
                    throw new ArgumentException($"Start dates missorted. Start date {startOfTerm} cannot be earlier than the current start date {currentStart}.");
                }
                var endOfTerm = inRangeEnd[i];
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

        #endregion

    }
}
