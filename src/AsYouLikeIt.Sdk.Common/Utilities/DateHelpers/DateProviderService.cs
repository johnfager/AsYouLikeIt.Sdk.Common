using System;
using System.Collections.Generic;
using System.Linq;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    /// <summary>
    /// Can be registered in the dependency injection container to provide access to all date period providers.
    /// </summary>
    public class DateProviderService : IDateProviderService
    {
        private readonly IEnumerable<IDatePeriodProvider> _datePeriodProviders;

        public DateProviderService()
        {
            // Use reflection to load all IDatePeriodProvider implementations
            _datePeriodProviders = typeof(DateProviderService).Assembly // Fixed incorrect usage of Select on Assembly
                .GetTypes()
                .Where(type => typeof(IDatePeriodProvider).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(type => (IDatePeriodProvider)Activator.CreateInstance(type))
                .ToList();
        }

        public IDatePeriodProvider GetProvider(DatePeriodType datePeriodType)
        {
            // Find the provider that matches the requested DatePeriodType
            return _datePeriodProviders.SingleOrDefault(provider => provider.DatePeriodType == datePeriodType)
                ?? throw new NotImplementedException($"{nameof(IDatePeriodProvider)} '{datePeriodType}' is not implemented.");
        }
    }
}
