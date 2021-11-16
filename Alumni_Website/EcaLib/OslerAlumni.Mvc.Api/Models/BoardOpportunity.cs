using System;
using System.Globalization;
using CMS.Helpers;
using Newtonsoft.Json;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual board opportunities,
    /// as represented in the search results from the search API.
    /// </summary>
    [SearchPageType(PageType_BoardOpportunity.CLASS_NAME)]
    public class BoardOpportunity
        : Page, ISearchable
    {
        /// <summary>
        /// Date and time of the job posting.
        /// </summary>
        [JsonProperty("postedDate")]
        [AzureSearchField(nameof(PageType_BoardOpportunity.PostedDate))]
        public DateTimeOffset PostedDate { get; set; }

        /// <summary>
        /// Location of the job posting.
        /// </summary>
        [JsonProperty("location")]
        [AzureSearchField(nameof(PageType_BoardOpportunity.BoardOpportunityLocation))]
        public string JobLocation { get; set; }
        
        /// <summary>
        /// Company of the job posting.
        /// </summary>
        [JsonProperty("company")]
        [AzureSearchField(nameof(PageType_BoardOpportunity.Company))]
        public string Company { get; set; }

        /// <summary>
        /// Link to the external job page if the detail
        /// page should link externally.
        /// </summary>
        [JsonProperty("externalUrl")]
        [AzureSearchField(nameof(PageType_BoardOpportunity.ExternalUrl))]
        public string ExternalUrl { get; set; }

        /// <summary>
        /// Job Category, e.g. Banking, etc
        /// </summary>
        [JsonIgnore]
        [AzureSearchField(nameof(PageType_BoardOpportunity.JobCategoryCodeName))]
        public string JobCategoryCodeName { get; set; }

        /// <summary>
        /// Localized display value for Job Category
        /// Job Category. eg) Banking, etc
        /// </summary>
        [JsonProperty("jobCategory")]
        public string JobCategoryDisplayName { get; set; }



        /// <summary>
        /// Board Opportunity Type, e.g. Public, Non-profit
        /// </summary>
        [JsonIgnore]
        [AzureSearchField(nameof(PageType_BoardOpportunity.BoardOpportunityTypeCodeName))]
        public string BoardOpportunityTypeCodeName { get; set; }


        /// <summary>
        /// Localized display value for Board Opportunity Type
        /// </summary>
        [JsonProperty("boardOpportunityType")]
        public string BoardOpportunityTypeDisplayName { get; set; }


        /// <summary>
        /// Localized Job Category and Posted Date
        /// </summary>
        [JsonProperty("jobDateAndCategoryDisplay")]
        public string JobDateAndCategoryDisplay => string.Format(
            ResHelper.GetString(Constants.ResourceStrings.Jobs.DatePostedAndCategory, Culture),
            PostedDate.ToString(StringHelper.GetDateTimeFormat(Culture), CultureInfo.GetCultureInfo(Culture)), JobCategoryDisplayName);
    }
}
