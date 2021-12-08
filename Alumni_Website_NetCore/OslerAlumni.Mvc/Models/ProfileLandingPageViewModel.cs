using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class ProfileLandingPageViewModel
        : LandingPageViewModel
    {
        #region "Properties"

        public PageType_Profile LoggedInUserPage { get; set; }

        #endregion

        public ProfileLandingPageViewModel(
            PageType_LandingPage page)
            : base(page)
        { }
    }
}
