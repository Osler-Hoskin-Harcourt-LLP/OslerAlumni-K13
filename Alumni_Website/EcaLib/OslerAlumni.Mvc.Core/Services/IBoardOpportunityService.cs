using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IBoardOpportunityService
        : IService
    {
        List<PageType_BoardOpportunity> GetLatestBoardOpportunities(int top);
    }
}
