using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OslerAlumni.Mvc.Api.Definitions;

namespace OslerAlumni.Mvc.Api.Models
{
    public class SearchFilter
    {
        /// <summary>
        /// Name of the field in the search results, by which they should be filtered, when this filter group is applied.
        /// </summary>
        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// Localized title of the filter group, to be displayed on the front-end.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Specifies the filter type, which should determine how users can interact with it on the front-end.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterType Type { get; set; }

        /// <summary>
        /// List of filter options to choose from (will only be populated for
        /// <see cref="FilterType.MultipleChoice"/> and <see cref="FilterType.SingleChoice"/> filters.
        /// </summary>
        [JsonProperty("options")]
        public IList<SearchFilterOption> Options { get; set; }

        /// <summary>
        /// Placeholder text for text inputs (will only be populated for
        /// <see cref="FilterType.FreeText"/> filters.)
        /// </summary>
        [JsonProperty("placeHolderText")]
        public string PlaceHolderText { get; set; }
    }
}
