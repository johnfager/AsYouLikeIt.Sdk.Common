using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AsYouLikeIt.Sdk.Common.Models
{
    [DebuggerDisplay("{IpAddress} / {SubnetMask}")]
    public class IpAddressSet
    {
        public string IpAddress { get; set; }

        public string SubnetMask { get; set; }
    }
}
