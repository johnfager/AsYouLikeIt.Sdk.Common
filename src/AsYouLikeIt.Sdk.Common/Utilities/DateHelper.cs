using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsYouLikeIt.Sdk.Common.Utilities
{
    /// <summary>
    /// Works with Date aspect, removing the time aspect.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Works with the date aspect, removing the time.
        /// </summary>
        public static class Days
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

        public static class Weeks
        {

            public static DateTime GetStartOfWeek(DateTime date)
            {
                // Get the first day of the week (Sunday) for the given date
                return date.Date.AddDays(-(int)date.DayOfWeek);
            }

            public static HashSet<DateTime> GetStartOfWeekDates(DateTime startDate, DateTime endDate)
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

                    // get the first day of the week of the currentDate
                    dates.Add(nextDate);

                    // move to the next week
                    nextDate = nextDate.AddDays(7);
                }
                return dates;
            }

            public static DateTime GetEndOfCurrentWeek(DateTime date)
            {
                return date.Date.AddDays(6 - (int)date.DayOfWeek);
            }

            public static DateTime GetEndOfPreviousWeek(DateTime date)
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
            public static HashSet<DateTime> GetEndOfWeekDates(DateTime startDate, DateTime endDate, bool completedTermsOnly)
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

                    // get the last day of the week of the currentDate
                    nextDate = nextDate.Date.AddDays(6 - (int)nextDate.DayOfWeek);
                    dates.Add(nextDate);
                    nextDate = nextDate.AddDays(7);
                }
                return dates;
            }

            public static List<IDateRange> GetWeeklyRanges(DateTime startDate, DateTime endDate, bool adjustStartAndEndsToDates)
            {
                var ranges = new List<IDateRange>();
                if (startDate > endDate)
                {
                    throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
                }

                var startOfWeekDates = GetStartOfWeekDates(startDate, endDate).OrderBy(d => d).ToArray();
                var endOfWeekDates = GetEndOfWeekDates(startDate, endDate).OrderBy(d => d).ToArray();
                if (startOfWeekDates.Length != endOfWeekDates.Length)
                {
                    throw new InvalidOperationException("Start and end dates count mismatch.");
                }

                if (adjustStartAndEndsToDates)
                {
                    startOfWeekDates[0] = startDate.Date;
                    endOfWeekDates[endOfWeekDates.Length - 1] = endDate.Date;
                }

                for (int i = 0; i < startOfWeekDates.Length; i++)
                {
                    ranges.Add(new DateRange { StartDate = startOfWeekDates[i], EndDate = endOfWeekDates[i] });
                }
                return ranges;
            }
        }





        public static HashSet<DateTime> GetStartOfMonthDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, startDate.Month, 1);
            while (neededDate <= endDate)
            {
                // get the last day of the month of the currentDate
                dates.Add(neededDate);
                neededDate = neededDate.AddMonths(1);
            }
            return dates;
        }

        public static HashSet<DateTime> GetEndOfMonthDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
            var endOfMonthNeeded = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
            while (neededDate <= endOfMonthNeeded)
            {
                // get the last day of the month of the currentDate
                var lastDayOfMonth = new DateTime(neededDate.Year, neededDate.Month, DateTime.DaysInMonth(neededDate.Year, neededDate.Month));
                dates.Add(lastDayOfMonth);
                neededDate = neededDate.AddMonths(1);
            }
            return dates;
        }

        public static List<IDateRange> GetMonthlyRanges(DateTime startDate, DateTime endDate, bool adjustStartAndEndsToDates)
        {

            throw new NotImplementedException("revise these range helpers and the tests.")

            var ranges = new List<IDateRange>();
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
            }

            var startOfMonthDates = GetStartOfMonthDates(startDate, endDate).ToArray();
            var endOfMonthDates = GetEndOfMonthDates(startDate, endDate).ToArray();
            if (startOfMonthDates.Length != endOfMonthDates.Length)
            {
                throw new InvalidOperationException("Start and end dates count mismatch.");
            }

            if (adjustStartAndEndsToDates)
            {
                startOfMonthDates[0] = startDate.Date;
                endOfMonthDates[endOfMonthDates.Length - 1] = endDate.Date;
            }

            for (int i = 0; i < startOfMonthDates.Length; i++)
            {
                ranges.Add(new DateRange { StartDate = startOfMonthDates[i], EndDate = endOfMonthDates[i] });
            }
            return ranges;
        }

        // get quarterly start dates
        public static HashSet<DateTime> GetStartOfQuarterDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, 1, 1);
            while (neededDate <= endDate)
            {
                // get the first day of the quarter of the currentDate
                var firstDayOfQuarter = new DateTime(neededDate.Year, ((neededDate.Month - 1) / 3) * 3 + 1, 1);
                dates.Add(firstDayOfQuarter);
                neededDate = neededDate.AddMonths(3);
            }
            return dates;
        }

        // get quarterly end dates
        public static HashSet<DateTime> GetEndOfQuarterDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, 1, 1);

            // Adjust the end date to the last day of the quarter
            var adjustedEndDate = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 3, DateTime.DaysInMonth(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 3));
            while (neededDate <= adjustedEndDate)
            {
                // get the last day of the quarter of the currentDate
                var lastDayOfQuarter = new DateTime(neededDate.Year, ((neededDate.Month - 1) / 3) * 3 + 3, DateTime.DaysInMonth(neededDate.Year, ((neededDate.Month - 1) / 3) * 3 + 3));
                dates.Add(lastDayOfQuarter);
                neededDate = neededDate.AddMonths(3);
            }
            return dates;
        }

        public static List<IDateRange> GetQuarterlyRanges(DateTime startDate, DateTime endDate, bool adjustStartAndEndsToDates)
        {
            var ranges = new List<IDateRange>();
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
            }

            var startOfQuarterDates = GetStartOfQuarterDates(startDate, endDate).OrderBy(d => d).ToArray();
            var endOfQuarterDates = GetEndOfQuarterDates(startDate, endDate).OrderBy(d => d).ToArray();
            if (startOfQuarterDates.Length != endOfQuarterDates.Length)
            {
                throw new InvalidOperationException("Start and end dates count mismatch.");
            }

            if (adjustStartAndEndsToDates)
            {
                startOfQuarterDates[0] = startDate.Date;
                endOfQuarterDates[endOfQuarterDates.Length - 1] = endDate.Date;
            }

            for (int i = 0; i < startOfQuarterDates.Length; i++)
            {
                ranges.Add(new DateRange { StartDate = startOfQuarterDates[i], EndDate = endOfQuarterDates[i] });
            }
            return ranges;
        }

        public static HashSet<DateTime> GetStartOfYearDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, 1, 1);
            while (neededDate <= endDate)
            {
                // get the first day of the year of the currentDate
                var firstDayOfYear = new DateTime(neededDate.Year, 1, 1);
                dates.Add(firstDayOfYear);
                neededDate = neededDate.AddYears(1);
            }
            return dates;
        }

        public static HashSet<DateTime> GetEndOfYearDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, 1, 1);
            // Adjust the end date to the last day of the year
            var adjustedEndDate = new DateTime(endDate.Year, 12, 31);
            while (neededDate <= adjustedEndDate)
            {
                // get the last day of the year of the currentDate
                var lastDayOfYear = new DateTime(neededDate.Year, 12, 31);
                dates.Add(lastDayOfYear);
                neededDate = neededDate.AddYears(1);
            }
            return dates;
        }

        public static List<IDateRange> GetYearlyRanges(DateTime startDate, DateTime endDate, bool adjustStartAndEndsToDates)
        {
            var ranges = new List<IDateRange>();
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
            }

            var startOfYearDates = GetStartOfYearDates(startDate, endDate).OrderBy(d => d).ToArray();
            var endOfYearDates = GetEndOfYearDates(startDate, endDate).OrderBy(d => d).ToArray();
            if (startOfYearDates.Length != endOfYearDates.Length)
            {
                throw new InvalidOperationException("Start and end dates count mismatch.");
            }

            if (adjustStartAndEndsToDates)
            {
                startOfYearDates[0] = startDate.Date;
                endOfYearDates[endOfYearDates.Length - 1] = endDate.Date;
            }

            for (int i = 0; i < startOfYearDates.Length; i++)
            {
                ranges.Add(new DateRange { StartDate = startOfYearDates[i], EndDate = endOfYearDates[i] });
            }
            return ranges;
        }

        /// <summary>
        /// Validates corresponding counts of dates and matches them based on position in the hashset.
        /// </summary>
        /// <param name="startDates"></param>
        /// <param name="endDates"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static HashSet<IDateRange> GetDateRangeHashset(HashSet<DateTime> startDates, HashSet<DateTime> endDates)
        {
            _ = startDates ?? throw new ArgumentNullException(nameof(startDates), "Start dates cannot be null.");
            _ = endDates ?? throw new ArgumentNullException(nameof(endDates), "End dates cannot be null.");

            if (startDates.Count != endDates.Count)
            {
                throw new InvalidOperationException("Start and end dates count mismatch.");
            }
            var ranges = new HashSet<IDateRange>();
            for (int i = 0; i < startDates.Count; i++)
            {
                var start = startDates.ElementAt(i);
                var end = endDates.ElementAt(i);
                if (start > end)
                {
                    throw new InvalidOperationException("Start date cannot be after end date.");
                }
                ranges.Add(new DateRange { StartDate = start, EndDate = end });
            }
            return ranges;
        }

        // create an array of data points or null based on a date set that returns a value or null if no value exists for that date.

        public static T[] CreateDataPointsAndFillMissingWithDefault<T>(HashSet<DateTime> dates, IEnumerable<(DateTime Date, T Value)> values)
            where T : struct
        {
            T[] result = CreateDataPointsInternal(dates, values, true)
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToArray();

            return result;
        }

        public static T?[] CreateDataPointsAndFillMissingWithNull<T>(HashSet<DateTime> dates, IEnumerable<(DateTime Date, T Value)> values)
            where T : struct
        {
            return CreateDataPointsInternal(dates, values, false);
        }

        private static T?[] CreateDataPointsInternal<T>(HashSet<DateTime> dates, IEnumerable<(DateTime Date, T Value)> values, bool populateDefaultsFormMissingData = false)
            // add in a where that allows for null values in the values collection
            where T : struct
        {
            if (dates == null || !dates.Any())
            {
                throw new ArgumentNullException(nameof(dates), "Dates cannot be null or empty.");
            }
            var dataPoints = new T?[dates.Count];

            var dateArray = dates.ToArray();

            // ensure that the dates are ordered ascending
            for (int i = 0; i < dateArray.Length; i++)
            {
                dateArray[i] = dateArray[i].Date;

                // validate that the user has sorted the dates ascending
                if (i > 0 && dateArray[i] < dateArray[i - 1])
                {
                    throw new ArgumentException(nameof(dates), $"Dates must be in ascending order. {nameof(dates)}[{i}] less than  {nameof(dates)}[{i - 1}] ('{dateArray[i]}' < '{dateArray[i - 1]}').");
                }

                // ensure that there is only a single or no value for each date in the values collection
                try
                {
                    var value = values.SingleOrDefault(v => v.Date == dateArray[i]);
                    // if the value is not null, assign it to the data point
                    if (value.Date == default)
                    {
                        if (populateDefaultsFormMissingData)
                        {
                            // if populateDefaultsFormMissingData is true, assign a default value
                            dataPoints[i] = default(T);
                        }
                        else
                        {
                            dataPoints[i] = null; // doesn't have a data point, so add a null
                        }
                    }
                    else
                    {
                        dataPoints[i] = value.Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(nameof(values), $"Values must be unique for each date. Duplicate value found for date '{dateArray[i]}'.", ex);
                }
            }
            return dataPoints;
        }

    }
}
