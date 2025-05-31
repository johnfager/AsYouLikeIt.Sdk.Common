using System;
using System.Collections.Generic;
using System.Text;

namespace AsYouLikeIt.Sdk.Common.Utilities.DateHelpers
{
    public enum DateRangeTermOptions
    {
        /// <summary>
        /// Terms with only partial data at the beginning are included.
        /// Terms that have not closed yet based on the end date are excluded.
        /// </summary>
        CompletedTermsOnly,
        /// <summary>
        /// Ranges will not start until within the begginning of a full term and end on the last complete term.
        /// </summary>
        FullTermsOnly
    }
}
