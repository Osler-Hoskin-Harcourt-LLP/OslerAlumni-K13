using System;
using ECA.Core.Definitions;

namespace ECA.Core.Extensions
{
    public static class TypeExtensions
    {
        public static string GetPropertyName(
            this Type type,
            string propertyName,
            NameSource propertyNameSource)
        {
            if ((type == null)
                || string.IsNullOrWhiteSpace(propertyName))
            {
                return null;
            }

            return type.GetProperty(propertyName)
                ?.GetPropertyName(propertyNameSource);
        }
    }
}
