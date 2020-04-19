using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sdk.Common.Extensions
{
    public static class GuidExtensions
    {
        public static bool GuidIsSet(this Guid helper)
        {
            return helper != Guid.Empty;
        }
    }
}
