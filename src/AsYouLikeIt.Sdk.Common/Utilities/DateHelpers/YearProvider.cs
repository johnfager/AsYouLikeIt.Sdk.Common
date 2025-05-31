using System;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public static class Year
    {
        public static YearProvider Provider { get; } = new YearProvider();
    }

    public class YearProvider : DatePeriodProviderBase
    {
        public YearProvider() : base(DatePeriodType.Year)
        {
        }

        public override DateTime GetStartOfCurrent(DateTime date) => new DateTime(date.Year, 1, 1);

        public override DateTime Increment(DateTime date, int units) => date.AddYears(units);

        protected override DateTime SetToEndTerm(DateTime date) => new DateTime(date.Year, 12, 31);
    }
}
