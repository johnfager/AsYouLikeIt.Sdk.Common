
namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System;

    public static class DecimalExtensions
    {
        public static decimal RoundTo2(this decimal helper)
        {
            return Math.Round(helper, 2);
        }

        public static string ToStringLocalizeEnUs(this decimal helper, string format)
        {
            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:" + format + "}", helper);
        }
    }
}
