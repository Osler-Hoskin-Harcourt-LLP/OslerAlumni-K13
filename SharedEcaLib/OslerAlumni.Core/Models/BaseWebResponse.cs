using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Models
{
    public class BaseWebResponse<T> : IBaseWebResponse<T>
    {
        [JsonProperty("result")]
        public virtual T Result { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WebResponseStatus Status { get; set; }

        /// <summary>
        /// informs the front end to refresh the page on success
        /// </summary>
        [JsonProperty("refreshOnSuccess")]
        public bool RefreshOnSuccess { get; set; }

        public string RedirectToUrl { get; set; }
    }
}
