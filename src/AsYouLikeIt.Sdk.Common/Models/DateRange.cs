using System;
using System.Diagnostics;

namespace AsYouLikeIt.Sdk.Common.Models
{

    [DebuggerDisplay("DateRange '{StartDate}' - '{EndDate}'")]
    public class DateRange : IDateRange, IEquatable<DateRange>
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateRange()
        {
        }

        public DateRange(DateTime? startDate, DateTime? endDate)
        {
            if(startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.", nameof(startDate));
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        public override bool Equals(object obj) => Equals(obj as DateRange);

        public bool Equals(DateRange other)
        {
            if (other is null) return false;
            return StartDate == other.StartDate && EndDate == other.EndDate;
        }

        public bool Equals(IDateRange other)
        {
            if (other is null) return false;
            return StartDate == other.StartDate && EndDate == other.EndDate;
        }

        public override int GetHashCode() => HashCode.Combine(StartDate, EndDate);
    }
}
