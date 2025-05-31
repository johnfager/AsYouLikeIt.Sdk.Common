using AsYouLikeIt.Sdk.Common.Utilities.DateHelpers;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_DateHelpers_DateProviderService
    {
        [Fact]
        public void GetStartOfCurrent_ReturnsDateItself()
        {
            var provider = new DateProviderService();
            foreach (var item in Enum.GetValues(typeof(DatePeriodType)).Cast<DatePeriodType>())
            {
                var dateProvider = provider.GetProvider(item);
                Assert.NotNull(dateProvider);
            }
        }
    }
}
