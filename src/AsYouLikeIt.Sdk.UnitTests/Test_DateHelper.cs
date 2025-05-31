using AsYouLikeIt.Sdk.Common.Utilities;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateHelper
    {
        [Fact]
        public void GetDailyDates_ReturnsCorrectCount()
        {
            // Arrange
            DateTime start = new DateTime(2025, 01, 01);
            DateTime end = new DateTime(2025, 01, 03);

            // Act
            HashSet<DateTime> dates = DateHelper.GetDailyDates(start, end);

            // Assert: Expecting 3 days (1st, 2nd, 3rd)
            Assert.Equal(3, dates.Count);
        }

        [Fact]
        public void GetStartOfWeekDates_ReturnsSameCountAsEndOfWeekDates()
        {
            // Arrange
            DateTime start = new DateTime(2025, 03, 04);
            DateTime end = new DateTime(2025, 04, 01);

            // Act
            HashSet<DateTime> startOfWeekDates = DateHelper.GetStartOfWeekDates(start, end);
            HashSet<DateTime> endOfWeekDates = DateHelper.GetEndOfWeekDates(start, end);

            // Assert: Both sets should have the same count.
            Assert.Equal(startOfWeekDates.Count, endOfWeekDates.Count);
        }

        [Fact]
        public void GetStartOfWeekDates_And_EndOfWeekDates_ReturnExpectedDates()
        {
            // Arrange
            // March 4, 2025 is a Tuesday. We'll cover 4 weeks.
            DateTime start = new DateTime(2025, 03, 04);
            DateTime end = new DateTime(2025, 03, 31);

            // Act
            var startOfWeekDates = DateHelper.GetStartOfWeekDates(start, end).OrderBy(d => d).ToArray();
            var endOfWeekDates = DateHelper.GetEndOfWeekDates(start, end).OrderBy(d => d).ToArray();

            // Assert: Check counts
            Assert.Equal(4, startOfWeekDates.Length);
            Assert.Equal(4, endOfWeekDates.Length);

            // Assert: Check actual dates
            // For March 2025, weeks starting on Sunday:
            // Week 1: Sunday, March 2 - Saturday, March 8
            // Week 2: Sunday, March 9 - Saturday, March 15
            // Week 3: Sunday, March 16 - Saturday, March 22
            // Week 4: Sunday, March 23 - Saturday, March 29
            Assert.Equal(new[]
            {
                new DateTime(2025, 3, 2),
                new DateTime(2025, 3, 9),
                new DateTime(2025, 3, 16),
                new DateTime(2025, 3, 23)
            }, startOfWeekDates);

            Assert.Equal(new[]
            {
                new DateTime(2025, 3, 8),
                new DateTime(2025, 3, 15),
                new DateTime(2025, 3, 22),
                new DateTime(2025, 3, 29)
            }, endOfWeekDates);
        }

        [Fact]
        public void GetStartOfMonthDates_ReturnsSameCountAsEndOfMonthDates()
        {
            // Arrange
            DateTime start = new DateTime(2025, 01, 01);
            DateTime end = new DateTime(2025, 04, 01); // spans January, February, March

            // Act
            HashSet<DateTime> startOfMonthDates = DateHelper.GetStartOfMonthDates(start, end);
            HashSet<DateTime> endOfMonthDates = DateHelper.GetEndOfMonthDates(start, end);

            // Assert: Both sets should have the same count.
            Assert.Equal(startOfMonthDates.Count, endOfMonthDates.Count);
            // Additionally, for this date range, expected count should be 3.
            Assert.Equal(4, startOfMonthDates.Count);
        }

        [Fact]
        public void GetStartOfMonthDates_And_EndOfMonthDates_ReturnExpectedDates()
        {
            // Arrange
            // Start and end are in different months, and the range covers partial months at both ends.
            DateTime start = new DateTime(2025, 01, 15);
            DateTime end = new DateTime(2025, 04, 10);

            // Act
            var startOfMonthDates = DateHelper.GetStartOfMonthDates(start, end).OrderBy(d => d).ToArray();
            var endOfMonthDates = DateHelper.GetEndOfMonthDates(start, end).OrderBy(d => d).ToArray();

            // Assert: Check counts (should include Jan, Feb, Mar, Apr)
            Assert.Equal(4, startOfMonthDates.Length);
            Assert.Equal(4, endOfMonthDates.Length);

            // Assert: Check actual dates
            Assert.Equal(new[]
            {
                new DateTime(2025, 1, 1),
                new DateTime(2025, 2, 1),
                new DateTime(2025, 3, 1),
                new DateTime(2025, 4, 1)
            }, startOfMonthDates);

            Assert.Equal(new[]
            {
                new DateTime(2025, 1, 31),
                new DateTime(2025, 2, 28),
                new DateTime(2025, 3, 31),
                new DateTime(2025, 4, 30)
            }, endOfMonthDates);

            // Additional: Check that the months containing the start and end dates are included
            Assert.Contains(startOfMonthDates, d => d.Month == start.Month && d.Year == start.Year);
            Assert.Contains(startOfMonthDates, d => d.Month == end.Month && d.Year == end.Year);
            Assert.Contains(endOfMonthDates, d => d.Month == start.Month && d.Year == start.Year);
            Assert.Contains(endOfMonthDates, d => d.Month == end.Month && d.Year == end.Year);
        }

        [Fact]
        public void GetStartOfYearDates_And_EndOfYearDates_ReturnExpectedDates()
        {
            // Arrange
            // Start and end are in different years, and the range covers partial years at both ends.
            DateTime start = new DateTime(2023, 5, 10);
            DateTime end = new DateTime(2025, 8, 20);

            // Act
            var startOfYearDates = DateHelper.GetStartOfYearDates(start, end).OrderBy(d => d).ToArray();
            var endOfYearDates = DateHelper.GetEndOfYearDates(start, end).OrderBy(d => d).ToArray();

            // Assert: Check counts (should include 2023, 2024, 2025)
            Assert.Equal(3, startOfYearDates.Length);
            Assert.Equal(3, endOfYearDates.Length);

            // Assert: Check actual dates
            Assert.Equal(new[]
            {
                new DateTime(2023, 1, 1),
                new DateTime(2024, 1, 1),
                new DateTime(2025, 1, 1)
            }, startOfYearDates);

            Assert.Equal(new[]
            {
                new DateTime(2023, 12, 31),
                new DateTime(2024, 12, 31),
                new DateTime(2025, 12, 31)
            }, endOfYearDates);

            // Additional: Check that the years containing the start and end dates are included
            Assert.Contains(startOfYearDates, d => d.Year == start.Year);
            Assert.Contains(startOfYearDates, d => d.Year == end.Year);
            Assert.Contains(endOfYearDates, d => d.Year == start.Year);
            Assert.Contains(endOfYearDates, d => d.Year == end.Year);
        }

        #region CreateDataPointsAndFillMissingWithNull Tests

        [Fact]
        public void CreateDataPoints_IntValues_ReturnsExpectedArray()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 2),
                new DateTime(2025, 1, 3)
            };
            var values = new List<(DateTime, int)>
            {
                (new DateTime(2025, 1, 1), 10),
                (new DateTime(2025, 1, 3), 30)
            };

            // Act
            var result = DateHelper.CreateDataPointsAndFillMissingWithNull<int>(dates, values);

            // Assert
            Assert.Equal(new int?[] { 10, null, 30 }, result);
        }

        [Fact]
        public void CreateDataPoints_DecimalValues_ReturnsExpectedArray()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 2, 1),
                new DateTime(2025, 2, 2)
            };
            var values = new List<(DateTime, decimal)>
            {
                (new DateTime(2025, 2, 1), 1.5m)
            };

            // Act
            var result = DateHelper.CreateDataPointsAndFillMissingWithNull<decimal>(dates, values);

            // Assert
            Assert.Equal(new decimal?[] { 1.5m, null }, result);
        }

        [Fact]
        public void CreateDataPoints_MissingDates_ThrowsArgumentNullException()
        {
            // Arrange
            HashSet<DateTime> dates = null;
            var values = new List<(DateTime, int)>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DateHelper.CreateDataPointsAndFillMissingWithNull<int>(dates, values));
        }

        [Fact]
        public void CreateDataPoints_DuplicateValues_ThrowsArgumentException()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 1, 1)
            };
            var values = new List<(DateTime, int)>
            {
                (new DateTime(2025, 1, 1), 1),
                (new DateTime(2025, 1, 1), 2)
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DateHelper.CreateDataPointsAndFillMissingWithNull<int>(dates, values));
        }

        [Fact]
        public void CreateDataPoints_UnorderedDates_ThrowsArgumentException()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 1, 2),
                new DateTime(2025, 1, 1)
            };
            var values = new List<(DateTime, int)>
            {
                (new DateTime(2025, 1, 1), 1),
                (new DateTime(2025, 1, 2), 2)
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DateHelper.CreateDataPointsAndFillMissingWithNull<int>(dates, values));
        }

        #region CreateDataPointsAndFillMissingWithDefault Tests

        [Fact]
        public void CreateDataPointsAndFillMissingWithDefault_IntValues_ReturnsExpectedArray()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 2),
                new DateTime(2025, 1, 3)
            };
            var values = new List<(DateTime, int)>
            {
                (new DateTime(2025, 1, 1), 10),
                (new DateTime(2025, 1, 3), 30)
            };

            // Act
            var result = DateHelper.CreateDataPointsAndFillMissingWithDefault<int>(dates, values);

            // Assert
            Assert.Equal(new int[] { 10, 0, 30 }, result);
        }

        [Fact]
        public void CreateDataPointsAndFillMissingWithDefault_DecimalValues_ReturnsExpectedArray()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 2, 1),
                new DateTime(2025, 2, 2)
            };
            var values = new List<(DateTime, decimal)>
            {
                (new DateTime(2025, 2, 1), 1.5m)
            };

            // Act
            var result = DateHelper.CreateDataPointsAndFillMissingWithDefault<decimal>(dates, values);

            // Assert
            Assert.Equal(new decimal[] { 1.5m, 0.0m }, result);
        }

        [Fact]
        public void CreateDataPointsAndFillMissingWithDefault_MissingDates_ThrowsArgumentNullException()
        {
            // Arrange
            HashSet<DateTime> dates = null;
            var values = new List<(DateTime, int)>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DateHelper.CreateDataPointsAndFillMissingWithDefault<int>(dates, values));
        }

        [Fact]
        public void CreateDataPointsAndFillMissingWithDefault_DuplicateValues_ThrowsArgumentException()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 1, 1)
            };
            var values = new List<(DateTime, int)>
            {
                (new DateTime(2025, 1, 1), 1),
                (new DateTime(2025, 1, 1), 2)
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DateHelper.CreateDataPointsAndFillMissingWithDefault<int>(dates, values));
        }

        [Fact]
        public void CreateDataPointsAndFillMissingWithDefault_UnorderedDates_ThrowsArgumentException()
        {
            // Arrange
            var dates = new HashSet<DateTime>
            {
                new DateTime(2025, 1, 2),
                new DateTime(2025, 1, 1)
            };
            var values = new List<(DateTime, int)>
            {
                (new DateTime(2025, 1, 1), 1),
                (new DateTime(2025, 1, 2), 2)
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DateHelper.CreateDataPointsAndFillMissingWithDefault<int>(dates, values));
        }

        #endregion

        [Fact]
        public void GetMonthlyRanges_ReturnsExpectedRanges()
        {
            // Arrange
            var start = new DateTime(2025, 1, 15);
            var end = new DateTime(2025, 3, 20);

            // Act
            var notAdjusted = DateHelper.GetMonthlyRanges(start, end, false);
            var adjusted = DateHelper.GetMonthlyRanges(start, end, true);

            // Assert: Not adjusted (should be full months)
            Assert.Equal(3, notAdjusted.Count);
            Assert.Equal(new DateTime(2025, 1, 1), notAdjusted[0].StartDate);
            Assert.Equal(new DateTime(2025, 1, 31), notAdjusted[0].EndDate);
            Assert.Equal(new DateTime(2025, 2, 1), notAdjusted[1].StartDate);
            Assert.Equal(new DateTime(2025, 2, 28), notAdjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 3, 1), notAdjusted[2].StartDate);
            Assert.Equal(new DateTime(2025, 3, 31), notAdjusted[2].EndDate);

            // Assert: Adjusted (first/last range should match input)
            Assert.Equal(3, adjusted.Count);
            Assert.Equal(start.Date, adjusted[0].StartDate);
            Assert.Equal(new DateTime(2025, 1, 31), adjusted[0].EndDate);
            Assert.Equal(new DateTime(2025, 2, 1), adjusted[1].StartDate);
            Assert.Equal(new DateTime(2025, 2, 28), adjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 3, 1), adjusted[2].StartDate);
            Assert.Equal(end.Date, adjusted[2].EndDate);
        }

        [Fact]
        public void GetWeeklyRanges_ReturnsExpectedRanges()
        {
            // Arrange
            var start = new DateTime(2025, 3, 4); // Tuesday
            var end = new DateTime(2025, 3, 25);  // Tuesday

            // Act
            var notAdjusted = DateHelper.GetWeeklyRanges(start, end, false);
            var adjusted = DateHelper.GetWeeklyRanges(start, end, true);

            // Assert: Not adjusted (weeks start Sunday, end Saturday)
            Assert.Equal(4, notAdjusted.Count);
            Assert.Equal(new DateTime(2025, 3, 2), notAdjusted[0].StartDate);
            Assert.Equal(new DateTime(2025, 3, 8), notAdjusted[0].EndDate);
            Assert.Equal(new DateTime(2025, 3, 9), notAdjusted[1].StartDate);
            Assert.Equal(new DateTime(2025, 3, 15), notAdjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 3, 16), notAdjusted[2].StartDate);
            Assert.Equal(new DateTime(2025, 3, 22), notAdjusted[2].EndDate);
            Assert.Equal(new DateTime(2025, 3, 23), notAdjusted[3].StartDate);
            Assert.Equal(new DateTime(2025, 3, 29), notAdjusted[3].EndDate);

            // Assert: Adjusted (first/last range should match input)
            Assert.Equal(4, adjusted.Count);
            Assert.Equal(start.Date, adjusted[0].StartDate);
            Assert.Equal(new DateTime(2025, 3, 8), adjusted[0].EndDate);
            Assert.Equal(new DateTime(2025, 3, 9), adjusted[1].StartDate);
            Assert.Equal(new DateTime(2025, 3, 15), adjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 3, 16), adjusted[2].StartDate);
            Assert.Equal(new DateTime(2025, 3, 22), adjusted[2].EndDate);
            Assert.Equal(new DateTime(2025, 3, 23), adjusted[3].StartDate);
            Assert.Equal(end.Date, adjusted[3].EndDate);
        }

        [Fact]
        public void GetQuarterlyRanges_ReturnsExpectedRanges()
        {
            // Arrange
            var start = new DateTime(2025, 2, 15);
            var end = new DateTime(2025, 8, 10);

            // Act
            var notAdjusted = DateHelper.GetQuarterlyRanges(start, end, false);
            var adjusted = DateHelper.GetQuarterlyRanges(start, end, true);

            // Assert: Not adjusted (quarters: Jan-Mar, Apr-Jun, Jul-Sep)
            Assert.Equal(3, notAdjusted.Count);
            Assert.Equal(new DateTime(2025, 1, 1), notAdjusted[0].StartDate);
            Assert.Equal(new DateTime(2025, 3, 31), notAdjusted[0].EndDate);
            Assert.Equal(new DateTime(2025, 4, 1), notAdjusted[1].StartDate);
            Assert.Equal(new DateTime(2025, 6, 30), notAdjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 7, 1), notAdjusted[2].StartDate);
            Assert.Equal(new DateTime(2025, 9, 30), notAdjusted[2].EndDate);

            // Assert: Adjusted (first/last range should match input)
            Assert.Equal(3, adjusted.Count);
            Assert.Equal(start.Date, adjusted[0].StartDate);
            Assert.Equal(new DateTime(2025, 3, 31), adjusted[0].EndDate);
            Assert.Equal(new DateTime(2025, 4, 1), adjusted[1].StartDate);
            Assert.Equal(new DateTime(2025, 6, 30), adjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 7, 1), adjusted[2].StartDate);
            Assert.Equal(end.Date, adjusted[2].EndDate);
        }

        [Fact]
        public void GetYearlyRanges_ReturnsExpectedRanges()
        {
            // Arrange
            var start = new DateTime(2023, 5, 10);
            var end = new DateTime(2025, 8, 20);

            // Act
            var notAdjusted = DateHelper.GetYearlyRanges(start, end, false);
            var adjusted = DateHelper.GetYearlyRanges(start, end, true);

            // Assert: Not adjusted (years: 2023, 2024, 2025)
            Assert.Equal(3, notAdjusted.Count);
            Assert.Equal(new DateTime(2023, 1, 1), notAdjusted[0].StartDate);
            Assert.Equal(new DateTime(2023, 12, 31), notAdjusted[0].EndDate);
            Assert.Equal(new DateTime(2024, 1, 1), notAdjusted[1].StartDate);
            Assert.Equal(new DateTime(2024, 12, 31), notAdjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 1, 1), notAdjusted[2].StartDate);
            Assert.Equal(new DateTime(2025, 12, 31), notAdjusted[2].EndDate);

            // Assert: Adjusted (first/last range should match input)
            Assert.Equal(3, adjusted.Count);
            Assert.Equal(start.Date, adjusted[0].StartDate);
            Assert.Equal(new DateTime(2023, 12, 31), adjusted[0].EndDate);
            Assert.Equal(new DateTime(2024, 1, 1), adjusted[1].StartDate);
            Assert.Equal(new DateTime(2024, 12, 31), adjusted[1].EndDate);
            Assert.Equal(new DateTime(2025, 1, 1), adjusted[2].StartDate);
            Assert.Equal(end.Date, adjusted[2].EndDate);
        }

        #endregion

    }
}
