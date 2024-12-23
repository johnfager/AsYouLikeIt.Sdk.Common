namespace AsYouLikeIt.Sdk.Common.Extensions
{
    using System;

    public static class GuidExtensions
    {
        public static bool GuidIsSet(this Guid helper)
        {
            return helper != Guid.Empty;
        }
    }
}
