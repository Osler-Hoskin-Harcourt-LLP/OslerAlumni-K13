using System;
using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Models
{
    public class FeaturedCommunityNewsSectionViewModel
    {
        public string NewsHeader { get; set; }
        public string NewsLink { get; set; }
        public string NewsLinkText { get; set; }
        public string NewsNoRecordsDisplay { get; set; }

        public Guid? NewsLinkedPageGuid { get; set; }

        public IEnumerable<HomePageFeaturedItem> CommunityNewsItems { get; set; }

    }
}
