using Newtonsoft.Json;

namespace OslerAlumni.Mvc.Api.Models
{
    public class SearchFilterOption
    {
        /// <summary>
        /// Localized name of the filter option, to be displayed on the front-end.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Culture-invariant value that represents the filter option, which can be used to filter search results.
        /// </summary>
        [JsonProperty("codeName")]
        public string CodeName { get; set; }
    }
}