using System;
using System.Globalization;
using CMS.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual job opportunities,
    /// as represented in the search results from the search API.
    /// </summary>
    [SearchPageType(PageType_Job.CLASS_NAME)]
    public class Job
        : Page, ISearchable
    {
        /// <summary>
        /// Date and time of the job posting.
        /// </summary>
        [JsonProperty("postedDate")]
        [AzureSearchField(nameof(PageType_Job.PostedDate))]
        public DateTimeOffset PostedDate { get; set; }

        /// <summary>
        /// Location of the job posting.
        /// </summary>
        [JsonProperty("jobLocation")]
        [AzureSearchField(nameof(PageType_Job.JobLocation))]
        public string JobLocation { get; set; }
        
        /// <summary>
        /// Company of the job posting.
        /// </summary>
        [JsonProperty("company")]
        [AzureSearchField(nameof(PageType_Job.Company))]
        public string Company { get; set; }

        /// <summary>
        /// Link to the external job page if the detail
        /// page should link externally.
        /// </summary>
        [JsonProperty("externalUrl")]
        [AzureSearchField(nameof(PageType_Job.ExternalUrl))]
        public string ExternalUrl { get; set; }
        
        /// <summary>
        /// Job classification, e.g. Osler, Marketplace, etc
        /// </summary>
        [JsonProperty("jobClassification")]
        [JsonConverter(typeof(StringEnumConverter))]
        [AzureSearchField(nameof(PageType_Job.JobClassification))]
        public JobClassification JobClassification { get; set; }
        
        /// <summary>
        /// Localized Job classification Display, e.g. Osler, Marketplace, etc
        /// </summary>
        [JsonProperty("jobClassificationDisplay")]
        public string JobClassificationDisplay =>
            ResHelper.GetString(((Enum) JobClassification).GetDisplayName(), Culture);

        /// <summary>
        /// Job Category, e.g. Banking, etc
        /// </summary>
        [JsonIgnore]
        [AzureSearchField(nameof(PageType_Job.JobCategoryCodeName))]
        public string JobCategoryCodeName { get; set; }

        /// <summary>
        /// Localized display value for Job Category
        /// Job Category. eg) Banking, etc
        /// </summary>
        [JsonProperty("jobCategory")]
        public string JobCategoryDisplayName { get; set; }


        /// <summary>
        /// Localized Job Category and Posted Date
        /// </summary>
        [JsonProperty("jobDateAndCategoryDisplay")]
        public string JobDateAndCategoryDisplay => string.Format(
            ResHelper.GetString(Constants.ResourceStrings.Jobs.DatePostedAndCategory, Culture),
            PostedDate.ToString(StringHelper.GetDateTimeFormat(Culture),  CultureInfo.GetCultureInfo(Culture)), JobCategoryDisplayName);
    }
}
