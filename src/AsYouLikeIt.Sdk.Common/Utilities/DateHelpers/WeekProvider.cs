using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{

    public static class WeekHelper
    {
        private static WeekProvider _weekProvider = new WeekProvider();

        public static WeekProvider Provider => _weekProvider;
    }

    public class WeekProvider
    {
        public DateTime GetStartOfWeek(DateTime date)
        {
            // Get the first day of the week (Sunday) for the given date
            return date.Date.AddDays(-(int)date.DayOfWeek);
        }

        public HashSet<DateTime> GetStartOfWeekDates(DateTime startDate, DateTime endDate)
        {
            var beginningDate = startDate.Date;
            var lastDate = endDate.Date;

            // -----------------------------------------------------------------------------
            //  use only the beginningDate and lastDate to create a set of dates from here
            // -----------------------------------------------------------------------------

            var dates = new HashSet<DateTime>();

            // Set the point to work with at the beggining since all weeks are 7 day increments and logic is only needed once to set our place
            var nextDate = GetStartOfWeek(beginningDate);
            while (nextDate <= lastDate)
            {
                // extra check to ensure no typos sneak into the logic
                if (nextDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    throw new InvalidOperationException($"The date '{nextDate.DayOfWeek}, {nextDate}' is not valid.");
                }
                dates.Add(nextDate);

                // move to the next week
                nextDate = nextDate.AddDays(7);
            }
            return dates;
        }

        public DateTime GetEndOfCurrentWeek(DateTime date)
        {
            return date.Date.AddDays(6 - (int)date.DayOfWeek);
        }

        public DateTime GetEndOfPreviousWeek(DateTime date)
        {
            return GetEndOfCurrentWeek(date).AddDays(-7);
        }

        /// <summary>
        /// Gets the last day of the week for each week in the range with the option to include only fully completed weeks.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="completedTermsOnly">If a week is not yet completed in the range, it will be omitted.</param>
        /// <returns></returns>
        public HashSet<DateTime> GetEndOfWeekDates(DateTime startDate, DateTime endDate, bool completedTermsOnly)
        {
            var beginningDate = GetEndOfCurrentWeek(startDate);

            // if completedTermsOnly is true, we need to find the last completed week. otherwise, we can use the endDate directly.
            var lastDate = completedTermsOnly ?
                GetEndOfPreviousWeek(endDate) :
                GetEndOfCurrentWeek(endDate);

            // -----------------------------------------------------------------------------
            //  use only the beginningDate and lastDate to create a set of dates from here
            // -----------------------------------------------------------------------------

            var dates = new HashSet<DateTime>();
            var nextDate = beginningDate;

            while (nextDate <= lastDate)
            {
                // extra check to ensure no typos sneak into the logic
                if (nextDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    throw new InvalidOperationException($"The date '{nextDate.DayOfWeek}, {nextDate}' is not valid.");
                }
                dates.Add(nextDate);

                // move to the next week
                nextDate = nextDate.AddDays(7);
            }
            return dates;
        }

        public List<IDateRange> GetWeeklyRanges(DateTime startDate, DateTime endDate, bool completedTermsOnly, bool includeOnlyCompleteInitialTerms)
        {
            var ranges = new List<IDateRange>();
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
            }

            var startOfWeekDates = GetStartOfWeekDates(startDate, endDate).ToArray();
            var endOfWeekDates = GetEndOfWeekDates(startDate, endDate, completedTermsOnly).ToArray();

            var validatedRanges = DateHelper.GetValidatedDateRanges(startOfWeekDates, endOfWeekDates, completedTermsOnly);
            return validatedRanges;
        }
    }
}
