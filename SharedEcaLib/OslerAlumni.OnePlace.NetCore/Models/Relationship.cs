using Newtonsoft.Json;
using OslerAlumni.OnePlace.Attributes;

namespace OslerAlumni.OnePlace.Models
{
    [OnePlaceObject("OnePlace__Relationship__c")]
    public class Relationship
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("RecordTypeId")]
        public string RelationshipTypeId { get; set; }

        [JsonProperty("RecordType")]
        public RelationshipType RelationshipType { get; set; }

        [JsonProperty("OnePlace__Contact_From__c")]
        public string FromContactId { get; set; }

        [JsonProperty("OnePlace__Account_To__c")]
        public string ToAccountId { get; set; }

        [JsonProperty("OnePlace__Account_To__r")]
        public Account ToAccount { get; set; }
    }
}
