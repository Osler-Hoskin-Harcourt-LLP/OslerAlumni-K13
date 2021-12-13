using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.SiteProvider;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Services
{
    /// <remarks>
    /// Note that resource strings do NOT need to use caching,
    /// because Kentico uses a hash table to store them for the duration of the application lifetime.
    /// Whenever a resource string is updated via CMS admin area, Kentico updates the hashtable record for it as well.
    /// Changes made directly in the DB will not be reflected in the hashtable without clearing the site cache.
    /// </remarks>
    public class OnePlaceFieldLocalizerService
        : ServiceBase, IOnePlaceFieldLocalizerService
    {
        private readonly ContextConfig _context;

        public OnePlaceFieldLocalizerService(ContextConfig context)
        {
            _context = context;
        }

        #region "Methods"


        /// <inheritdoc />
        public string GetResourceStringCodeNameForCurrentIndustry(
            string onePlaceCurrentIndustry)
        {
            return GetResourceStringCodeName(
                onePlaceCurrentIndustry,
                GlobalConstants.ResourceStrings.OnePlaceLocalizations.IndustryPrefix);
        }

        public string GetResourceStringCodeNameForOfficeLocation(
            string onePlaceOffice)
        {
            return GetResourceStringCodeName(
                onePlaceOffice,
                GlobalConstants.ResourceStrings.OnePlaceLocalizations.OfficeLocationPrefix);
        }

        public string GetResourceStringCodeNameForPracticeArea(string onePlacePracticeArea)
        {
            return GetResourceStringCodeName(
                onePlacePracticeArea,
                GlobalConstants.ResourceStrings.OnePlaceLocalizations.PracticeAreaPrefix);
        }

        public string GetResourceStringCodeNameForJurisdiction(string jurisdiction)
        {
            return GetResourceStringCodeName(
                jurisdiction,
                GlobalConstants.ResourceStrings.OnePlaceLocalizations.JurisdictionPrefix);
        }

       

        #endregion

        #region "Helper methods"

        private string GetResourceStringCodeName(string str, string resStringPrefix)
        {
            return string.IsNullOrWhiteSpace(str)
                ? string.Empty
                : $"{resStringPrefix}{str.ToSafeKenticoIdentifier(SiteContext.CurrentSiteName)}";
        }

        #endregion
    }
}
