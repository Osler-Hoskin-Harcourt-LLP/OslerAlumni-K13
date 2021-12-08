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
    [SearchPageType(PageType_Resource.CLASS_NAME)]
    public class Resource
        : Page, ISearchable
    {
        /// <summary>
        /// Date and time of the resource item publication.
        /// </summary>
        [JsonProperty("datePublished")]
        [AzureSearchField(nameof(PageType_Resource.DatePublished))]
        public DateTimeOffset DatePublished { get; set; }

        /// <summary>
        /// Type of the resource item
        /// </summary>
        [JsonProperty("authors")]
        [AzureSearchField(nameof(PageType_Resource.Authors))]
        public string Authors { get; set; }

        /// <summary>
        /// Type of the resource item
        /// </summary>
        [JsonProperty("resourceTypes")]
        [AzureSearchField(nameof(PageType_Resource.Types))]
        public string ResourceTypes { get; set; }

        /// <summary>
        /// Link to an external resource page if the detail
        /// page should link externally.
        /// </summary>
        [JsonProperty("externalUrl")]
        [AzureSearchField(nameof(PageType_Resource.ExternalUrl))]
        public string ExternalUrl { get; set; }

        /// <summary>
        /// True if link is an external file.
        /// </summary>
        [JsonProperty("isFile")]
        [AzureSearchField(nameof(PageType_Resource.IsFile))]
        public bool IsFile { get; set; }
    }
}
