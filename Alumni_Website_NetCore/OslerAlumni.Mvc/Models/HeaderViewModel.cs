using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;

namespace OslerAlumni.Mvc.Models
{
    public class HeaderViewModel
    {
        public LanguageToggleViewModel LanguageToggle { get; set; }

        public IEnumerable<NavigationItem> PrimaryNavigationItems { get; set; }

        public IEnumerable<NavigationItem> SecondaryNavigationItems { get; set; }

        public string DesktopLogoImageUrl { get; set; }
        public string MobileLogoImageUrl { get; set; }

        public bool IsAuthenticatedUser { get; set; }

        public string GlobalSearchPageUrl { get; set; }

        public string UserFirstName { get; set; }

        public bool IsLoggedInViaOslerNetwork { get; set; }
    }
}
