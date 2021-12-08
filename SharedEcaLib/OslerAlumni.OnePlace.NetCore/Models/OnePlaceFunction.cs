using Newtonsoft.Json;
using OslerAlumni.OnePlace.Attributes;

namespace OslerAlumni.OnePlace.Models
{
    [OnePlaceObject("OnePlace__Function__c")]
    public class OnePlaceFunction
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}
