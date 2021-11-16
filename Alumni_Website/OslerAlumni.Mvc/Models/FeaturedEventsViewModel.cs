using System.Collections.Generic;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Models
{
    public class FeaturedEventsViewModel
    {
        public string EventsHeader { get; set; }
        public string EventsLink { get; set; }
        public string EventsLinkText { get; set; }
        
        public string EventsNoRecordsDisplay { get; set; }

        public IEnumerable<HomePageFeaturedEventItem> EventItems { get; set; }

    }
}
