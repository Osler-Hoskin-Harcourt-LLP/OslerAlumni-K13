using ECA.Core.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OslerAlumni.Mvc.Api.Helpers
{
    public static class AzureHelper
    {
        /// <summary>
        /// Full Text search requires special characters to be escaped.
        /// see: https://docs.microsoft.com/en-us/azure/search/query-lucene-syntax#escaping-special-characters
        /// </summary>
        /// <param name="keywordSearch"></param>
        /// <returns></returns>
        public static string SanitizeKeyword(
            string keywordSearch)
        {
            if (string.IsNullOrWhiteSpace(keywordSearch))
            {
                return null;
            }

            keywordSearch =
                EscapeSpecialCharacters(keywordSearch);

            return keywordSearch;
        }

        /// <summary>
        /// Full Text search requires special characters to be escaped.
        /// see: https://docs.microsoft.com/en-us/azure/search/query-lucene-syntax#escaping-special-characters
        /// </summary>
        /// <param name="keywordSearch"></param>
        /// <returns></returns>
        private static string EscapeSpecialCharacters(
            string keywordSearch)
        {
            var illegalCharacters = new List<string>
            {
                "+",
                "-",
                "&&",
                "||",
                "!",
                "(",
                ")",
                "{",
                "}",
                "[",
                "]",
                "^",
                "\"",
                "~",
                "*",
                "?",
                ":",
                "/"
            };

            // Note: "\" character must be replaced first!
            keywordSearch = keywordSearch.Replace(@"\", @"\\");

            return keywordSearch.Replace(
                illegalCharacters.ToDictionary(s => s, s => $@"\{s}"));
        }
    }
}
