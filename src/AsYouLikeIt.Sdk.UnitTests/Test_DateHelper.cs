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
            Assert.Equal(3, startOfMonthDates.Count);
        }

        [Fact]
        public void GetStartOfQuarterDates_ReturnsSameCountAsEndOfQuarterDates()
        {
            // Arrange
            DateTime start = new DateTime(2025, 01, 01);
            DateTime end = new DateTime(2026, 01, 01); // full year quarters

            // Act
            HashSet<DateTime> startOfQuarterDates = DateHelper.GetStartOfQuarterDates(start, end);
            HashSet<DateTime> endOfQuarterDates = DateHelper.GetEndOfQuarterDates(start, end);

            // Assert: Both sets should have the same count.
            Assert.Equal(startOfQuarterDates.Count, endOfQuarterDates.Count);
            // For a full year starting at Jan 1 and ending just before the next Jan 1, expect 4 quarters.
            Assert.Equal(4, startOfQuarterDates.Count);
        }

        [Fact]
        public void GetStartOfYearDates_ReturnsSameCountAsEndOfYearDates()
        {
            // Arrange
            DateTime start = new DateTime(2025, 01, 01);
            DateTime end = new DateTime(2027, 01, 01); // covers 2025 and 2026

            // Act
            HashSet<DateTime> startOfYearDates = DateHelper.GetStartOfYearDates(start, end);
            HashSet<DateTime> endOfYearDates = DateHelper.GetEndOfYearDates(start, end);

            // Assert: Both sets should have the same count.
            Assert.Equal(startOfYearDates.Count, endOfYearDates.Count);
            // For this range, expect 2 years.
            Assert.Equal(2, startOfYearDates.Count);
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


        #endregion

    }
}
