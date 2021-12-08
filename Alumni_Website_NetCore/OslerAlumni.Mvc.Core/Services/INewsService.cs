using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface INewsService
        : IService
    {
        PageType_News GetFeaturedNews(
            PageType_LandingPage landingPage);

        List<PageType_News> GetLatestNews(int top);
    }
}
