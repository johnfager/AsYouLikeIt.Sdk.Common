﻿using AsYouLikeIt.Sdk.Common.Models;
using System;
using System.Collections.Generic;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    /// <summary>
    /// Create common methods for date period providers to implement. 
    /// This will allow for consistent functionality across different date period providers
    /// and allow developers to easily switch between them without changing the code that uses them.
    /// </summary>
    public interface IDatePeriodProvider
    {
        DatePeriodType DatePeriodType { get; }

        DateTime GetStartOfCurrent(DateTime date);

        DateTime GetStartOfNext(DateTime date);

        DateTime GetEndOfCurrent(DateTime date);

        DateTime GetEndOfPrevious(DateTime date);

        HashSet<DateTime> GetStartingDates(DateTime startDate, DateTime endDate, bool trimIncompleteStartingTerms);

        HashSet<DateTime> GetEndingDates(DateTime startDate, DateTime endDate, bool trimIncompleteEndingTerms);

        List<IDateRange> GetTermRanges(DateTime startDate, DateTime endDate, bool trimIncompleteEndingTerms, bool trimIncompleteStartingTerms);
    }

    public enum DatePeriodType
    {
        Day,
        Week,
        Month,
        Quarter,
        Year
    }
}