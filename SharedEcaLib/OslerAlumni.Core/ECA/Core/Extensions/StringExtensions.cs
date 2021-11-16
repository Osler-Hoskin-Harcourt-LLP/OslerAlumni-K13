using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CMS.DocumentEngine;
using CMS.Helpers;

namespace ECA.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Given a string, returns the first Guid that it finds.
        /// </summary>
        /// <returns>Guid if found, else null</returns>
        public static Guid? GetGuidFromString(this string stringValue)
        {
            stringValue = stringValue ?? string.Empty;

            var extractGuidRegex =
                @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}";

            var regex = new Regex(extractGuidRegex);
            var match = regex.Match(stringValue);

            Guid result;

            if (Guid.TryParse(match.Value, out result))
            {
                return result;
            }

            return null;
        }

        public static string JoinSorted(
            this string[] strArray,
            string separator,
            string defaultValue)
        {
            if (DataHelper.DataSourceIsEmpty(strArray))
            {
                return defaultValue;
            }

            var result = string.Join(
                separator,
                strArray.OrderBy(str => str));

            if (string.IsNullOrWhiteSpace(result))
            {
                return defaultValue;
            }

            return result;
        }

        public static string ReplaceIfEqual(
            this string str1,
            string str2,
            string replacement)
        {
            return string.Equals(
                    str1,
                    str2,
                    StringComparison.OrdinalIgnoreCase)
                ? replacement
                : str1;
        }

        public static string Replace(
            this string str,
            Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            foreach (var rep in replacements)
            {
                str = str.Replace(rep.Key, rep.Value);
            }

            return str;
        }

        public static string ReplaceIfEmpty(
            this string str,
            string emptyReplacement)
        {
            return string.IsNullOrWhiteSpace(str)
                ? emptyReplacement
                : str;
        }

        public static List<string> SplitOn(
            this string str,
            params char[] separator)
        {
            return str?.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                       .Select(item => item.Trim())
                       .ToList() ??
                   new List<string>();
        }

        /// <summary>
        /// Removes extra whitespaces from the entire string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimAll(
            this string str
            )
        {
            return str?.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                       .Join(" ");
        }

        public static string TrimStart(
            this string str,
            string start)
        {
            if (string.IsNullOrEmpty(str)
                || string.IsNullOrEmpty(start))
            {
                return str;
            }

            while (str.StartsWith(start))
            {
                str = str.Substring(start.Length);
            }

            return str;
        }

        public static string TrimEnd(
            this string str,
            string end)
        {
            if (string.IsNullOrEmpty(str)
                || string.IsNullOrEmpty(end))
            {
                return str;
            }

            while (str.EndsWith(end))
            {
                str = str.Substring(0, str.Length - end.Length);
            }

            return str;
        }

        public static string ToPascalCase(
            this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(
                str,
                @"^_?[a-z]",
                m => m.ToString()
                        .Replace("_", string.Empty)
                        .ToUpper());
        }

        public static string ToSafeKenticoIdentifier(
            this string str, string siteName)
        {
            //The following is needed so that accented charcters
            //such as é, â, ü etc get converted to their ascii equivalents e, a, u
            //Note this is not 100% safe but works for french characters.

            // TODO##
            //str =
            //    TreePathUtils.GetSafeUrlPath(
            //        str,
            //        siteName);

            return ValidationHelper.GetIdentifier(str, String.Empty);
        }


    }
}
