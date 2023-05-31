

namespace AsYouLikeIt.Sdk.Common.Utilities
{

    using System;
    using System.Text.RegularExpressions;

    public class RegExPatterns
    {

        public static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                Regex.Match("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        public const string HasRegex = @"^[0-9A-Fa-f\r\n]+$";

        public const string WordAndSpace = "[^\\w\\s]";

        /// <summary>
        /// Matches HTML tags (see http://haacked.com/archive/2004/10/25/usingregularexpressionstomatchhtml.aspx)
        /// </summary>
        /// <remarks></remarks>

        public const string HtmlTag = "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>";

        /// <summary>
        /// Matches Markup language tags, but does not validate
        /// </summary>
        /// <remarks></remarks>
        public const string MarkupTag = "\\{\\$/?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)[(&nbsp;) ]*/\\}";


        public const string SimpleString = "[a-zA-Z0-9_]+";

        public const string SafeColumnHeaders = @"[a-zA-Z0-9_&#\-+/: ]+";

        public const string SimpleStringSpaces = "[a-zA-Z0-9_ ]+";

        public const string NumericValuesOnly = "[0-9]+";

        public const string Domain = "\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        public const string DomainClassicStrong = "^([a-zA-Z0-9]([a-zA-Z0-9\\-]{0,61}[a-zA-Z0-9])?\\.)+[a-zA-Z]{2,6}$";

        public const string Serialization = "(%%[A-Za-z0-9\\-_.\\[\\])^%]*%%)";


        //public const string PhoneNumber = @"(?=(.*\d){10})"; // @"(^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$)*";

        public const string PhoneNumber = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        /// <summary>
        /// Matches an internet email address
        /// </summary>
        /// <remarks></remarks>
        public const string EmailAddress = "^((?>[a-zA-Z\\d!#$%&'*+\\-/=?^_`{|}~]+\\x20*|\"((?=[\\x01-\\x7f])[^\"\\\\]|\\\\[\\x01-\\x7f])*\"\\x20*)*(?<angle><))?((?!\\.)(?>\\.?[a-zA-Z\\d!#$%&'*+\\-/=?^_`{|}~]+)+|\"((?=[\\x01-\\x7f])[^\"\\\\]|\\\\[\\x01-\\x7f])*\")@(((?!-)[a-zA-Z\\d\\-]+(?<!-)\\.)+[a-zA-Z]{2,}|\\[(((?(?<!\\[)\\.)(25[0-5]|2[0-4]\\d|[01]?\\d?\\d)){4}|[a-zA-Z\\d\\-]*[a-zA-Z\\d]:((?=[\\x01-\\x7f])[^\\\\\\[\\]]|\\\\[\\x01-\\x7f])+)\\])(?(angle)>)$";
        //changed: WAS = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"

        public const string EmailAddressJavaScript = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        /// <summary>
        /// Matches an internet IPv4 address
        /// </summary>
        /// <remarks></remarks>

        public const string IpAddress = "^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        /// <summary>
        /// Matches an integer (number greater than zero)
        /// </summary>
        /// <remarks></remarks>

        public const string Integer = "^\\d+$";

        public const string Number = "[0-9]";

        public const string Numbers = "[^0-9]";

        public const string NumbersAndPeriods = "[^0-9.]";

        public const string Currency = "^(?!\\u00a2)\\p{Sc}?(?!0,?\\d)(?:\\d{1,3}(?:([, .])\\d{3})?(?:\\1\\d{3})*|(?:\\d+))((?!\\1)[,.]\\d{2})?$";

        /// <summary>
        /// Matches strings that contain only A-Z a-z 0-9
        /// </summary>
        public const string StripNonAlphaNumeric = "[^0-9A-Za-z]";

        ///// <summary>
        ///// Matches strings that contain only A-Z a-z 0-9 - _
        ///// </summary>
        ///// <remarks></remarks>
        //public const string AlphaNumericDashesUnderscores = "[a-zA-Z0-9_\\-]+";

        //public const string AlphaNumericUnderscores = "[a-zA-Z0-9_]+";

        //public const string AlphaNumericDashesNoUnderscores = "[a-zA-Z0-9\\-]+";

        public const string AlphaNumericStartAndEndNonConsecutiveDashesLower = "^[a-z0-9]+(-[a-z0-9]+)*$";

        public const string AlphaNumericStartAndEndNonConsecutiveDashes = "^[a-zA-Z0-9]+(-[a-zA-Z0-9]+)*$";

        public const string AlphaNumericStartAndEndNonConsecutiveUnderscores = @"^[a-zA-Z0-9]+(_[a-zA-Z0-9]+)*$";

        public const string AlphaNumericStartAndEndNonConsecutiveDashesUnderscores = @"^[a-zA-Z0-9]+([-_][a-zA-Z0-9]+)*$";

        public const string DataSetName = "^[A-Za-z][A-Za-z0-9]{2,62}$";

        /// <summary>
        /// Matches strings that contain only A-Z a-z 0-9 - _ .
        /// </summary>
        /// <remarks></remarks>
        public const string AlphaNumericDashesUnderscoresPeriods = "[a-zA-Z0-9._\\-]+";

        //StripNonAlphaNumericDashUnderscore

        /// <summary>
        /// Matches strings that contain only A-Z a-z 0-9 - _ / .
        /// </summary>
        /// <remarks></remarks>
        public const string AlphaNumericDashesUnderscoresFwdSlashesPeriods = "[/a-zA-Z0-9._\\-]+";

        /// <summary>
        /// Matches strings that contain only A-Z a-z 0-9 - _ / .
        /// </summary>
        /// <remarks></remarks>
        public const string AlphaNumericDashesUnderscoresFwdSlashesPeriodsPercent = "[/a-zA-Z0-9._\\-%]+";

        /// <summary>
        /// Matches strings that contain only A-Z a-z 0-9 - _
        /// </summary>
        /// <remarks></remarks>
        public static string AlphaNumericDashesUnderscoresPeriodsApostprohesSpaces = "[a-zA-Z0-9_\\-.' ]+";

        //[\x00-\x1f]
        public const string InvalidXmlCharacters = "[\\x00-\\x08,\\x0B-\\x1F,\\x7F]";

        public const string UrlOrBlank = @"^(|https?:\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)$";

        public const string UrlSecureOrBlank = @"^(|https:\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)$";

        public const string UrlSecureLocalhostOrBlank = @"(^https?:\/\/localhost((:[0-9]+)?)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)$|^(|https:\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)$";

    }

    public static class RegExMessages
    {
        public const string UrlOrBlank = "Value must be a valid URL.";

        public const string UrlSecureOrBlank = "Value must be a valid secure URL starting with https://.";

        public const string UrlSecureLocalhostOrBlank = "Value must be a valid secure URL starting with https:// or http://localhost for testing.";

        public const string SafeColumnHeaders = "Columns may only contain letters, numbers, spaces, ampersands, pound symbols, dashes and underscores.";

        public const string ValidKey = "Value is not a valid key.";

    }
}
