using System;
using System.Collections.Generic;
using System.Text;

namespace AsYouLikeIt.Sdk.Common.Models
{
    public interface IDateRange
    {
        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }
    }
}
