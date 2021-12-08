using System;

namespace ECA.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Checks if the DateTime value is later than the given value, within a specified error margin.
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="seconds">Error margin in seconds.</param>
        /// <returns></returns>
        public static bool IsLaterThan(
            this DateTime dt1,
            DateTime? dt2,
            int seconds = 0)
        {
            if (!dt2.HasValue)
            {
                return false;
            }

            return (dt1 - dt2.Value).TotalSeconds > seconds;
        }
    }
}
