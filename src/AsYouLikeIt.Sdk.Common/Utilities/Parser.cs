namespace AsYouLikeIt.Sdk.Common.Utilities
{
    using AsYouLikeIt.Sdk.Common.Extensions;
    using System;
    using System.Globalization;

    public partial class Parser
    {

        public static bool TryParseValue<T>(string preParseRegex, object input, out T value)
        {
            return ToObject<T>(preParseRegex, input, null, out value);
        }

        public static bool TryParseValue<T>(string preParseRegex, object input, NumberStyles? numberStyles, out T value)
        {
            return ToObject<T>(preParseRegex, input, numberStyles, out value);
        }

        public static bool ToObject(string preParseRegex, object input, Type type, out object output)
        {
            return ToObject(preParseRegex, input, null, type, out output);
        }

        public static bool ToObject(string preParseRegex, object input, NumberStyles? numberStyles, Type type, out object output)
        {
            object returnObj = null;
            try
            {
                returnObj = ToObject(preParseRegex, input, numberStyles, type);
                // regular cast 
                output = returnObj;
                return true;
            }
            catch (FormatException)
            {
                // invalid format is a false and error
            }
            if (returnObj == null && (!type.IsValueType || Nullable.GetUnderlyingType(type) == null))
            {
                // this is a non-nullable type            
                output = Activator.CreateInstance(type);
                return false;
            }
            output = null;
            return false;
        }

        #region helpers

        private static bool ToObject<T>(string preParseRegex, object input, NumberStyles? numberStyles, out T output)
        {
            try
            {
                var type = typeof(T);
                var returnObj = ToObject(preParseRegex, input, numberStyles, type);
                if (returnObj == null && (!type.IsValueType || Nullable.GetUnderlyingType(type) == null))
                {
                    // this is a non-nullable type
                    output = default(T);
                    return false;
                }

                // regular cast 
                output = (T)returnObj;
                return true;
            }
            catch (FormatException)
            {
                // invalid format is a false and error
                output = default(T);
                return false;
            }
        }

        private static object ToObject(string preParseRegex, object input, NumberStyles? numberStyles, Type objectType)
        {
            string asString = null;
            if (input != null)
            {
                asString = input.ToString();
                if (!string.IsNullOrEmpty(preParseRegex))
                {
                    if (!RegExPatterns.IsValidRegex(preParseRegex))
                    {
                        var configurationError = $"RegEx pattern '{preParseRegex}' is not a valid RegEx pattern.";
                        throw new FormatException(configurationError);
                    }
                    // strip matching regex patterns
                    asString = asString.RegexReplace(preParseRegex);
                }
                asString = asString.Trim();
            }

            // "string", "boolean", "byte", "int", "decimal", "date", "datetime"
            if (objectType == typeof(string))
            {
                return asString;
            }
            else if (objectType == typeof(bool) || objectType == typeof(bool?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    bool? nBool = null;
                    return nBool;
                }
                if (asString == "0")
                {
                    return false;
                }
                else if (asString == "1")
                {
                    return true;
                }
                else
                {
                    bool thisBool;
                    var isValidBool = bool.TryParse(asString, out thisBool);
                    if (!isValidBool)
                    {
                        throw new FormatException("Invalid bool");
                    }
                    return thisBool;
                }
            }
            else if (objectType == typeof(byte) || objectType == typeof(byte?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    byte? nByte = null;
                    return nByte;
                }
                else
                {
                    byte thisByte;
                    bool isValid;
                    if (numberStyles == null)
                    {
                        isValid = byte.TryParse(asString, out thisByte);
                    }
                    else
                    {
                        isValid = byte.TryParse(asString, numberStyles.Value, CultureInfo.InvariantCulture, out thisByte);
                    }
                    if (!isValid)
                    {
                        throw new FormatException("Invalid byte");
                    }
                    return thisByte;
                }
            }
            else if (objectType == typeof(int) || objectType == typeof(int?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    int? nInt = null;
                    return nInt;
                }
                else
                {
                    int thisInt;
                    bool isValid;
                    if (numberStyles == null)
                    {
                        isValid = int.TryParse(asString, out thisInt);
                    }
                    else
                    {
                        isValid = int.TryParse(asString, numberStyles.Value, CultureInfo.InvariantCulture, out thisInt);
                    }
                    if (!isValid)
                    {
                        throw new FormatException("Invalid int");
                    }
                    return thisInt;
                }
            }
            else if (objectType == typeof(long) || objectType == typeof(long?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    long? nLong = null;
                    return nLong;
                }
                else
                {
                    long thisLong;
                    bool isValid;
                    if (numberStyles == null)
                    {
                        isValid = long.TryParse(asString, out thisLong);
                    }
                    else
                    {
                        isValid = long.TryParse(asString, numberStyles.Value, CultureInfo.InvariantCulture, out thisLong);
                    }
                    if (!isValid)
                    {
                        throw new FormatException("Invalid long");
                    }
                    return thisLong;
                }
            }
            else if (objectType == typeof(decimal) || objectType == typeof(decimal?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    decimal? nDecimal = null;
                    return nDecimal;
                }
                else
                {
                    decimal thisDecimal;
                    bool isValid;
                    if (numberStyles == null)
                    {
                        isValid = decimal.TryParse(asString, out thisDecimal);
                    }
                    else
                    {
                        isValid = decimal.TryParse(asString, numberStyles.Value, CultureInfo.InvariantCulture, out thisDecimal);
                    }
                    if (!isValid)
                    {
                        throw new FormatException("Invalid decimal");
                    }
                    return thisDecimal;
                }
            }
            else if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    DateTime? nDateTime = null;
                    return nDateTime;
                }
                else
                {
                    DateTime thisDateTime;
                    var numericOnlyAsString = asString.StripNonNumeric();
                    if (numericOnlyAsString.Length == 8)
                    {
                        try
                        {
                            var year = int.Parse(asString.Substring(0, 4));
                            if (year >= 1753) // use the min SQL DATETIME year
                            {
                                var month = int.Parse(asString.Substring(4, 2));
                                if (month >= 1 && month <= 12)
                                {
                                    var day = int.Parse(asString.Substring(6, 2));
                                    if (day >= 1 && day <= 31)
                                    {
                                        thisDateTime = new DateTime(year, month, day);
                                        return thisDateTime;
                                    }
                                }
                            }
                        }
                        catch { } // let an exception go
                    }
                    var isValid = DateTime.TryParse(asString, out thisDateTime);
                    if (!isValid)
                    {
                        throw new FormatException("Invalid DateTime");
                    }
                    return thisDateTime;
                }
            }
            else if (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    DateTimeOffset? nDateTime = null;
                    return nDateTime;
                }
                else
                {
                    DateTimeOffset thisDateTime;
                    var isValid = DateTimeOffset.TryParse(asString, out thisDateTime);
                    if (!isValid)
                    {
                        throw new FormatException("Invalid DateTimeOffset");
                    }
                    return thisDateTime;
                }
            }
            else if (objectType == typeof(Guid) || objectType == typeof(Guid?))
            {
                if (string.IsNullOrEmpty(asString))
                {
                    Guid? nGuid = null;
                    return nGuid;
                }
                else
                {
                    Guid thisGuid;
                    var isValid = Guid.TryParse(asString, out thisGuid);
                    if (!isValid)
                    {
                        throw new FormatException("Invalid Guid");
                    }
                    return thisGuid;
                }
            }
            throw new NotImplementedException(string.Format("Type '{0}' is not implemented.", objectType.FullName));
        }

        #endregion

    }
}