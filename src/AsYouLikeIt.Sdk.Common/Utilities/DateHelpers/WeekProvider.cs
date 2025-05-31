using System;
using System.Collections.Generic;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{

    public static class Week
    {
        public static WeekProvider Provider { get; } = new WeekProvider();
    }

    public class WeekProvider : DatePeriodProviderBase
    {

        private DayOfWeek _startOfWeek = DayOfWeek.Sunday;

        public override DateTime GetStartOfCurrent(DateTime date) => date.Date.AddDays(-(int)date.DayOfWeek);

        public override DateTime GetStartOfNext(DateTime date) => GetStartOfCurrent(date).AddDays(7);

        public override DateTime GetEndOfCurrent(DateTime date) => GetStartOfCurrent(date).AddDays(6);

        public override DateTime GetEndOfPrevious(DateTime date) => GetEndOfCurrent(date).AddDays(-7);

        public override HashSet<DateTime> GetStartingDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();

            // Set the point to work with at the beggining since all weeks are 7 day increments and logic is only needed once to set our place
            var nextDate = GetStartOfCurrent(startDate);
            while (nextDate <= endDate)
            {
                dates.Add(nextDate);
                nextDate = GetStartOfNext(nextDate);
            }
            return dates;
        }

        public override HashSet<DateTime> GetEndingDates(DateTime startDate, DateTime endDate, bool completedTermsOnly)
        {
            var nextDate = GetEndOfCurrent(startDate);

            // if completedTermsOnly is true, we need to find the last completed week. otherwise, we can use the endDate directly.
            var lastDate = completedTermsOnly ?
                GetEndOfPrevious(endDate) :
                GetEndOfCurrent(endDate);
  
            var dates = new HashSet<DateTime>();

            while (nextDate <= lastDate)
            {
                // extra check to ensure no typos sneak into the logic
                if (nextDate.DayOfWeek != _startOfWeek - 1)
                {
                    throw new InvalidOperationException($"The date '{nextDate.DayOfWeek}, {nextDate}' is not valid.");
                }
                dates.Add(nextDate);

                // move to the next 
                nextDate = GetEndOfCurrent(GetStartOfNext(nextDate));
            }
            return dates;
        }
    }
}
