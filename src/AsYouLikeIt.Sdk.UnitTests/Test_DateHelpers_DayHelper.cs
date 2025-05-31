using AsYouLikeIt.Sdk.Common.Utilities.DateHelpers;
using Xunit;
using System;
using System.Collections.Generic;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateHelpers_DayHelper
    {
        private readonly DayProvider _provider = new DayProvider();

        [Fact]
        public void GetStartOfCurrent_ReturnsDateItself()
        {
            var date = new DateTime(2024, 6, 5, 15, 30, 0); // 3:30 PM
            var startOfDay = _provider.GetStartOfCurrent(date);
            Assert.Equal(new DateTime(2024, 6, 5), startOfDay);
        }

        [Fact]
        public void GetEndOfCurrent_ReturnsEndOfDay()
        {
            var date = new DateTime(2024, 6, 5, 10, 0, 0);
            var endOfDay = _provider.GetEndOfCurrent(date);
            Assert.Equal(new DateTime(2024, 6, 6).AddTicks(-1), endOfDay);
        }

        [Fact]
        public void GetStartOfNext_ReturnsNextDay()
        {
            var date = new DateTime(2024, 6, 5);
            var nextStart = _provider.GetStartOfNext(date);
            Assert.Equal(new DateTime(2024, 6, 6), nextStart);
        }

        [Fact]
        public void GetEndOfPrevious_ReturnsPreviousDayEnd()
        {
            var date = new DateTime(2024, 6, 5);
            var prevEnd = _provider.GetEndOfPrevious(date);
            Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), prevEnd);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStartingDates_ReturnsAllDaysInRange(bool trimIncompleteStartingTerms)
        {
            var start = new DateTime(2024, 6, 3);
            var end = new DateTime(2024, 6, 5);

            var starts = _provider.GetStartingDates(start, end, trimIncompleteStartingTerms);

            var expected = new HashSet<DateTime>
            {
                new DateTime(2024, 6, 3),
                new DateTime(2024, 6, 4),
                new DateTime(2024, 6, 5)
            };
            Assert.Equal(expected, starts);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetEndingDates_CompletedTermsOnly_Variations(bool trimIncompleteEndingTerms)
        {
            var start = new DateTime(2024, 6, 3);
            var end = new DateTime(2024, 6, 5);

            var ends = _provider.GetEndingDates(start, end, trimIncompleteEndingTerms);

            if (trimIncompleteEndingTerms)
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 3).AddDays(1).AddTicks(-1),
                    new DateTime(2024, 6, 4).AddDays(1).AddTicks(-1)
                };
                Assert.Equal(expected, ends);
            }
            else
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 3).AddDays(1).AddTicks(-1),
                    new DateTime(2024, 6, 4).AddDays(1).AddTicks(-1),
                    new DateTime(2024, 6, 5).AddDays(1).AddTicks(-1)
                };
                Assert.Equal(expected, ends);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void GetTermRanges_AllCombinations(bool trimIncompleteStartingTerms, bool trimIncompleteEndingTerms)
        {
            var start = new DateTime(2024, 6, 3).AddHours(5);
            var end = new DateTime(2024, 6, 5).AddHours(8);

            var ranges = _provider.GetTermRanges(start, end, trimIncompleteStartingTerms, trimIncompleteEndingTerms);

            if (trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Single(ranges);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[0].EndDate);
            }
            else if (!trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 3), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 4).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[1].EndDate);
            }
            else if (trimIncompleteStartingTerms && !trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 5), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 6).AddTicks(-1), ranges[1].EndDate);
            }
            else
            {
                Assert.Equal(3, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 3), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 4).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 6, 5), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 6, 6).AddTicks(-1), ranges[2].EndDate);
            }

            Assert.All(ranges, r => Assert.True(r.StartDate <= r.EndDate));
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void GetTermRanges_MidnightDates(bool trimIncompleteStartingTerms, bool trimIncompleteEndingTerms)
        {
            var start = new DateTime(2024, 6, 3);
            var end = new DateTime(2024, 6, 5);

            var ranges = _provider.GetTermRanges(start, end, trimIncompleteStartingTerms, trimIncompleteEndingTerms);

            if (trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 3), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 4).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[1].EndDate);
            }
            else if (!trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 3), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 4).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[1].EndDate);
            }
            else if (trimIncompleteStartingTerms && !trimIncompleteEndingTerms)
            {
                Assert.Equal(3, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 3), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 4).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 6, 5), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 6, 6).AddTicks(-1), ranges[2].EndDate);
            }
            else
            {
                Assert.Equal(3, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 3), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 4).AddTicks(-1), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 4), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 5).AddTicks(-1), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 6, 5), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 6, 6).AddTicks(-1), ranges[2].EndDate);
            }

            Assert.All(ranges, r => Assert.True(r.StartDate <= r.EndDate));
        }
    }
}