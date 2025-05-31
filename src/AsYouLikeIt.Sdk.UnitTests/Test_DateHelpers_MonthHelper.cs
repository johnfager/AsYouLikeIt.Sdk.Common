using AsYouLikeIt.Sdk.Common.Utilities.DateHelpers;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateHelpers_MonthHelper
    {
        private readonly MonthProvider _provider = new MonthProvider();

        [Fact]
        public void GetStartOfCurrent_ReturnsFirstOfMonth()
        {
            var date = new DateTime(2024, 6, 15);
            var startOfMonth = _provider.GetStartOfCurrent(date);
            Assert.Equal(new DateTime(2024, 6, 1), startOfMonth);
        }

        [Fact]
        public void GetEndOfCurrent_ReturnsLastOfMonth()
        {
            var date = new DateTime(2024, 6, 15);
            var endOfMonth = _provider.GetEndOfCurrent(date);
            Assert.Equal(new DateTime(2024, 6, 30), endOfMonth);
        }

        [Fact]
        public void GetStartOfNext_ReturnsFirstOfNextMonth()
        {
            var date = new DateTime(2024, 6, 15);
            var nextStart = _provider.GetStartOfNext(date);
            Assert.Equal(new DateTime(2024, 7, 1), nextStart);
        }

        [Fact]
        public void GetEndOfPrevious_ReturnsLastOfPreviousMonth()
        {
            var date = new DateTime(2024, 6, 15);
            var prevEnd = _provider.GetEndOfPrevious(date);
            Assert.Equal(new DateTime(2024, 5, 31), prevEnd);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStartingDates_ReturnsAllFirstsInRange(bool trimIncompleteStartingTerms)
        {
            var start = new DateTime(2024, 5, 15);
            var end = new DateTime(2024, 7, 10);
            var starts = _provider.GetStartingDates(start, end, trimIncompleteStartingTerms);

            if (trimIncompleteStartingTerms)
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 1),
                    new DateTime(2024, 7, 1)
                };
                Assert.Equal(expected, starts);
            }
            else
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 5, 1),
                    new DateTime(2024, 6, 1),
                    new DateTime(2024, 7, 1)
                };
                Assert.Equal(expected, starts);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetEndingDates_CompletedTermsOnly_Variations(bool trimIncompleteEndingTerms)
        {
            var start = new DateTime(2024, 5, 15);
            var end = new DateTime(2024, 7, 10);
            var ends = _provider.GetEndingDates(start, end, trimIncompleteEndingTerms);

            if (trimIncompleteEndingTerms)
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 5, 31),
                    new DateTime(2024, 6, 30)
                };
                Assert.Equal(expected, ends);
            }
            else
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 5, 31),
                    new DateTime(2024, 6, 30),
                    new DateTime(2024, 7, 31)
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
            var start = new DateTime(2024, 5, 15);
            var end = new DateTime(2024, 7, 10);
            var ranges = _provider.GetTermRanges(start, end, trimIncompleteStartingTerms, trimIncompleteEndingTerms);

            if (trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Single(ranges);
                Assert.Equal(new DateTime(2024, 6, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 30), ranges[0].EndDate);
            }
            else if (!trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2024, 5, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 5, 31), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 30), ranges[1].EndDate);
            }
            else if (trimIncompleteStartingTerms && !trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2024, 6, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 30), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 7, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 7, 31), ranges[1].EndDate);
            }
            else
            {
                Assert.Equal(3, ranges.Count);
                Assert.Equal(new DateTime(2024, 5, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 5, 31), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 30), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 7, 1), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 7, 31), ranges[2].EndDate);
            }

            Assert.All(ranges, r => Assert.True(r.StartDate <= r.EndDate));
        }
    }
}