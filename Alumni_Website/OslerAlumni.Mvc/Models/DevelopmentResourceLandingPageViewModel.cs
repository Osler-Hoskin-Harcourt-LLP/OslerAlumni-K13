using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class DevelopmentResourceLandingPageViewModel
        : LandingPageViewModel
    {
        #region "Properties"

        public string DefaultImageUrl { get; set; }

        public ResourceFeaturedItemViewModel FeaturedItem { get; set; }

        #endregion

        public DevelopmentResourceLandingPageViewModel(
            PageType_LandingPage page)
            : base(page)
        { }
    }
}
