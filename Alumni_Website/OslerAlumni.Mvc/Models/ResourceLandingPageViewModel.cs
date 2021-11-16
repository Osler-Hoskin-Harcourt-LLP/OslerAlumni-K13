using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class ResourceLandingPageViewModel
        : LandingPageViewModel
    {
        #region "Properties"

        public string DefaultImageUrl { get; set; }

        public ResourceFeaturedItemViewModel FeaturedItem { get; set; }

        #endregion

        public ResourceLandingPageViewModel(
            PageType_LandingPage page)
            : base(page)
        { }
    }
}