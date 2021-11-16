using System.Collections.Generic;
using CMS.DataEngine;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Helpers;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Kentico.Models
{
    public partial class PageType_Profile
        : IBasePageType
    {
        public List<YearAndJurisdiction> YearOfCallAndJurisdictionsList
        {
            get
            {
                return UserProfileMappingHelper.YearOfCallAndJurisdictionsList(YearsAndJurisdictions);
            }
            set { }
        }

        public List<EducationRecord> EducationOverviewList => UserProfileMappingHelper.ToEducationHistory(EducationOverview);
    }
}
