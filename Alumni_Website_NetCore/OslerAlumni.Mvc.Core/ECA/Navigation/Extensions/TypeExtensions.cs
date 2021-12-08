using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ECA.Mvc.Navigation.Extensions
{
    public static class TypeExtensions
    {
        public static string GetControllerName(
            this Type type)
        {
            if (type == null)
            {
                return null;
            }

            return Regex.Replace(
                type.Name,
                "Controller$",
                string.Empty);
        }

        public static Dictionary<PropertyInfo, T> GetPropertiesWithAttribute<T>(
            this Type type)
            where T : Attribute
        {
            if (type == null)
            {
                return null;
            }

            return type
                .GetProperties(
                    BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    p => p,
                    p => p.GetCustomAttribute<T>(true))
                .Where(kvp => kvp.Value != null)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value);
        }

        public static MethodInfo GetMatchingMethod(
            this Type type,
            string methodName,
            object[] parameterValues)
        {
            if ((type == null)
                || string.IsNullOrWhiteSpace(methodName))
            {
                return null;
            }

            var parameterTypes =
                parameterValues?
                    .Select(pv => pv?.GetType())
                    .ToArray();

            return parameterTypes == null
                ? type.GetMethod(methodName)
                : type.GetMethod(methodName, parameterTypes);
        }
    }
}
