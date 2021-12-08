using System;
using CMS.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for global result objects,
    /// as represented in the search results from the search API.
    /// </summary>
    public class GlobalResult
        : Page, ISearchable
    {
        /// <summary>
        /// True if link is an external file.
        /// </summary>
        [JsonProperty("isFile")]
        [AzureSearchField(nameof(PageType_Resource.IsFile))]
        public bool IsFile { get; set; }

        /// <summary>
        /// Link to an external resource page if the detail
        /// page should link externally.
        /// </summary>
        [JsonProperty("externalUrl")]
        [AzureSearchField(nameof(PageType_Resource.ExternalUrl))]
        public string ExternalUrl { get; set; }
    }
}
