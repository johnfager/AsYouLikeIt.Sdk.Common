using System;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public static class Quarter
    {
        public static QuarterProvider Provider { get; } = new QuarterProvider();
    }

    public class QuarterProvider : DatePeriodProviderBase
    {
        public QuarterProvider() : base(DatePeriodType.Quarter)
        {
        }

        public override DateTime GetStartOfCurrent(DateTime date)
        {
            int month = ((date.Month - 1) / 3) * 3 + 1; // Calculate the first month of the quarter
            return new DateTime(date.Year, month, 1);
        }

        public override DateTime Increment(DateTime date, int units) => date.AddMonths(units * 3);

        protected override DateTime SetToEndTerm(DateTime date)
        {
            var startOfQuarter = GetStartOfCurrent(date);
            return Increment(startOfQuarter, 1).AddDays(-1); // Get the last day of the quarter
        }
    }
}
