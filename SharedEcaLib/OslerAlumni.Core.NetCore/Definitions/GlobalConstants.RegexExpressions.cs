namespace OslerAlumni.Core.Definitions
{
    public static partial class GlobalConstants
    {
        public static class RegexExpressions
        {
            public const string EmailRegex = @"^([a-zA-Z0-9'%!_+\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";

            public const string YearAndJurisdictionRegex = @"(\d{4}) (.*)";

            public const string YearAndJurisdictionReversedRegex = @"(.*) (\d{4})"; 

            /// <summary>
            /// https://regex101.com/r/O7YVJx/1
            /// </summary>
            public const string EducationHistoryRegex = @"(.*),\s*(\d{4})\s*,(.*)";

            /// <summary>
            /// //https://regex101.com/r/cI4gH9/1
            /// </summary>
            public const string LinkedInUrlRegex = @"^https:\/\/[a-z]{2,3}\.linkedin\.com\/.*$";
            /// <summary>
            /// https://regex101.com/r/2Ccuio/1
            /// </summary>
            public const string TwitterUrlRegex = @"^https:\/\/([w]{3}\.)?twitter\.com\/.*$";
            public const string InstagramUrlRegex = @"^https:\/\/www\.instagram\.com\/.*$";
        }
    }
}
