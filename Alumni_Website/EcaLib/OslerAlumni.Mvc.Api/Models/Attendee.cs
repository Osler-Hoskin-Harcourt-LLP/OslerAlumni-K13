using System;
using Newtonsoft.Json;

namespace OslerAlumni.Mvc.Api.Models
{
    public class Attendee
    {

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("profileUrl")]
        public string ProfileUrl { get; set; }

        [JsonProperty("searchField")]
        public string SearchField => string.Join(" ", new[]
            {
                FirstName,
                LastName,
                CompanyName
            })
            .Replace(",", String.Empty)
            .Replace(".", String.Empty);

    }
}
