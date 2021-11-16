using Newtonsoft.Json;

using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual profiles,
    /// as represented in the search results from the search API.
    /// </summary>
    [SearchPageType(PageType_Profile.CLASS_NAME)]
    public class Profile
        : Page, ISearchable
    {
        [JsonProperty("name")]
        [AzureSearchField(nameof(PageType_Profile.Title))]
        public string Name { get; set; }

        [JsonProperty("jobTitle")]
        [AzureSearchField(nameof(PageType_Profile.JobTitle))]
        public string JobTitle { get; set; }
        
        [JsonProperty("company")]
        [AzureSearchField(nameof(PageType_Profile.ProfileCompany))]
        public string Company { get; set; }

        [JsonProperty("city")]
        [AzureSearchField(nameof(PageType_Profile.City))]
        public string City { get; set; }

        [JsonProperty("Province")]
        [AzureSearchField(nameof(PageType_Profile.Province))]
        public string Province { get; set; }

        [JsonProperty("country")]
        [AzureSearchField(nameof(PageType_Profile.Country))]
        public string Country { get; set; }

        /// <summary>
        /// This field is added so that Order By Clause works.
        /// </summary>
        [JsonProperty("lastName")]
        [AzureSearchField(nameof(PageType_Profile.LastNameNormalized))]
        public string LastName { get; set; }

    }
}
