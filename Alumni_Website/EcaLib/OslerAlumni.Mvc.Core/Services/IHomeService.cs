using System.Collections.Generic;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IHomeService
    {
        List<HomePageFeaturedEventItem> GetFeaturedEventItems(int top, bool filterForCompetitor);
        List<HomePageFeaturedItem> GetFeaturedResourceItems(int top, bool filterForCompetitor);
        List<HomePageFeaturedItem> GetFeaturedBoardOpportunityItems(int top);
        List<HomePageFeaturedItem> GetFeaturedNewsItems(int top);
        List<HomePageFeaturedItem> GetFeaturedJobItems(int top);

    }
}
