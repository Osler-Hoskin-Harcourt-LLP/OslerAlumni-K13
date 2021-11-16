using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Core.Extensions
{
    public static class IEnumerableOfTExtensions
    {
        public static bool ContainsCaseInsensitive(this IEnumerable<string> source, string item)
        {
            return source.GetItemOrdinalOrDefault(item) != null;
        }

        public static string GetItemOrdinalOrDefault(
            this IEnumerable<string> source,
            string item)
        {
            if ((source == null) || (string.IsNullOrWhiteSpace(item)))
            {
                return null;
            }

            return source
                .FirstOrDefault(i => string.Equals(i, item, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<T> WhereNotNull<T>(
            this IEnumerable<T> source
        )
        {
            return source.Where(item => item != null);
        }
    }
}
