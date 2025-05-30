using System;
using Xunit;
using AsYouLikeIt.Sdk.Common.Models;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateRanges
    {
        [Fact]
        public void DateRanges_WithSameStartAndEnd_AreEqual()
        {
            var a = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };
            var b = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };

            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
            Assert.True(a.Equals((object)b));
            Assert.Equal(a, b);
        }

        [Fact]
        public void DateRanges_WithDifferentStartOrEnd_AreNotEqual()
        {
            var a = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };
            var b = new DateRange { StartDate = new DateTime(2025, 2, 1), EndDate = new DateTime(2025, 1, 31) };
            var c = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 2, 28) };

            Assert.False(a.Equals(b));
            Assert.False(a.Equals(c));
            Assert.NotEqual(a, b);
            Assert.NotEqual(a, c);
        }

        [Fact]
        public void DateRange_Equals_Null_ReturnsFalse()
        {
            var a = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };
            DateRange b = null;

            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)null));
        }

        [Fact]
        public void DateRange_HashCode_IsConsistentForEqualRanges()
        {
            var a = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };
            var b = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };

            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void DateRange_Equals_IDateRange_Interface()
        {
            IDateRange a = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };
            IDateRange b = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = new DateTime(2025, 1, 31) };
            IDateRange c = new DateRange { StartDate = new DateTime(2025, 2, 1), EndDate = new DateTime(2025, 1, 31) };

            Assert.True(a.Equals(b));
            Assert.False(a.Equals(c));
        }

        [Fact]
        public void DateRange_WithMixedNulls_AreNotEqual()
        {
            var a = new DateRange { StartDate = null, EndDate = new DateTime(2025, 1, 31) };
            var b = new DateRange { StartDate = new DateTime(2025, 1, 1), EndDate = null };
            var c = new DateRange { StartDate = null, EndDate = null };
            var d = new DateRange { StartDate = null, EndDate = new DateTime(2025, 1, 31) };

            // a and b: different null positions, should not be equal
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));

            // a and c: only end date matches, should not be equal
            Assert.False(a.Equals(c));
            Assert.False(c.Equals(a));

            // a and d: both have same null and value positions, should be equal
            Assert.True(a.Equals(d));
            Assert.True(d.Equals(a));
            Assert.Equal(a, d);

            // c and c: both null, should be equal
            Assert.True(c.Equals(c));
        }
    }
}
