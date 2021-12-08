using System;
using System.Collections.Generic;
using ECA.Mvc.Navigation.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Models
{
    public class FeaturedBoardOpportunitiesViewModel
    {
        public string BoardsHeader { get; set; }
        public string BoardsLink { get; set; }
        public string BoardLinkText { get; set; }
        
        public string BoardsNoRecordsDisplay { get; set; }

        public IEnumerable<HomePageFeaturedItem> BoardOpportunityItems { get; set; }

    }
}
