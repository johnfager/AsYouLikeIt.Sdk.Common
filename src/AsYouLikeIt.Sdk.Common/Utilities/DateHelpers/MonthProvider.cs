using System;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public static class Month
    {
        public static MonthProvider Provider { get; } = new MonthProvider();
    }

    public class MonthProvider : DatePeriodProviderBase
    {
        public MonthProvider() : base(DatePeriodType.Month)
        {
        }

        public override DateTime GetStartOfCurrent(DateTime date) => new DateTime(date.Year, date.Month, 1);

        public override DateTime Increment(DateTime date, int units) => date.AddMonths(units);

        protected override DateTime SetToEndTerm(DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

    }
}
