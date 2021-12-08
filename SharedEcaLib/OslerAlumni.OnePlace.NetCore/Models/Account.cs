using Newtonsoft.Json;
using OslerAlumni.OnePlace.Attributes;

namespace OslerAlumni.OnePlace.Models
{
    [OnePlaceObject("Account")]
    public class Account
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Industry")]
        public string Industry { get; set; }
    }
}