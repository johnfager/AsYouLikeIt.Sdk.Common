
namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var description = value
                            .GetType()
                            .GetMember(value.ToString())
                            .FirstOrDefault()
                            ?.GetCustomAttribute<DescriptionAttribute>()
                            ?.Description;

            return description ?? value.ToString();
        }
    }
}
