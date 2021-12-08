using System.Collections.Generic;
using System.Linq;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Definitions
{
    public static partial class Constants
    {
        public const string VerifyUserTokenPurpose = "ResetPassword";

        public static readonly IReadOnlyList<string> OslerPageTypes =
            new[]
            {
                PageType_Page.CLASS_NAME,
                PageType_Event.CLASS_NAME,
                PageType_LandingPage.CLASS_NAME,
                PageType_News.CLASS_NAME,
                PageType_Job.CLASS_NAME,
                PageType_Resource.CLASS_NAME,
                PageType_Profile.CLASS_NAME,
                PageType_DevelopmentResource.CLASS_NAME,
                PageType_BoardOpportunity.CLASS_NAME
            };
    }
}
