using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Services
{
    /// <summary>
    /// Use this class for mapping fields from oneplace to osler alumni.
    /// For instace splitting multi-select values / parsing syntax / localization.
    /// </summary>
    public interface IOnePlaceFieldLocalizerService
        : IService
    {
        string GetResourceStringCodeNameForCurrentIndustry(
            string onePlaceCurrentIndustry);

        string GetResourceStringCodeNameForOfficeLocation(string onePlaceOffice);

        string GetResourceStringCodeNameForPracticeArea(string onePlacePracticeArea);
        string GetResourceStringCodeNameForJurisdiction(string jurisdiction);

    }
}
