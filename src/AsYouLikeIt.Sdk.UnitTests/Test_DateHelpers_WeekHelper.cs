using AsYouLikeIt.Sdk.Common.Utilities.DateHelpers;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateHelpers_WeekHelper
    {
        private readonly WeekProvider _provider = new WeekProvider();

        [Fact]
        public void GetStartOfCurrent_ReturnsSunday()
        {
            var date = new DateTime(2024, 6, 5); // Wednesday
            var startOfWeek = _provider.GetStartOfCurrent(date);
            Assert.Equal(new DateTime(2024, 6, 2), startOfWeek); // Sunday
        }

        [Fact]
        public void GetEndOfCurrent_ReturnsSaturday()
        {
            var date = new DateTime(2024, 6, 5); // Wednesday
            var endOfWeek = _provider.GetEndOfCurrent(date);
            Assert.Equal(new DateTime(2024, 6, 8), endOfWeek); // Saturday
        }

        [Fact]
        public void GetStartOfNext_ReturnsNextSunday()
        {
            var date = new DateTime(2024, 6, 5); // Wednesday
            var nextStart = _provider.GetStartOfNext(date);
            Assert.Equal(new DateTime(2024, 6, 9), nextStart); // Next Sunday
        }

        [Fact]
        public void GetEndOfPrevious_ReturnsPreviousSaturday()
        {
            var date = new DateTime(2024, 6, 5); // Wednesday
            var prevEnd = _provider.GetEndOfPrevious(date);
            Assert.Equal(new DateTime(2024, 6, 1), prevEnd); // Previous Saturday
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetStartingDates_ReturnsAllSundaysInRange(bool trimIncompleteStartingTerms)
        {
            var start = new DateTime(2024, 6, 3); // Monday
            var end = new DateTime(2024, 6, 20);  // Thursday
            var starts = _provider.GetStartingDates(start, end, trimIncompleteStartingTerms);

            if (trimIncompleteStartingTerms)
            {
                // If trimIncompleteStartingTerms is true, the first week should not be included if it starts before the start date
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 9),
                    new DateTime(2024, 6, 16)
                };
                Assert.Equal(expected, starts);
            }
            else
            {
                // If trimIncompleteStartingTerms is false, the first week should be included
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 2),
                    new DateTime(2024, 6, 9),
                    new DateTime(2024, 6, 16)
                };
                Assert.Equal(expected, starts);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetEndingDates_CompletedTermsOnly_Variations(bool trimIncompleteEndingTerms)
        {
            var start = new DateTime(2024, 6, 3); // Monday
            var end = new DateTime(2024, 6, 20);  // Thursday
            var ends = _provider.GetEndingDates(start, end, trimIncompleteEndingTerms);
            if (trimIncompleteEndingTerms)
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 8),
                    new DateTime(2024, 6, 15)
                };
                Assert.Equal(expected, ends);
            }
            else
            {
                var expected = new HashSet<DateTime>
                {
                    new DateTime(2024, 6, 8),
                    new DateTime(2024, 6, 15),
                    new DateTime(2024, 6, 22)
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
            var start = new DateTime(2024, 6, 3); // Monday
            var end = new DateTime(2024, 6, 20);  // Thursday
            var ranges = _provider.GetTermRanges(start, end, trimIncompleteStartingTerms, trimIncompleteEndingTerms);

            // Check the number of ranges based on the parameters
            if (trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Single(ranges); // One complete week
                Assert.Equal(new DateTime(2024, 6, 9), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 15), ranges[0].EndDate);
            }
            else if (!trimIncompleteStartingTerms && trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count); // Two complete weeks + one partial week
                // add an assertion that the first range starts on the first Sunday after the start date
                Assert.Equal(new DateTime(2024, 6, 2), ranges[0].StartDate);
                // add an assertion that the last range ends on the last Saturday after the end date
                Assert.Equal(new DateTime(2024, 6, 15), ranges[^1].EndDate);
                // The first week in the trimmed result should not include the week starting before the start date
                Assert.DoesNotContain(ranges, r => r.EndDate == new DateTime(2024, 6, 20));
            }
            else if (trimIncompleteStartingTerms && !trimIncompleteEndingTerms)
            {
                Assert.Equal(2, ranges.Count); // Two complete weeks starting from the first Sunday after the start date
                Assert.Equal(new DateTime(2024, 6, 9), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 15), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 16), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 22), ranges[1].EndDate);
            }
            else
            {
                Assert.Equal(3, ranges.Count); // Two complete weeks + one partial week starting from the first Sunday before the start date
                Assert.Equal(new DateTime(2024, 6, 2), ranges[0].StartDate);
                Assert.Equal(new DateTime(2024, 6, 8), ranges[0].EndDate);
                Assert.Equal(new DateTime(2024, 6, 9), ranges[1].StartDate);
                Assert.Equal(new DateTime(2024, 6, 15), ranges[1].EndDate);
                Assert.Equal(new DateTime(2024, 6, 16), ranges[2].StartDate);
                Assert.Equal(new DateTime(2024, 6, 22), ranges[2].EndDate);
            }

            Assert.All(ranges, r => Assert.True(r.StartDate <= r.EndDate));
        }
    }
}
