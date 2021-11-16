using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IEventsService
        : IService
    {
        List<PageType_Event> GetLatestEvents(int top, bool filterForCompetitor);
    }
}
