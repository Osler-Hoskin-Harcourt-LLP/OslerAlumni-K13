using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IResourceService
        : IService
    {
        PageType_Resource GetFeaturedResource(
            PageType_LandingPage landingPage);

        /// <summary>
        /// Converts DB value into localized text.
        /// </summary>
        /// <param name="resourceTypes"></param>
        /// <param name="culture"></param>
        /// <returns>Comma seperated list of display names</returns>
        string GetResourceTypesDisplayString(string resourceTypes, string culture = null);

        /// <summary>
        /// Converts resource Type code names to display names
        /// </summary>
        /// <param name="resourceTypeArray"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        string GetResourceTypesDisplayString(string[] resourceTypeArray, string culture = null);

        List<PageType_Resource> GetLatestResources(int top, bool filterForCompetitor);

    }
}
