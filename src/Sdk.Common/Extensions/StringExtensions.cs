namespace Sdk.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Utilities;

    public static class StringExtensions
    {
        /// <summary>
		/// Takes a string and returns a list of GUIDs using the split character provided. Only valid guids will return. 
		/// Will return an empty list if the string does not contain any valid GUIDs. 
		/// To support legacy behavior if the split character is ";" then it will match both "," and ";" if you do not
		/// want this behavior use the override List(Guid) Guidify(this string value, params char[] seperator)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="splitCharacter"></param>
		/// <returns>An emptry List(Guid) if none are found.</returns>
		public static List<Guid> Guidify(this string value, string splitCharacter = ";")
        {
            var seperator = new char[] { char.Parse(splitCharacter) };

            // To support legacy behavior if the split char is ";" then we also match ","
            if (splitCharacter == ";")
            {
                seperator = new char[] { ',', ';' };
            }

            return value.Guidify(seperator);
        }

        /// <summary>
        /// Takes a string and returns a list of GUIDs using the split character provided. Only valid guids will return. 
        /// Will return an empty list if the string does not contain any valid GUIDs.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="seperator">List of seperator character to split the string on</param>
        /// <returns>NULL if no guids are present or a list of guids.</returns>
        public static List<Guid> Guidify(this string value, params char[] seperator)
        {
            var list = new List<Guid>(); // no checking for nulls
            if (!string.IsNullOrWhiteSpace(value))
            {
                var guids = value.SplitStringAndTrim(seperator);
                if (guids != null && guids.Any())
                {
                    foreach (var g in guids)
                    {
                        if (Guid.TryParse(g, out Guid guid))
                        {
                            list.Add(guid);
                        }
                    }
                    return list;
                }
            }
            return list;
        }


        public static string ToCsvWithSpace(this IEnumerable<string> helper)
        {
            if (helper == null || !helper.Any())
            {
                return string.Empty;
            }
            var output = string.Join(", ", helper);
            return output;
        }


        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }


        public static Stream GenerateStreamFromString(this string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static bool IsGuid(this string expression)
        {
            return Guid.TryParse(expression, out Guid guid);
        }

        public static string MakeFileNameSafe(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            //Replace spaces with dashes
            if (input.Contains(" "))
            {
                input = input.ReplaceCaseInsensitive(" ", "-");
                input = input.Trim('-');
            }
            input = input.RemoveAccents();
            //Strip anything else that won't work
            input = (input.StripNonAlphaNumericDashUnderscorePeriod());
            return input;
        }
        public static string MakeBlobNameSafe(this string input, bool makeLower = false)
        {
            var blobName = input;
            if (blobName == null)
            {
                throw new ArgumentNullException(nameof(blobName));
            }
            blobName = blobName.Trim().SwitchBackSlashToForwardSlash();
            var output = blobName.StripAllLeadingAndTrailingSlashes();
            var sections = output.SplitStringAndTrim("/");
            //bool blobNameStartsWithSlash = blobName.StartsWith("/");
            //bool blobNameEndsWithSlash = blobName.EndsWith("/");
            string parsed = "";
            var divider = "";
            foreach (var section in sections)
            {
                if (makeLower)
                {
                    parsed += divider + section.ToLowerInvariant().MakeFileNameSafe();
                }
                else
                {
                    parsed += divider + section.MakeFileNameSafe();
                }
                divider = "/";
            }
            if (string.IsNullOrEmpty(blobName))
            {
                throw new ArgumentNullException("The file path is not valid. Input: " + input);
            }
            output = parsed;
            return output;
        }


        /// <summary>
        /// Replaces text in a string using Regular Expressions
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <param name="EscapePattern"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ReplaceCaseInsensitive(this string input, string pattern, string replacement, bool EscapePattern = true)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            if (EscapePattern)
            {
                pattern = Regex.Escape(pattern);
            }
            // This is required for replacement text that has $ signs
            replacement = Regex.Replace(replacement, Regex.Escape("$"), "$$$$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string strRet = Regex.Replace(input, pattern, replacement, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return strRet;
        }

        public static string ReplaceFirst(this string input, string pattern, string replacement)
        {
            var regex = new Regex(Regex.Escape(pattern));
            var newText = regex.Replace(input, replacement, 1);
            return newText;
        }

        /// <summary>
        /// Removes most common accent characters and replaces with corresponding regular characters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string RemoveAccents(this string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            //NOTE: This mirrors a SQL function
            //Handle the foreign characters
            input = Regex.Replace(input, "[àâäåãáæ]", "a", RegexOptions.Compiled);
            input = Regex.Replace(input, "[ÂÄÀÅÃÁÆ]", "A", RegexOptions.Compiled);

            input = Regex.Replace(input, "[éèêë]", "e", RegexOptions.Compiled);
            input = Regex.Replace(input, "[ÉÊËÈ]", "E", RegexOptions.Compiled);

            input = Regex.Replace(input, "[ïîìí]", "i", RegexOptions.Compiled);
            input = Regex.Replace(input, "[ÏÎÌÍ]", "I", RegexOptions.Compiled);

            input = Regex.Replace(input, "[öôóòõœ]", "o", RegexOptions.Compiled);
            input = Regex.Replace(input, "[ÓÔÖÒÕŒ]", "O", RegexOptions.Compiled);

            input = Regex.Replace(input, "[ùûüú]", "o", RegexOptions.Compiled);
            input = Regex.Replace(input, "[ÜÛÙÚ]", "U", RegexOptions.Compiled);

            input = Regex.Replace(input, "[ç]", "c", RegexOptions.Compiled);
            input = Regex.Replace(input, "[Ç]", "C", RegexOptions.Compiled);

            input = Regex.Replace(input, "[ñ]", "n", RegexOptions.Compiled);
            input = Regex.Replace(input, "[Ñ]", "N", RegexOptions.Compiled);

            input = Regex.Replace(input, "[ÿ]", "y", RegexOptions.Compiled);
            input = Regex.Replace(input, "[Ÿ]", "Y", RegexOptions.Compiled);

            return input;

        }


        public static string SetFixedLengthWithSpaces(this string input, int length)
        {
            string output = input;
            if (input == null)
            {
                input = string.Empty;
            }
            if (input.Length > length)
            {
                throw new InvalidOperationException(string.Format("Value '{0}' is longer than length '{1}'.", input, length.ToString()));
            }
            int add = length - input.Length;
            for (int i = 0; i < add; i++)
            {
                input += " ";
            }
            return input;
        }

        public static string CapFirst(this string helper)
        {
            helper = helper.ToLower(); // necessary or will not work; expected confusing c# behavior
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var treated = textInfo.ToTitleCase(helper);
            // common address replacement
            treated = treated.Replace("Po Box", "PO Box");
            return treated;
        }


        public static bool IsValidKeyStrict(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (maxLength < 3)
            {
                maxLength = 3;
            }
            if (input.Length > maxLength)
            {
                return false;
            }
            var middleLength = maxLength - 1;
            var pattern = "^[A-Za-z][A-Za-z0-9]{2," + middleLength.ToString() + "}$";
            Match match = Regex.Match(input, pattern);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsValidKeyStrictWithUnderscores(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (maxLength < 3)
            {
                maxLength = 3;
            }
            if (input.Length > maxLength)
            {
                return false;
            }
            var pattern = RegExPatterns.AlphaNumericStartAndEndNonConsecutiveUnderscores;
            Match match = Regex.Match(input, pattern);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsValidKeyStrictWithDashes(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (maxLength < 3)
            {
                maxLength = 3;
            }
            if (input.Length > maxLength)
            {
                return false;
            }
            var pattern = RegExPatterns.AlphaNumericStartAndEndNonConsecutiveDashes;
            Match match = Regex.Match(input, pattern);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsValidKeyStrictWithDashesUnderscores(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (maxLength < 3)
            {
                maxLength = 3;
            }
            if (input.Length > maxLength)
            {
                return false;
            }
            var pattern = RegExPatterns.AlphaNumericStartAndEndNonConsecutiveDashesUnderscores;
            Match match = Regex.Match(input, pattern);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsValidKeyStrictLowerCaseOnly(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            if (maxLength < 3)
            {
                maxLength = 3;
            }
            var pattern = "^[a-z][a-z0-9]{2," + maxLength.ToString() + "}$";
            Match match = Regex.Match(input, pattern);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static bool IsValidKey(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            Match match = Regex.Match(input, RegExPatterns.AlphaNumericDashesUnderscoresPeriods);
            return ((match.Success && (match.Index == 0)) && (match.Length == input.Length));
        }

        public static string ToValidKey(this string input)
        {
            if (input == null)
            {
                return null;
            }
            return (input.Trim().Replace(" ", "-")).Trim().StripNonAlphaNumericDashUnderscorePeriod().Replace("---", "-").Replace("--", "-").Trim('-').Trim('_');
        }

        public static string EnsureNullIfEmptyAndTrim(this string text)
        {
            if (text == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            return text.Trim();
        }

        public static string Left(this string text, int length)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(0, length);
        }

        public static string Right(this string text, int length)
        {

            if (string.IsNullOrEmpty(text))
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(text.Length - length);
        }

        /// <summary>
        /// Returns a non-null left trimmed string value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string LeftAndNotNothing(this string input, int length)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return input.Left(length);
        }

        public static string PadWithZeros(this string input, int totalDigits)
        {
            input += string.Empty;
            if (input.Length < totalDigits)
            {
                input = PadWithZeros("0" + input, totalDigits);
            }
            return input;
        }

        /// <summary>
        /// Returns a string that is followed with X number of spaces
        /// </summary>
        /// <param name="input"></param>
        /// <param name="totalDigits"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PadRightWithSpaces(this string input, int totalDigits)
        {
            input += string.Empty;
            if (input.Length < totalDigits)
            {
                input = PadRightWithSpaces(input + " ", totalDigits);
            }
            return input;
        }


        /// <summary>
        /// Returns a string that is followed with X number of spaces
        /// </summary>
        /// <param name="input"></param>
        /// <param name="totalDigits"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string PadLeftWithSpaces(this string input, int totalDigits)
        {
            input += string.Empty;
            if (input.Length < totalDigits)
            {
                input = PadLeftWithSpaces(" " + input, totalDigits);
            }
            return input;
        }

        /// <summary>
        /// Returns an empty string if the input is null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string EmptyIfNull(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                return input;
            }
        }

        public static bool EqualsCaseInsensitive(this string helper, string compareTo)
        {
            return string.Equals(helper, compareTo, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsCaseInsensitive(this string helper, string pattern)
        {
            Match match = Regex.Match(helper, pattern, RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static bool HasValueCaseInsensitive(this IEnumerable<string> helper, string pattern)
        {
            foreach (var s in helper)
            {
                if (s == null)
                {
                    continue;
                }
                var patternEscaped = Regex.Escape(pattern);
                var fullMatch = Format.IsFullMatchCaseInsensitive(s, patternEscaped);
                if (fullMatch)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasRegexFullMatchCaseInsensitive(this IEnumerable<string> patterns, string value)
        {
            foreach (var pattern in patterns)
            {
                if (pattern == null)
                {
                    continue;
                }
                var fullMatch = Format.IsFullMatchCaseInsensitive(value, pattern);
                if (fullMatch)
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<string> SplitStringAndTrim(this string helper, string splitCharacter)
        {
            char splitOn = char.Parse(splitCharacter);
            return helper.SplitStringAndTrim(splitOn);
        }

        public static IEnumerable<string> SplitStringAndTrim(this string helper, params char[] seperator)
        {
            //Get string coll of items
            string[] values = null;
            values = helper.Split(seperator);
            var list = new List<string>();
            foreach (var s in values)
            {
                var thisVal = s.Trim();
                if (!string.IsNullOrWhiteSpace(thisVal))
                {
                    list.Add(thisVal);
                }
            }
            return list;
        }

        public static string TitleCase(this string helper)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(helper);
        }

        /// <summary>
        /// Turn a string to camelCase
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string helper)
        {
            if (helper == null)
            {
                return null;
            }
            helper = helper.Replace(" ", "_");
            return Char.ToLowerInvariant(helper[0]) + helper.Substring(1).Replace("_", string.Empty);
        }

        public static string Encrypt(this string helper)
        {
            if (string.IsNullOrWhiteSpace(helper))
            {
                return helper; // no changes
            }
            if (Globals.StringEncryptorDelegate == null)
            {
                throw new InvalidProgramException($"Must define {nameof(Globals.StringEncryptorDelegate)}.");
            }
            else
            {
                return Globals.StringEncryptorDelegate(helper);
            }
        }

        public static string Decrypt(this string helper)
        {
            if (string.IsNullOrWhiteSpace(helper))
            {
                return helper; // no changes
            }
            if (Globals.StringDecryptorDelegate == null)
            {
                throw new InvalidProgramException($"Must define {nameof(Globals.StringEncryptorDelegate)}.");
            }
            else
            {
                return Globals.StringEncryptorDelegate(helper);
            }
        }

        /// <summary>
        /// Removes anything other than 0-9 a-z or A-Z
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string StripNonAlphaNumeric(this string input)
        {

            if (input == null)
            {
                return string.Empty;
            }

            string removeInvalidChars = RegExPatterns.StripNonAlphaNumeric;
            input = Regex.Replace(input, removeInvalidChars, string.Empty);
            return input;

        }

        public static string StripNonNumeric(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            string removeInvalidChars = RegExPatterns.Numbers;
            input = Regex.Replace(input, removeInvalidChars, string.Empty);
            return input;
        }

        public static string StripNonNumericExceptPeriods(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            string removeInvalidChars = RegExPatterns.NumbersAndPeriods;
            input = Regex.Replace(input, removeInvalidChars, string.Empty);
            return input;
        }

        public static string StripArraySpecification(string input)
        {
            var regEx = @"\[(.*?)\]";
            var replaced = Regex.Replace(input, regEx, string.Empty);
            return replaced;
        }

        public static string RegexReplace(this string input, string regExPattern)
        {
            if (input == null)
            {
                return string.Empty;
            }
            string removeInvalidChars = regExPattern;
            input = Regex.Replace(input, removeInvalidChars, string.Empty);
            return input;
        }


        public static string StripNonAlphaNumericDashUnderscore(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            string removeInvalidChars = "[^a-zA-Z0-9_\\-]"; //RegExPatterns.AlphaNumericDashesUnderscores; //"[a-zA-Z0-9_\\-]+"
            input = Regex.Replace(input, removeInvalidChars, string.Empty);
            return input;
        }

        public static string StripNonAlphaNumericDashUnderscorePeriod(this string input)
        {
            if (input == null)
            {
                return string.Empty;
            }
            string removeInvalidChars = "[^a-zA-Z0-9_\\-\\.]";
            input = Regex.Replace(input, removeInvalidChars, string.Empty);
            return input;
        }

        public static string SwitchBackSlashToForwardSlash(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return input.Replace(@"\", @"/");
        }

        public static string SwitchForwardSlashToBackSlash(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return input.Replace(@"/", @"\");
        }


        public static string StripTrailingSlash(this string input)
        {

            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            string strPatternExp = "[/]$";
            return Regex.Replace(input, strPatternExp, "");
        }


        public static string StripLeadingSlash(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = "^[/]";
            return Regex.Replace(input, strPatternExp, "");
        }

        public static string StripLeadingSlashesAll(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = "^(/+)"; //"^[/]";
            return Regex.Replace(input, strPatternExp, "");
        }


        /// <summary>
        /// Removes all content past the last forward slash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripTrailingSlashAndSubsequentText(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = "(/)[^/]*$";
            return Regex.Replace(input, strPatternExp, "");
        }


        public static string StripAllLeadingAndTrailingSlashes(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = "(^(/+))|((/+)$)";
            return Regex.Replace(input, strPatternExp, "");
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Reverse(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            char[] rev = input.ToCharArray();
            Array.Reverse(rev);
            return (new string(rev));
        }

        /// <summary>
        /// Strips the file extension from a string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string StripFileExtension(this string input)
        {

            if (string.IsNullOrEmpty(input) || !input.Contains("."))
            {
                return input;
            }
            string strPatternExp = "(.)[^.]*$";
            return Regex.Replace(input, strPatternExp, "");

        }

        public static string GetFileExtension(this string input)
        {
            string ext = null;
            var extPos = input.LastIndexOf(".");
            if (extPos > 2)
            {
                ext = input.Substring(extPos);
            }
            return ext;
        }

        /// <summary>
        /// Strips the file extension from a string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string StripFileExtension(this string input, string[] extensionsToStrip)
        {

            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            string matchPattern = "";
            string sep = "";
            foreach (string ext in extensionsToStrip)
            {
                matchPattern += sep + ext;
                sep = "|";
            }

            string strPatternExp = "(" + matchPattern + ")$";
            return Regex.Replace(input, strPatternExp, "");

        }

        public static string StripTrailingSlashesAll(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = "(/+)$";
            return Regex.Replace(input, strPatternExp, "");
        }

        public static string StripTrailingBackslashesAll(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = @"(\\+)$";
            return Regex.Replace(input, strPatternExp, "");
        }

        public static string StripLeadingBackslashesAll(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string strPatternExp = @"^(\\+)";
            return Regex.Replace(input, strPatternExp, "");
        }

        /// <summary>
        /// Uses a regular expression to look for a word boundary around the match pattern
        /// </summary>
        /// <param name="matchPattern"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ContainsWord(this string searchInput, string matchPattern, bool ignoreCase)
        {
            string strExp = "\\b" + Regex.Escape(matchPattern) + "\\b";
            Regex exp = default(Regex);
            if (ignoreCase)
            {
                exp = new Regex(strExp, RegexOptions.IgnoreCase);
            }
            else
            {
                exp = new Regex(strExp);
            }
            if (exp.IsMatch(searchInput))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static readonly HashSet<char> DefaultNonWordCharacters = new HashSet<char> { ',', '.', ':', ';' };

        /// <summary>
		/// Returns a substring from the start of <paramref name="value"/> no 
		/// longer than <paramref name="length"/>.
		/// Returning only whole words is favored over returning a string that 
		/// is exactly <paramref name="length"/> long. 
		/// </summary>
		/// <param name="value">The original string from which the substring 
		/// will be returned.</param>
		/// <param name="length">The maximum length of the substring.</param>
		/// <param name="nonWordCharacters">Characters that, while not whitespace, 
		/// are not considered part of words and therefor can be removed from a 
		/// word in the end of the returned value. 
		/// Defaults to ",", ".", ":" and ";" if null.</param>
		/// <exception cref="System.ArgumentException">
		/// Thrown when <paramref name="length"/> is negative
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown when <paramref name="value"/> is null
		/// </exception>
		public static string LeftWithoutBreakingWords(
          this string value,
          int length,
          HashSet<char> nonWordCharacters = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (length < 0)
            {
                throw new ArgumentException("Negative values not allowed.", "length");
            }

            if (nonWordCharacters == null)
            {
                nonWordCharacters = DefaultNonWordCharacters;
            }

            if (length >= value.Length)
            {
                return value;
            }
            int end = length;

            for (int i = end; i > 0; i--)
            {
                if (value[i].IsWhitespace())
                {
                    break;
                }

                if (nonWordCharacters.Contains(value[i])
                    && (value.Length == i + 1 || value[i + 1] == ' '))
                {
                    //Removing a character that isn't whitespace but not part 
                    //of the word either (ie ".") given that the character is 
                    //followed by whitespace or the end of the string makes it
                    //possible to include the word, so we do that.
                    break;
                }
                end--;
            }

            if (end == 0)
            {
                //If the first word is longer than the length we favor 
                //returning it as cropped over returning nothing at all.
                end = length;
            }

            return value.Substring(0, end);
        }


        /// <summary>
        /// Returns a substring from the start of <paramref name="value"/> no 
        /// longer than <paramref name="length"/>.
        /// Returning only whole words is favored over returning a string that 
        /// is exactly <paramref name="length"/> long. 
        /// </summary>
        /// <param name="value">The original string from which the substring 
        /// will be returned.</param>
        /// <param name="length">The maximum length of the substring.</param>
        /// <param name="addEllipse">Adds an ellipse to the end if the string is trimmed.</param>
        /// <param name="nonWordCharacters">Characters that, while not whitespace, 
        /// are not considered part of words and therefor can be removed from a 
        /// word in the end of the returned value. 
        /// Defaults to ",", ".", ":" and ";" if null.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when <paramref name="length"/> is negative
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="value"/> is null
        /// </exception>
        public static string LeftWithoutBreakingWords(
          this string value,
          int length,
          bool addEllipse,
          HashSet<char> nonWordCharacters = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (length < 0)
            {
                throw new ArgumentException("Negative values not allowed.", "length");
            }

            if (nonWordCharacters == null)
            {
                nonWordCharacters = DefaultNonWordCharacters;
            }

            if (length >= value.Length)
            {
                return value;
            }
            int end = length;

            for (int i = end; i > 0; i--)
            {
                if (value[i].IsWhitespace())
                {
                    break;
                }

                if (nonWordCharacters.Contains(value[i])
                    && (value.Length == i + 1 || value[i + 1] == ' '))
                {
                    //Removing a character that isn't whitespace but not part 
                    //of the word either (ie ".") given that the character is 
                    //followed by whitespace or the end of the string makes it
                    //possible to include the word, so we do that.
                    break;
                }
                end--;
            }

            if (end == 0)
            {
                //If the first word is longer than the length we favor 
                //returning it as cropped over returning nothing at all.
                end = length;
            }

            if (addEllipse)
            {
                return value.Substring(0, end) + "...";
            }
            else
            {
                return value.Substring(0, end);
            }
        }

        private static bool IsWhitespace(this char character)
        {
            return character == ' ' || character == 'n' || character == 't';
        }

        public static string TruncateToWordWithEllipses(this string input, int maxChars)
        {
            if (input == null)
            {
                return null;
            }

            if (input.Length <= maxChars)
            {
                return input;
            }

            var list = Regex.Split(input, @"\s+");
            var outputList = new List<string>();

            foreach (var word in list)
            {
                if (outputList.SelectMany(s => s).Count()           // total chars
                    + (!outputList.Any() ? 0 : outputList.Count)  // account for spaces
                    + word.Length                                   // incoming word to add
                    + 3                                             // ellipses
                    > maxChars)
                {
                    break;
                }

                outputList.Add(word);
            }

            return $"{string.Join(" ", outputList)}...";
        }

    }
}
