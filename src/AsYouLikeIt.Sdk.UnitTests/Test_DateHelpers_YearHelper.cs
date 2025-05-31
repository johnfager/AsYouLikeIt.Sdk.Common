using AsYouLikeIt.Sdk.Common.Utilities.DateHelpers;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateHelpers_YearHelper
    {
        private readonly YearProvider _provider = new YearProvider();

        [Fact]
        public void GetStartOfCurrent_ReturnsFirstOfYear()
        {
            var date = new DateTime(2024, 6, 15);
            var startOfYear = _provider.GetStartOfCurrent(date);
            Assert.Equal(new DateTime(2024, 1, 1), startOfYear);
        }

        [Fact]
        public void GetEndOfCurrent_ReturnsLastOfYear()
        {
            var date = new DateTime(2024, 6, 15);
            var endOfYear = _provider.GetEndOfCurrent(date);
            Assert.Equal(new DateTime(2024, 12, 31), endOfYear);
        }

        [Fact]
        public void GetStartOfNext_ReturnsFirstOfNextYear()
        {
            var date = new DateTime(2024, 6, 15);
            var nextStart = _provider.GetStartOfNext(date);
            Assert.Equal(new DateTime(2025, 1, 1), nextStart);
        }

        [Fact]
        public void GetEndOfPrevious_ReturnsLastOfPreviousYear()
        {
            var date = new DateTime(2024, 6, 15);
            var prevEnd = _provider.GetEndOfPrevious(date);
            Assert.Equal(new DateTime(2023, 12, 31), prevEnd);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStartingDates_ReturnsAllYearStartsInRange(bool trimIncompleteStartingTerms)
        {
            var start = new DateTime(2022, 6, 15);
            var end = new DateTime(2025, 3, 10);
            var starts = _provider.GetStartingDates(start, end, trimIncompleteStartingTerms);

            if (trimIncompleteStartingTerms)
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2023, 1, 1),
                    new DateTime(2024, 1, 1),
                    new DateTime(2025, 1, 1)
                };
                Assert.Equal(expected, starts);
            }
            else
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2022, 1, 1),
                    new DateTime(2023, 1, 1),
                    new DateTime(2024, 1, 1),
                    new DateTime(2025, 1, 1)
                };
                Assert.Equal(expected, starts);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetEndingDates_CompletedTermsOnly_Variations(bool trimIncompleteEndingTerms)
        {
            var start = new DateTime(2022, 6, 15);
            var end = new DateTime(2025, 3, 10);
            var ends = _provider.GetEndingDates(start, end, trimIncompleteEndingTerms);

            if (trimIncompleteEndingTerms)
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2022, 12, 31),
                    new DateTime(2023, 12, 31),
                    new DateTime(2024, 12, 31)
                };
                Assert.Equal(expected, ends);
            }
            else
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2022, 12, 31),
                    new DateTime(2023, 12, 31),
                    new DateTime(2024, 12, 31),
                    new DateTime(2025, 12, 31)
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
            var start = new DateTime(2022, 6, 15);
            var end = new DateTime(2025, 3, 10);
            var ranges = _provider.GetTermRanges(start, end, trimIncompleteStartingTerms, trimIncompleteEndingTerms);

            if (trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count);
                Assert.Equal(new DateTime(2023, 1, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2023, 12, 31), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 1, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 12, 31), ranges[1].EndDate);
            }
            else if (!trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(3, ranges.Count);
                Assert.Equal(new DateTime(2022, 1, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2022, 12, 31), ranges[0].EndDate);
                Assert.Equal(new DateTime(2023, 1, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2023, 12, 31), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 1, 1), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 12, 31), ranges[2].EndDate);
            }
            else if (trimIncompleteStartingTerms && !trimIncompleteEndingTerms)
            {
                Assert.Equal(3, ranges.Count);
                Assert.Equal(new DateTime(2023, 1, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2023, 12, 31), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 1, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 12, 31), ranges[1].EndDate);
                Assert.Equal(new DateTime(2025, 1, 1), ranges[2].StartDate);
                Assert.Equal(new DateTime(2025, 12, 31), ranges[2].EndDate);
            }
            else
            {
                Assert.Equal(4, ranges.Count);
                Assert.Equal(new DateTime(2022, 1, 1), ranges[0].StartDate);
                Assert.Equal(new DateTime(2022, 12, 31), ranges[0].EndDate);
                Assert.Equal(new DateTime(2023, 1, 1), ranges[1].StartDate);
                Assert.Equal(new DateTime(2023, 12, 31), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 1, 1), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 12, 31), ranges[2].EndDate);
                Assert.Equal(new DateTime(2025, 1, 1), ranges[3].StartDate);
                Assert.Equal(new DateTime(2025, 12, 31), ranges[3].EndDate);
            }

            Assert.All(ranges, r => Assert.True(r.StartDate <= r.EndDate));
        }
    }
}