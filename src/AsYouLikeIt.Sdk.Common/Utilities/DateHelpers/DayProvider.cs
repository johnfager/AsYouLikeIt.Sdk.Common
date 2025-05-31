using System;
using System.Collections.Generic;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public class DayProvider : DatePeriodProviderBase
    {
        public override DateTime GetStartOfCurrent(DateTime date) => date.Date;

        public override DateTime GetStartOfNext(DateTime date) => GetStartOfCurrent(date).AddDays(1);

        /// <summary>
        /// Gets the end of the current day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public override DateTime GetEndOfCurrent(DateTime date) => date.Date.AddDays(1).AddTicks(-1);

        public override DateTime GetEndOfPrevious(DateTime date) => GetEndOfCurrent(date).AddDays(-1);

        public override HashSet<DateTime> GetStartingDates(DateTime startDate, DateTime endDate)
        {
            var beginningDate = startDate.Date;
            var lastDate = endDate.Date;

            // -----------------------------------------------------------------------------
            //  use only the beginningDate and lastDate to create a set of dates from here
            // -----------------------------------------------------------------------------

            var dates = new HashSet<DateTime>();
            DateTime currentDate = beginningDate.Date;
            while (currentDate <= lastDate)
            {
                // get the last day of the month of the currentDate
                dates.Add(currentDate);
                currentDate = currentDate.Date.AddDays(1);
            }
            return dates;
        }

        public override HashSet<DateTime> GetEndingDates(DateTime startDate, DateTime endDate, bool completedTermsOnly)
        {
            var beginningDate = GetEndOfCurrent(startDate);

            // if completedTermsOnly is true, we need to find the last completed week. otherwise, we can use the endDate directly.
            var lastDate = completedTermsOnly ?
                GetEndOfPrevious(endDate) :
                GetEndOfCurrent(endDate);

            // -----------------------------------------------------------------------------
            //  use only the beginningDate and lastDate to create a set of dates from here
            // -----------------------------------------------------------------------------

            var dates = new HashSet<DateTime>();
            DateTime currentDate = beginningDate;
            while (currentDate <= lastDate)
            {
                // get the last day of the month of the currentDate
                dates.Add(currentDate);
                currentDate = currentDate.Date.AddDays(1);
            }
            return dates;
        }
    }
}
