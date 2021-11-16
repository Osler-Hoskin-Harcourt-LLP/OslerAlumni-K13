using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECA.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Based on https://stackoverflow.com/a/9210493.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsDictionary(
            this object obj)
        {
            return obj?
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(
                    p => p.Name,
                    p => p.GetValue(obj));
        }

        public static object ChangeType(
            this object obj,
            Type targetType)
        {
            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, (obj ?? 0).ToString(), true);
            }

            if (targetType == typeof(Guid))
            {
                return (obj == null)
                    ? Guid.Empty
                    : new Guid(obj.ToString());
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return (DateTimeOffset?)obj ?? new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);
            }

            return Convert.ChangeType(obj, targetType);
        }
    }
}
