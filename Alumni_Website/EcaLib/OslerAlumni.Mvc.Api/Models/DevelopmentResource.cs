using System;

using Newtonsoft.Json;

using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual resource items,
    /// as represented in the search results from the search API.
    /// </summary>
    [SearchPageType(PageType_DevelopmentResource.CLASS_NAME)]
    public class DevelopmentResource
        : Page, ISearchable
    {
        /// <summary>
        /// Date and time of the resource item publication.
        /// </summary>
        [JsonProperty("datePublished")]
        [AzureSearchField(nameof(PageType_DevelopmentResource.DatePublished))]
        public DateTimeOffset DatePublished { get; set; }

        /// <summary>
        /// Type of the resource item
        /// </summary>
        [JsonProperty("authors")]
        [AzureSearchField(nameof(PageType_DevelopmentResource.Authors))]
        public string Authors { get; set; }

        /// <summary>
        /// Type of the resource item
        /// </summary>
        [JsonProperty("developmentResourceTypes")]
        [AzureSearchField(nameof(PageType_DevelopmentResource.DevelopmentResourceTypes))]
        public string DevelopmentResourceTypes { get; set; }

        /// <summary>
        /// Link to an external resource page if the detail
        /// page should link externally.
        /// </summary>
        [JsonProperty("externalUrl")]
        [AzureSearchField(nameof(PageType_DevelopmentResource.ExternalUrl))]
        public string ExternalUrl { get; set; }

        /// <summary>
        /// True if link is an external file.
        /// </summary>
        [JsonProperty("isFile")]
        [AzureSearchField(nameof(PageType_DevelopmentResource.IsFile))]
        public bool IsFile { get; set; }
    }
}
