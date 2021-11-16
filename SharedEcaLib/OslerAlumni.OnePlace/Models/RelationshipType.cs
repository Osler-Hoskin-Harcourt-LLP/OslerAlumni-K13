using Newtonsoft.Json;
using OslerAlumni.OnePlace.Attributes;

namespace OslerAlumni.OnePlace.Models
{
    [OnePlaceObject("RecordType")]
    public class RelationshipType
    {
        #region "Properties"

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("DeveloperName")]
        public string CodeName { get; set; }

        [JsonProperty("Description")]
        public string DisplayName { get; set; }

        #endregion
    }
}
