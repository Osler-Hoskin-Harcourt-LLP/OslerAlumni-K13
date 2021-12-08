using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace OslerAlumni.Mvc.Extensions
{
    public static partial class OslerHtmlHelperExtensions
    {
        /// <summary>
        /// Truncates a string after a full word and not in between
        /// e.g) This is truncated ...
        /// </summary>
        /// <param name="html"></param>
        /// <param name="str"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string TruncateAfterWord(
            this IHtmlHelper html, string str, int charCount)
        {
            if (str.Length <= charCount)
            {
                return str;
            }

            str = str.Substring(0, charCount - 4);

            var lastWhiteSpaceIndex = str.LastIndexOf(" ", StringComparison.Ordinal);

            if (lastWhiteSpaceIndex > 0)
            {
                str = str.Substring(0, lastWhiteSpaceIndex);
            }

            return $"{str} ...";
        }
    }
}
