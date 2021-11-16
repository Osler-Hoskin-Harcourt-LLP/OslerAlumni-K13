using System;
using Newtonsoft.Json;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual pages (regardless of their type),
    /// as represented in the search results from the search API.
    /// </summary>
    public class Page
        : ISearchable
    {
        /// <summary>
        /// Name of the corresponding Kentico Page Type.
        /// </summary>
        [JsonProperty("pageType")]
        [AzureSearchField(nameof(PageType_BasePageType.ClassName))]
        public string PageType { get; set; }

        /// <summary>
        /// Node GUID of the corresponding page in Kentico's content tree.
        /// </summary>
        [JsonProperty("nodeGuid")]
        [AzureSearchField(nameof(PageType_BasePageType.NodeGUID))]
        public Guid NodeGuid { get; set; }

        /// <summary>
        /// Culture code of the Kentico page.
        /// </summary>
        [JsonProperty("culture")]
        [AzureSearchField(nameof(PageType_BasePageType.DocumentCulture))]
        public string Culture { get; set; }

        /// <summary>
        /// Title of the page.
        /// </summary>
        [JsonProperty("title")]
        [AzureSearchField(nameof(PageType_BasePageType.Title))]
        public string Title { get; set; }

        /// <summary>
        /// Short description of the page.
        /// </summary>
        [JsonProperty("shortDescription")]
        [AzureSearchField(nameof(PageType_BasePageType.ShortDescription))]
        public string ShortDescription { get; set; }

        /// <summary>
        /// URL of the details page.
        /// </summary>
        [JsonProperty("pageUrl")]
        public string PageUrl { get; set; }

    }
}
