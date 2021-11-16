using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;

namespace OslerAlumni.Mvc.Models
{
    public class FooterViewModel
    {
        public IEnumerable<NavigationItem> FooterNavigationItems { get; set; }

        public string LogoImageUrl { get; set; }
    }
}
