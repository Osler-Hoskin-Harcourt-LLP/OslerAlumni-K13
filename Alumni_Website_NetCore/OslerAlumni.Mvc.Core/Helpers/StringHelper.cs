using System.Linq;
using System.Web;
using CMS.Helpers;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Returns the first non-whitespace/empty string in the provided sequence
        /// </summary>
        /// <param name="str"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string GetNonEmpty(params string[] values)
        {
            return values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
        }

        /// <summary>
        /// Returns the date time format string for a particular culture
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetDateTimeFormat(string culture)
        {
            return ResHelper.GetString(Constants.ResourceStrings.DateTimeFormat, culture);
        }

        /// <summary>
        /// Remove "ViewComponent" from the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveViewComponent(string input)
        {
            return input.Replace("ViewComponent", "");
        }
    }
}
