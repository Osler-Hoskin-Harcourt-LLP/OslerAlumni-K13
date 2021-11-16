using System;
using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Models
{
    public class FeaturedJobOpportunitiesViewModel
    {
        public string JobsHeader { get; set; }
        public string JobsLink { get; set; }
        public string JobsLinkText { get; set; }
        public string JobsNoRecordsDisplay { get; set; }

        public IEnumerable<HomePageFeaturedItem> JobOpportunityItems { get; set; }

    }
}
