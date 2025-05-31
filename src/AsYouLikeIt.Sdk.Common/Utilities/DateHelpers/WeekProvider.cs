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
        public WeekProvider() : base(DatePeriodType.Week)
        {
        }

        public override DateTime GetStartOfCurrent(DateTime date) => date.Date.AddDays(-(int)date.DayOfWeek);

        public override DateTime Increment(DateTime date, int units) => date.AddDays(units * 7);

        protected override DateTime SetToEndTerm(DateTime date) => date.AddDays(6);
    }
}
