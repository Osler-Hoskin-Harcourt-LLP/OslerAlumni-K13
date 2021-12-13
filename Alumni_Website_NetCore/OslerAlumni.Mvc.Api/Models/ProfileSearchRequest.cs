using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Helpers;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for resource search requests to the search API.
    /// </summary>
    public class ProfileSearchRequest
        : SearchRequest<Profile>
    {
        #region "Properties"

        [JsonProperty("includeFilters")]
        public bool IncludeFilters { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; } = "";

        [JsonProperty("yearOfCall")]
        public string YearOfCall { get; set; } = "";

        [JsonProperty("jurisdictions")]
        public List<int> Jurisdictions { get; set; } = new List<int>();

        [JsonIgnore]
        public List<string> JurisdictionsCodeNames { get; set; } = new List<string>();

        [JsonProperty("industries")]
        public List<int> Industries { get; set; } = new List<int>();

        [JsonIgnore]
        public List<string> IndustriesCodeNames { get; set; } = new List<string>();

        [JsonProperty("practiceAreas")]
        public List<int> PracticeAreas { get; set; } = new List<int>();

        [JsonIgnore]
        public List<string> PracticeAreasCodeNames { get; set; } = new List<string>();

        [JsonProperty("officeLocations")]
        public List<int> OfficeLocations { get; set; } = new List<int>();

        [JsonIgnore]
        public List<string> OfficeLocationsCodeNames { get; set; } = new List<string>();

        #endregion

        #region "Methods"

        public override AzureSearchFilterExpression GetFilterExpression()
        {
            AzureSearchFilterExpression filter = base.GetFilterExpression();

            //Sanitize Text Fields
            Location = AzureHelper.SanitizeKeyword(Location);
            YearOfCall = AzureHelper.SanitizeKeyword(YearOfCall);

            if (!string.IsNullOrEmpty(Location))
            {
                var additionalFilter = new AzureSearchFilterExpression()
                    .Matches(nameof(PageType_Profile.City), Location);

                filter.And(additionalFilter);
            }

            if (!string.IsNullOrEmpty(YearOfCall) && (JurisdictionsCodeNames?.Any() ?? false))
            {
                // We need to combine both properties if both have values
                var list = JurisdictionsCodeNames.Select(jurisdiction => $"\"{YearOfCall} {jurisdiction}\"").ToList();

                AzureSearchFilterExpression additionalFilter = new AzureSearchFilterExpression();

                foreach (var item in list)
                {
                    var itemfilter = new AzureSearchFilterExpression()
                        .Matches(nameof(PageType_Profile.YearsAndJurisdictions), item, QueryType.Full, SearchMode.All);

                    additionalFilter.Or(itemfilter);
                }

                filter.And(additionalFilter);

            }
            else if (!string.IsNullOrEmpty(YearOfCall))
            {
                var additionalFilter = new AzureSearchFilterExpression()
                    .Matches(nameof(PageType_Profile.YearsAndJurisdictions), YearOfCall);

                filter.And(additionalFilter);
            }
            else if (JurisdictionsCodeNames?.Any() ?? false)
            {
                AzureSearchFilterExpression additionalFilter = new AzureSearchFilterExpression();

                foreach (var item in JurisdictionsCodeNames)
                {
                    var itemfilter = new AzureSearchFilterExpression()
                        .Matches(nameof(PageType_Profile.YearsAndJurisdictions), item);

                    additionalFilter.Or(itemfilter);
                }

                filter.And(additionalFilter);
            }

            if (IndustriesCodeNames?.Any() ?? false)
            {
                AzureSearchFilterExpression additionalFilter = new AzureSearchFilterExpression();

                foreach (var item in IndustriesCodeNames)
                {
                    var itemfilter = new AzureSearchFilterExpression()
                        .Matches(nameof(PageType_Profile.CurrentIndustry), item);

                    additionalFilter.Or(itemfilter);
                }

                filter.And(additionalFilter);
            }

            if (PracticeAreasCodeNames?.Any() ?? false)
            {
                AzureSearchFilterExpression additionalFilter = new AzureSearchFilterExpression();

                foreach (var item in PracticeAreasCodeNames)
                {
                    var itemfilter = new AzureSearchFilterExpression()
                        .Matches(nameof(PageType_Profile.PracticeAreas), item);

                    additionalFilter.Or(itemfilter);
                }

                filter.And(additionalFilter);
            }

            if (OfficeLocationsCodeNames?.Any() ?? false)
            {
                AzureSearchFilterExpression additionalFilter = new AzureSearchFilterExpression();

                foreach (var item in OfficeLocationsCodeNames)
                {
                    var itemfilter = new AzureSearchFilterExpression()
                        .Matches(nameof(PageType_Profile.OfficeLocations), item);

                    additionalFilter.Or(itemfilter);
                }

                filter.And(additionalFilter);
            }

            return filter;
        }

        public override bool IsKeywordOrFilteredSearch()
        {
            return base.IsKeywordOrFilteredSearch()
                   || !string.IsNullOrWhiteSpace(Location)
                   || !string.IsNullOrWhiteSpace(YearOfCall)
                   || (Jurisdictions != null && Jurisdictions.Count > 0)
                   || (Industries != null && Industries.Count > 0)
                   || (PracticeAreas != null && PracticeAreas.Count > 0)
                   || (OfficeLocations != null && OfficeLocations.Count > 0);
        }

        #endregion
    }
}
