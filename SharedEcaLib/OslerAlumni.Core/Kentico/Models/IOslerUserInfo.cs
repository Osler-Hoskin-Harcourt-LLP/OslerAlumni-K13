using System;
using System.Collections.Generic;
using CMS.Base;
using CMS.DataEngine;
using CMS.Membership;
using OslerAlumni.Core.Kentico.Helpers;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Kentico.Models
{
    public interface IOslerUserInfo
        : IUserInfo, IInfo
    {
        UserInfo UserInfo { get; }

        string OnePlaceReference { get; set; }

        bool IsAlumni { get; set; }

        bool IsDisabledAutomatically { get; set; }

        string AlumniEmail { get; set; }

        DateTime? StartDateAtOsler { get; set; }
        DateTime? EndDateAtOsler { get; set; }

        string ProfileImage { get; set; }

        string Company { get; set; }

        string JobTitle { get; set; }

        string Country { get; set; }

        string Province { get; set; }

        string City { get; set; }

        string YearsAndJurisdictions { get; set; }

        string CurrentIndustry { get; set; }

        string LinkedInUrl { get; set; }

        string TwitterUrl { get; set; }
        string InstagramUrl { get; set; }

        bool IncludeEmailInDirectory { get; set; }

        bool DisplayImageInDirectory { get; set; }

        bool SubscribeToEmailUpdates { get; set; }

        bool UnsubscribeAllCommunications { get; set; }

        string SubscriptionPreferences { get; set; }

        string CommunicationPreferences { get; set; }


        string PracticeAreas { get; set; }

        List<string> PracticeAreasList { get; set; }

        string OfficeLocations { get; set; }

        List<string> OfficeLocationsList { get; set; }

        string BoardMemberships { get; set; }

        List<string> BoardMembershipsList { get; set; }

        List<YearAndJurisdiction> YearOfCallAndJurisdictionsList { get; set; }

        bool? UpdateOnePlace { get; set; }

        bool IsCompetitor { get; set; }

        string EducationOverview { get; set; }

        List<EducationRecord> EducationOverviewList { get; }


        /// <summary>
        /// Automatically populates the values of fields that depend on other fields, e.g.:
        /// - FullName: {FirstName} {LastName}
        /// </summary>
        void AutopopulateDependantFields();

        /// <summary>
        /// Checks if the user meets the criteria for an Osler Alumnus/Alumna.
        /// </summary>
        /// <param name="siteName">
        /// Additionally checks if the user belongs the the specified site.
        /// If the provided value is null or empty, this check is omitted.
        /// </param>
        /// <returns></returns>
        bool IsAlumniUser(string siteName);

    }
}
