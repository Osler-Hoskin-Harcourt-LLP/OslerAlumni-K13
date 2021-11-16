using System;
using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Models
{
    public class FeaturedResourcesViewModel
    {
        public string ResourcesHeader { get; set; }
        public string ResourcesLink { get; set; }
        public string ResourcesLinkText { get; set; }
        public string ResourcesNoRecordsDisplay { get; set; }

        public IEnumerable<HomePageFeaturedItem> ResourceItems { get; set; }

    }
}
