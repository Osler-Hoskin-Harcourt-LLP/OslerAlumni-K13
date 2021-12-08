using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class LandingPageViewModel
        : BasePageViewModel
    {
        #region "Properties"
        
        public int MainListPageSize { get; set; }

        public int TopListPageSize { get; set; }

        #endregion

        public LandingPageViewModel(PageType_LandingPage page)
            : base(page)
        {
            MainListPageSize = page.MainListPageSize;
            TopListPageSize = page.TopListPageSize;
        }
    }
}