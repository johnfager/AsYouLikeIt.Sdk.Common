using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    /// <summary>
    /// Can be registered in thd dependency injection container to provide access to all date period providers.
    /// </summary>
    public class DataProviderService
    {
        private readonly IEnumerable<IDatePeriodProvider> _datePeriodProviders;

        public DataProviderService()
        {
            // use reflection to load all IDatePeriodProvider implementations
            _datePeriodProviders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IDatePeriodProvider).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(type => (IDatePeriodProvider)Activator.CreateInstance(type))
                .ToList();
        }

        public IDatePeriodProvider GetProvider(DatePeriodType datePeriodType)
        {
            // Find the provider that matches the requested DatePeriodType
            return _datePeriodProviders.SingleOrDefault(provider => provider.DatePeriodType == datePeriodType) ?? throw new NotImplementedException($"{nameof(IDatePeriodProvider)} '{datePeriodType}' is not implemented.");
        }

    }
}
