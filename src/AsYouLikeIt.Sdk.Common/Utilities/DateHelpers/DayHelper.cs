using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public class DayHelper
    {
        public static HashSet<DateTime> GetDailyDates(DateTime startDate, DateTime endDate)
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

        public static IDateRange GetDailyDateRange(DateTime startDate, DateTime endDate)
            => new DateRange(startDate.Date, endDate.Date);
    }
}
