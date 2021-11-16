using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual news articles,
    /// as represented in the search results from the search API.
    /// </summary>
    [SearchPageType(PageType_News.CLASS_NAME)]
    public class News
        : Page, ISearchable
    {
        /// <summary>
        /// URL of the main News image, if available.
        /// Currently only used for Spotlight news pages.
        /// </summary>
        [JsonProperty("imageUrl")]
        [AzureSearchField(nameof(PageType_News.Image))]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Alt text for the main image, if available.
        /// </summary>
        [JsonProperty("imageAltText")]
        [AzureSearchField(nameof(PageType_News.ImageAltText))]
        public string ImageAltText { get; set; }
        
        /// <summary>
        /// Date and time of the news article publication.
        /// </summary>
        [JsonProperty("datePublished")]
        [AzureSearchField(nameof(PageType_News.DatePublished))]
        public DateTimeOffset DatePublished { get; set; }
        
        /// <summary>
        /// Type of the news article (News vs Spotlight).
        /// </summary>
        [JsonProperty("newsPageType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [AzureSearchField(nameof(PageType_News.Type))]
        public NewsPageType Type { get; set; }
    }
}
