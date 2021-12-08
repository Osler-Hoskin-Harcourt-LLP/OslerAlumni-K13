using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class ProfileDetailsPageViewModel
        : BasePageViewModel
    {
        public bool IncludeEmailInDirectory { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string CurrentIndustry { get; set; }

        public string LinkedInUrl { get; set; }

        public string LinkedInHandle => LinkedInUrl?.Substring(LinkedInUrl.LastIndexOf("/") + 1);

        public string TwitterUrl { get; set; }
        public string TwitterHandle => $"@{TwitterUrl?.Substring(TwitterUrl.LastIndexOf("/") + 1)}";
        public string InstagramUrl { get; set; }

        public string InstagramHandle => InstagramUrl?.Substring(InstagramUrl.LastIndexOf("/") + 1);

        public List<string> OfficeLocations { get; set; }
        public List<string> PracticeAreas { get; set; }
        public List<string> BoardMemberships { get; set; }

        public string YearsAtOsler { get; set; }

        public List<YearAndJurisdiction> YearOfCallAndJurisdictions { get; set; }

        public List<EducationRecord> EducationHistory { get; set; }

        public string ProfileImageUrl { get; set; }

        public ProfileDetailsPageViewModel(PageType_Profile page)
            : base(page)
        {
            Email = page.AlumniEmail;
            City = page.City;
            Province = page.Province;
            Country = page.Country;
            JobTitle = page.JobTitle;
            Company = page.ProfileCompany;
            IncludeEmailInDirectory = page.IncludeEmailInDirectory;
            LinkedInUrl = page.LinkedInUrl;
            TwitterUrl = page.TwitterUrl;
            InstagramUrl = page.InstagramUrl;
            ProfileImageUrl = page.ProfileImage;
        }
    }
}
