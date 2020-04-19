namespace System
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Returns the format hh:mm:ss as a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetTimeSpanToString(this TimeSpan input)
        {
            return string.Format("{0:hh\\:mm\\:ss}", input);
        }

        public static string GetTimeSpanToLongString(this TimeSpan input)
        {
            return string.Format("{0:hh\\:mm\\:ss\\:ff}", input);
        }
    }
}
