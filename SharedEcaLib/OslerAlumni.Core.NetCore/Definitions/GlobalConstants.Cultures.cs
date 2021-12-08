using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;

namespace OslerAlumni.Core.Definitions
{
    public static partial class GlobalConstants
    {
        public static class Cultures
        {
            #region "Private Fields"

            private static readonly List<string> _allowedCultureCodes =
                new List<string>
                {
                    English,
                    French
                };

            #endregion

            public const string Default = English;

            public const string English = "en-CA";
            public const string French = "fr-CA";

            public static readonly Dictionary<string, string> AllowedCultureCodes =
                _allowedCultureCodes
                    .ToDictionary(
                        CultureHelper.GetShortCultureCode,
                        cultureCode => cultureCode);
        }
    }
}
