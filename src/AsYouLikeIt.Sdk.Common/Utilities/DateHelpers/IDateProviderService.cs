namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public interface IDateProviderService
    {
        IDatePeriodProvider GetProvider(DatePeriodType datePeriodType);
    }
}
