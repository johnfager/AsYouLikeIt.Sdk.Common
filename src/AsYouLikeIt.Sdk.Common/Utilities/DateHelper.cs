using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsYouLikeIt.Sdk.Common.Utilities
{
    public static class DateHelper
    {
        public static HashSet<DateTime> GetDailyDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            DateTime currentDate = startDate.Date;
            while (currentDate <= endDate)
            {
                // get the last day of the month of the currentDate
                dates.Add(currentDate);
                currentDate = currentDate.Date.AddDays(1);
            }
            return dates;
        }

        public static HashSet<DateTime> GetStartOfWeekDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = startDate.Date;
            while (neededDate <= endDate)
            {
                // get the first day of the week of the currentDate
                var firstDayOfWeek = neededDate.Date.AddDays(-(int)neededDate.DayOfWeek);
                dates.Add(firstDayOfWeek);
                neededDate = neededDate.AddDays(7);
            }
            return dates;
        }

        public static HashSet<DateTime> GetEndOfWeekDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = startDate.Date;
            while (neededDate <= endDate)
            {
                // get the last day of the week of the currentDate
                var lastDayOfWeek = neededDate.Date.AddDays(6 - (int)neededDate.DayOfWeek);
                dates.Add(lastDayOfWeek);
                neededDate = neededDate.AddDays(7);
            }
            return dates;
        }

        public static HashSet<DateTime> GetStartOfMonthDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, startDate.Month, 1);
            while (neededDate < endDate)
            {
                // get the last day of the month of the currentDate
                var lastDayOfMonth = new DateTime(neededDate.Year, neededDate.Month, DateTime.DaysInMonth(neededDate.Year, neededDate.Month));
                dates.Add(lastDayOfMonth);
                neededDate = neededDate.AddMonths(1);
            }
            return dates;
        }

        public static HashSet<DateTime> GetEndOfMonthDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
            while (neededDate < endDate)
            {
                // get the last day of the month of the currentDate
                var lastDayOfMonth = new DateTime(neededDate.Year, neededDate.Month, DateTime.DaysInMonth(neededDate.Year, neededDate.Month));
                dates.Add(lastDayOfMonth);
                neededDate = neededDate.AddMonths(1);
            }
            return dates;
        }

        // get quarterly start dates
        public static HashSet<DateTime> GetStartOfQuarterDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, 1, 1);
            while (neededDate < endDate)
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
            while (neededDate < endDate)
            {
                // get the last day of the quarter of the currentDate
                var lastDayOfQuarter = new DateTime(neededDate.Year, ((neededDate.Month - 1) / 3) * 3 + 3, DateTime.DaysInMonth(neededDate.Year, ((neededDate.Month - 1) / 3) * 3 + 3));
                dates.Add(lastDayOfQuarter);
                neededDate = neededDate.AddMonths(3);
            }
            return dates;
        }

        public static HashSet<DateTime> GetStartOfYearDates(DateTime startDate, DateTime endDate)
        {
            var dates = new HashSet<DateTime>();
            var neededDate = new DateTime(startDate.Year, 1, 1);
            while (neededDate < endDate)
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
            while (neededDate < endDate)
            {
                // get the last day of the year of the currentDate
                var lastDayOfYear = new DateTime(neededDate.Year, 12, 31);
                dates.Add(lastDayOfYear);
                neededDate = neededDate.AddYears(1);
            }
            return dates;
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
