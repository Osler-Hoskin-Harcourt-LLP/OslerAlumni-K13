using Newtonsoft.Json;
using OslerAlumni.OnePlace.Attributes;

namespace OslerAlumni.OnePlace.Models
{
    [OnePlaceObject("OnePlace__Invitee__c")]
    public class OnePlaceFunctionInvitee
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("OnePlace__Contact__c")]
        public string ContactId { get; set; }

        [JsonProperty("OnePlace__Contact__r")]
        public Contact Contact { get; set; }

        [JsonProperty("OnePlace__Function__c")]
        public string OnePlaceFunctionId { get; set; }

        [JsonProperty("RSVPs__c")]
        public string RsvpStatus { get; set; }

    }
}
