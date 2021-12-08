using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class NewsLandingPageViewModel
        : LandingPageViewModel
    {
        #region "Properties"

        public string DefaultImageUrl { get; set; }

        public NewsFeaturedItemViewModel FeaturedItem { get; set; }

        #endregion

        public NewsLandingPageViewModel(
            PageType_LandingPage page)
            : base(page)
        { }
    }
}