using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;

namespace OslerAlumni.Mvc.Models
{
    public class BreadCrumbsViewModel
    {
        public IEnumerable<NavigationItem> BreadCrumbs { get; set; }
    }
}
