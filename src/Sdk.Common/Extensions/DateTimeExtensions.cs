
namespace Sdk.Common.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime SqlSafeMinDate { get => new DateTime(1900, 1, 1); }

        public static DateTime SqlSafeMaxDate { get => new DateTime(9999, 12, 31); }

        /// <summary>
        /// If the value is set to either th SQL safe min date or the DateTime.MinDate
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static bool IsUnset(this DateTime helper)
        {
            return (helper == SqlSafeMinDate || helper == DateTime.MinValue);
        }

        public static bool IsUnset(this DateTime? helper)
        {
            if (!helper.HasValue) return true;
            return (helper == SqlSafeMinDate || helper == DateTime.MinValue);
        }

        public static DateTime DateToEndOfDay(this DateTime helper)
        {
            helper = Convert.ToDateTime(helper.ToShortDateString());
            helper = helper.AddDays(1).AddSeconds(-1);
            return helper;

        }

        public static DateTime DateAdd(this DateTime startFromDate, string datePart, int units)
        {
            if (string.IsNullOrEmpty(datePart))
            {
                throw new ArgumentException("datePart");
            }
            if (units == 0)
            {
                return startFromDate;
            }
            var datePartLower = datePart.ToLower();
            if (datePartLower == "ms")
            {
                return startFromDate.AddMilliseconds(units);
            }
            else if (datePartLower == "ss" || datePartLower == "s")
            {
                return startFromDate.AddSeconds(units);
            }
            else if (datePartLower == "mi" || datePartLower == "n")
            {
                return startFromDate.AddMinutes(units);
            }
            else if (datePartLower == "hh")
            {
                return startFromDate.AddHours(units);
            }
            else if (datePartLower == "wk" || datePartLower == "ww")
            {
                return startFromDate.AddDays(units * 7);
            }
            else if (datePartLower == "dd" || datePartLower == "d")
            {
                return startFromDate.AddDays(units);
            }
            else if (datePartLower == "mm" || datePartLower == "m")
            {
                return startFromDate.AddMonths(units);
            }
            else if (datePartLower == "yy" || datePartLower == "yyyy")
            {
                return startFromDate.AddYears(units);
            }
            else
            {
                throw new NotImplementedException(string.Format("datePart '{0}' is not implemented.", datePart));
            }
        }

        public static string ToYearMonthDateTimeIncludingZoneString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz");
        }

        /// <summary>
        /// Returns the format yyyyMMdd_hhmmsstt as a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetDateAndTimeToString(this DateTime input)
        {
            return string.Format("{0:yyyyMMdd}_{0:hhmmsstt}", input);
        }



    }
}
