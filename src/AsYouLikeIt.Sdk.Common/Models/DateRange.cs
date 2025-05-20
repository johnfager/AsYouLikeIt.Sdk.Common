using System;

namespace AsYouLikeIt.Sdk.Common.Models
{
    public class DateRange : IDateRange
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }       
    }
}
