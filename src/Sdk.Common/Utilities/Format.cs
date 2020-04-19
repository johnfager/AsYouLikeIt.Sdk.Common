
namespace Sdk.Common.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Extensions;
    using System.Collections.Specialized;

    public static class Format
    {
        // <summary>
        /// Patches all items starting with '~/' style paths. Resolves the Kentico WYSIWG editor's output of images and other links.
        /// </summary>
        /// <param name="applicationPath">The path to resolve in place of the tilde (if any). Examples: '/', '', '/MyApp', 'MyApp', '/MyApp' would all be create a correct URL.</param>
        /// <param name="html">Input html content.</param>
        /// <returns>Resolved urls to relative paths.</returns>
        /// <remarks>Kentico stores '/xyz' links as '~/xyz' in rich text editors despite not showing that way in the admin.</remarks>
        public static string StripTildeCharacters(string applicationPath, string html)
        {

            if (string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            // cleanup app path
            if (applicationPath == null)
            {
                applicationPath = string.Empty;
            }
            applicationPath = applicationPath.StripAllLeadingAndTrailingSlashes();
            if (applicationPath.Trim() == "/")
            {
                applicationPath = string.Empty;
            }
            if (!string.IsNullOrEmpty(applicationPath))
            {
                applicationPath = "/" + applicationPath.StripAllLeadingAndTrailingSlashes();
            }

            var pathIndex = html.IndexOf("~/");
            if (pathIndex >= 1)
            {
                var sb = new StringBuilder((int)(html.Length * 1.1));
                var lastIndex = 0;
                while (pathIndex >= 1)
                {
                    if ((html[pathIndex - 1] == '(') || (html[pathIndex - 1] == '"') || (html[pathIndex - 1] == '\''))
                    {
                        // Add previous content
                        if (lastIndex < pathIndex)
                        {
                            sb.Append(html, lastIndex, pathIndex - lastIndex);
                        }

                        // Add application path and move to the next location
                        sb.Append(applicationPath);
                        lastIndex = pathIndex + 1;
                    }

                    pathIndex = html.IndexOf("~/", pathIndex + 2);
                }

                // Add the rest of the content
                if (lastIndex < html.Length)
                {
                    sb.Append(html, lastIndex, html.Length - lastIndex);
                }

                html = sb.ToString();
            }
            return html;
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                           .Where(x => x % 2 == 0)
                           .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                           .ToArray();
        }

        public static string GetMaskedEmail(string email)
        {
            if (!email.Contains("@"))
            {
                throw new FormatException(string.Format("Email '{0}' is not validly formatted.", email));
            }
            var emailParts = email.SplitStringAndTrim("@").ToArray();
            var emailFirstPart = emailParts[0];
            var emailMask = email;
            try
            {
                if (emailFirstPart.Length > 2)
                {
                    var masked = emailMask.Substring(0, 1);
                    for (int i = 0; i < emailFirstPart.Length - 1; i++)
                    {
                        masked += "*";
                    }
                    emailMask = masked + email.Substring(emailFirstPart.Length);
                }
            }
            catch { }
            if (string.IsNullOrEmpty(emailMask))
            {
                emailMask = email;
            }
            return emailMask;
        }

        public static string DefaultNullToEmpty(object input)
        {
            if (input is int i)
            {
                if (i <= 0)
                {
                    return string.Empty;
                }
            }
            else if (input is DateTime dateTime)
            {
                if (dateTime.IsUnset())
                {
                    return string.Empty;
                }
            }
            return input.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="population">The number affected</param>
        /// <param name="universe">The count of all involved</param>
        /// <returns></returns>zx
        public static double GetPercentage(int population, int universe)
        {
            if (universe == 0)
            {
                return 0;
            }
            else
            {
                return (population / (double)universe); // must cast to double in C#
            }
        }

        public static string TextToHtml(string str)
        {
            if (str == null)
            {
                return null;
            }
            string output = str;
            output = output.Replace("\n", "\n<br />");
            return output;
        }

        public static string StripInvalidXmlCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            input = Regex.Replace(input, RegExPatterns.InvalidXmlCharacters, string.Empty);
            return input;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static bool IsFullMatch(string input, string regExPattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regExPattern))
            {
                return false;
            }
            Match match = Regex.Match(input, regExPattern);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsFullMatchCaseInsensitive(string input, string regExPattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(regExPattern))
            {
                return false;
            }
            var match = Regex.Match(input, regExPattern, RegexOptions.IgnoreCase);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsFullMatch(string input, string regExPattern, bool allowNullOrEmpty)
        {
            if (allowNullOrEmpty && string.IsNullOrEmpty(input))
            {
                return true;
            }
            return IsFullMatch(input, regExPattern);
        }

        public static bool IsValidEmailAddress(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match match = Regex.Match(input, RegExPatterns.EmailAddress);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static string FormatPhoneNumber(string phoneNumber, bool returnNullOnError)
        {
            //Attempt to convert to formatted number
            //Handle extensions and international
            phoneNumber += string.Empty;
            bool appendMobile = false;
            if (phoneNumber.EndsWith(" M"))
            {
                appendMobile = true;
            }
            string strPhone = phoneNumber.StripNonNumeric().TrimStart(new char[] { '0', '1' }); // Handle leading 0 1 

            string extension = null;
            Int64 phoneAsBigInt = 0;
            try
            {
                // could be conversion issues, so use bigint here
                phoneAsBigInt = Convert.ToInt64(strPhone);
                strPhone = phoneAsBigInt.ToString();
                // handle extensions or more than 10 digits
                if (strPhone.Length > 10)
                {
                    extension = strPhone.Substring(10, strPhone.Length - 10);
                    strPhone = strPhone.Left(10);
                    phoneAsBigInt = Convert.ToInt64(strPhone);
                }
            }
            catch { }

            if (strPhone.Length < 10)
            {
                if (returnNullOnError)
                {
                    return null;
                }
                else
                {
                    return strPhone;
                }
            }

            // attempt to parse
            try
            {
                phoneNumber = string.Format("{0:(###) ###-####}", phoneAsBigInt);
            }
            catch (Exception)
            {
            }

            if (extension != null)
            {
                // auto-dial format for extensions in smartphones
                phoneNumber += ";" + extension;
            }

            if (appendMobile)
            {
                phoneNumber += " M";
            }
            return phoneNumber;
        }

        /// <summary>
        /// Merges a file path, changes any forward slashes to back slashes.
        /// </summary>
        /// <returns></returns>
        public static string PathMerge(params string[] paths)
        {
            if (paths == null || !paths.Any())
            {
                throw new ArgumentException(nameof(paths));
            }
            for (int i = 0; i < paths.Length; i++)
            {
                if (i == 0)
                {
                    // for the first arg, strip only the trailing slashes
                    paths[i] = paths[i].SwitchForwardSlashToBackSlash().StripTrailingBackslashesAll().Trim();
                }
                else if (i == paths.Length - 1)
                {
                    // for the last arg, strip only the trailing slashes
                    paths[i] = paths[i].SwitchForwardSlashToBackSlash().StripLeadingBackslashesAll().Trim();
                }
                else
                {
                    // otherwise strip all leading and trailing slashes
                    paths[i] = paths[i].SwitchForwardSlashToBackSlash().StripAllLeadingAndTrailingSlashes().Trim();
                }
            }
            return string.Join("\\", paths);
        }

        /// <summary>
        /// Merges a set of values into a path, changes any back slashes to forward slahes.
        /// </summary>
        /// <returns></returns>
        public static string PathMergeForwardSlashes(params string[] paths)
        {
            if (paths == null || !paths.Any())
            {
                throw new ArgumentException(nameof(paths));
            }
            for (int i = 0; i < paths.Length; i++)
            {
                if (i == 0)
                {
                    // for the first arg, strip only the trailing slashes
                    paths[i] = paths[i].SwitchBackSlashToForwardSlash().StripTrailingSlashesAll().Trim();
                }
                else if (i == paths.Length - 1)
                {
                    // for the last arg, strip only the trailing slashes
                    paths[i] = paths[i].SwitchBackSlashToForwardSlash().StripLeadingSlashesAll().Trim();
                }
                else
                {
                    // otherwise strip all leading and trailing slashes
                    paths[i] = paths[i].SwitchBackSlashToForwardSlash().StripAllLeadingAndTrailingSlashes().Trim();
                }
            }
            return string.Join("/", paths);
        }

        public static string CreateCsv(IEnumerable<string> values)
        {
            return CreateCsv(values, ",");
        }

        public static string CreateCsv(IEnumerable<string> values, string separator, string encapsulator = "\"", bool alwaysEncapsulate = false)
        {
            if (values == null || !values.Any())
            {
                return null;
            }

            if (string.IsNullOrEmpty(separator))
            {
                separator = ",";
            }

            var encapsulate = alwaysEncapsulate;
            var sb = new StringBuilder();
            string valueSep = string.Empty;
            foreach (var strValue in values)
            {
                // if contained, must encapsulate
                string writeValue = strValue;
                sb.Append(valueSep);
                if (writeValue.Contains(separator) || writeValue.Contains(encapsulator))
                {
                    encapsulate = true;
                }
                if (encapsulate)
                {
                    sb.Append(encapsulator);
                    //Escape the encapsulators
                    writeValue = strValue.Replace(encapsulator, encapsulator + encapsulator);
                    sb.Append(strValue);
                    sb.Append(encapsulator);
                }
                else
                {
                    sb.Append(strValue);
                }
                valueSep = separator;
            }
            return sb.ToString();
        }

        public static string[] ParseCsvRow(string input)
        {
            return ParseSeparatedValuesRow(input, ",", true);
        }

        public static string[] ParseSeparatedValuesRow(string input, string separator)
        {
            return ParseSeparatedValuesRow(input, separator, true);
        }
        
        public static string[] ParseSeparatedValuesRow(string input, string separator, bool trim)
        {
            if (string.IsNullOrEmpty(separator))
            {
                separator = ",";
            }

            string separatorRegex = Regex.Escape(separator);

            var pattern = "(?<field>" + separatorRegex + ")|((?<field>[^\"" + separatorRegex + "\\r\\n]+)|\"(?<field>([^\"]|\"\")+)\")(" + separatorRegex + "|(?<rowbreak>\\r\\n|\\n|$))";
            Regex re = new Regex(pattern);
            string field = "";
            MatchCollection mc = re.Matches(input);
            var stc = new StringCollection();
            int iCol = 1;
            foreach (Match m in mc)
            {
                // retrieve the field and replace two double-quotes with a single double-quote 
                field = m.Result("${field}");

                // clear any that are just the separator value
                if (field == separator)
                {
                    field = string.Empty;
                }

                // do this only after checking for if it equals the separator
                field = field.Replace("\"\"", "\"");

                if (trim && !string.IsNullOrEmpty(field))
                {
                    stc.Add(field.Trim());
                }
                else
                {
                    stc.Add(string.Empty);
                }
                iCol += 1;
            }

            //Cover a blank last column
            if (input.Right(separator.Length) == separator | input.Right(separator.Length + 2) == separator + "\"\"")
            {
                stc.Add(string.Empty);
            }

            string[] s = new string[stc.Count];
            stc.CopyTo(s, 0);
            return s;

        }

    }
}
