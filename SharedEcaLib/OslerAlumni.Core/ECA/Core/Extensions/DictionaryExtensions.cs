using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryGetKeyByOrdinalValue<T>(
            this Dictionary<T, string> dict,
            string value,
            out T key)
        {
            key = default(T);

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var foundItem = dict
                .FirstOrDefault(kvp => string.Equals(kvp.Value, value, StringComparison.OrdinalIgnoreCase));

            if (foundItem.Equals(default(KeyValuePair<T, string>)))
            {
                return false;
            }

            key = foundItem.Key;

            return true;
        }

        public static bool TryGetValueByOrdinalKey<T>(
            this Dictionary<string, T> dict,
            string key,
            out T value)
        {
            value = default(T);

            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            key = dict.Keys
                .GetItemOrdinalOrDefault(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            value = dict[key];

            return true;
        }
    }
}
