
namespace Sdk.Common.Extensions
{
    using System;

    public static class IntegerExtensions
    {
        /// <summary>
        /// Pads a string using 0's to the desired length.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string PadToLength(this in int helper, int length)
        {
            var s = helper.ToString();
            var l = s.Length;
            if (l > length)
            {
                throw new FormatException($"Integer '{s}' is longer than pad length of '{length}'");
            }
            else if (l == length)
            {
                return s;
            }
            else
            {
                // calculate the amount of padding needed and output
                var needed = length - l;
                return s.PadLeft(needed, '0');
            }
        }

        /// <summary>
        /// Returns a default value if the int is negative or zero.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int DefaultIfIsNegativeOrZero(this int value, int defaultValue)
        {
            if (value <= 0)
            {
                return defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Creates a verbal word from a number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetVerbalWord(this in int number)
        {
            if (number == 0)
                return _smallNumbers[0];

            // Array to hold four three-digit groups
            int[] digitGroups = new int[4];

            // Ensure a positive number to extract from
            int positive = Math.Abs(number);

            // Extract the three-digit groups
            for (int i = 0; i < 4; i++)
            {
                digitGroups[i] = positive % 1000;
                positive /= 1000;
            }

            // Convert each three-digit group to words
            string[] groupText = new string[4];

            for (int i = 0; i < 4; i++)
            {
                groupText[i] = ThreeDigitGroupToWords(digitGroups[i]);
            }

            // Recombine the three-digit groups
            string combined = groupText[0];
            bool appendAnd;

            // Determine whether an 'and' is needed
            appendAnd = (digitGroups[0] > 0) && (digitGroups[0] < 100);

            // Process the remaining groups in turn, smallest to largest
            for (int i = 1; i < 4; i++)
            {
                // Only add non-zero items
                if (digitGroups[i] != 0)
                {
                    // Build the string to add as a prefix
                    string prefix = groupText[i] + " " + _scaleNumbers[i];

                    if (combined.Length != 0)
                    {
                        prefix += appendAnd ? " and " : ", ";
                    }

                    // Opportunity to add 'and' is ended
                    appendAnd = false;

                    // Add the three-digit group to the combined string
                    combined = prefix + combined;
                }
            }

            return combined;
        }

        #region supporting functions for GetVerbalWord

        // Single-digit and small number names
        private static readonly string[] _smallNumbers = new string[]
            { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
              "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen",
              "Eighteen", "Nineteen"};

        // Tens number names from twenty upwards
        private static readonly string[] _tens = new string[]
            { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"};

        // Scale number names for use during recombination
        private static readonly string[] _scaleNumbers = new string[] { "", "Thousand", "Million", "Billion" };

        // Converts a three-digit group into English words
        private static string ThreeDigitGroupToWords(int threeDigits)
        {
            // Initialise the return text
            string groupText = "";

            // Determine the hundreds and the remainder
            int hundreds = threeDigits / 100;
            int tensUnits = threeDigits % 100;

            // Hundreds rules
            if (hundreds != 0)
            {
                groupText += _smallNumbers[hundreds] + " Hundred";

                if (tensUnits != 0)
                {
                    groupText += " and ";
                }
            }

            // Determine the tens and units
            int tens = tensUnits / 10;
            int units = tensUnits % 10;

            // Tens rules
            if (tens >= 2)
            {
                groupText += _tens[tens];
                if (units != 0)
                {
                    groupText += " " + _smallNumbers[units];
                }
            }
            else if (tensUnits != 0)
                groupText += _smallNumbers[tensUnits];

            return groupText;
        }

        #endregion

    }
}
