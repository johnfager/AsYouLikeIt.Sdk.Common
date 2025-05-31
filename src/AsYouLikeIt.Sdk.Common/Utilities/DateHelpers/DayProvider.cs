using System;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public static class Day
    {
        public static DayProvider Provider { get; } = new DayProvider();
    }

    public class DayProvider : DatePeriodProviderBase
    {
        public DayProvider() : base(DatePeriodType.Day)
        {
        }

        public override DateTime GetStartOfCurrent(DateTime date) => date.Date;

        public override DateTime Increment(DateTime date, int units) => date.AddDays(units);

        protected override DateTime SetToEndTerm(DateTime date) => date.Date.AddDays(1).AddTicks(-1);
    }
}
