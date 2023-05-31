
namespace Sdk.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumHelpers
    {
        /// <summary>
        /// Uses the description attribute to build a dictionary of Enum/Description
        /// Good for filters on views        ///
        /// If no description attribute, uses enum name
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        public static Dictionary<T, string> ToDictionaryWithDescription<T>()
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .OrderBy(s => s)
                       .ToDictionary(s => s,
                                     s =>
                                     {
                                         var attr = typeof(T).GetMember(s.ToString())
                                                                    .FirstOrDefault()
                                                                    ?.GetCustomAttribute<DescriptionAttribute>();

                                         return attr?.Description ?? s.ToString();
                                     });
        }
    }
}
