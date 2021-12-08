using System;
using System.Collections.Generic;
using ECA.Core.Extensions;
using Newtonsoft.Json;
using OslerAlumni.OnePlace.Attributes;
using OslerAlumni.OnePlace.Serialization;
using Salesforce.Common.Models.Json;

namespace OslerAlumni.OnePlace.Models
{
    [OnePlaceObject("Contact")]
    public class Contact
    {
        #region "Constants"

        public const string DeceasedStatus = "Deceased";

        #endregion

        #region "Properties"

        [JsonProperty("Id")]
        public string Id { get; set; }

        #region "Alumni"

        [JsonProperty("z_LastModifiedDateRel_Acc__c")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastModifiedDate { get; set; }

        [JsonProperty("OnePlace__Alumni__c")]
        public bool? IsAlumni { get; set; }

        [JsonProperty("OnePlace__Bad_Terms__c")]
        public bool? HasLeftOnBadTerms { get; set; }

        [JsonProperty("OnePlace__Status__c")]
        public string Status { get; set; }

        [JsonIgnore]
        public bool IsDeceased
            => string.Equals(Status, DeceasedStatus, StringComparison.OrdinalIgnoreCase);

        [JsonProperty("Competitor_Contact__c")]
        public bool? IsCompetitor { get; set; }

        [JsonProperty("OnePlace__Joined__c")]
        public DateTime? StartDateAtOsler { get; set; }

        [JsonProperty("OnePlace__Left__c")]
        public DateTime? EndDateAtOsler { get; set; }

        #endregion

        #region "Personal"

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("OnePlace__Preferred_Name__c")]
        public string GoesBy { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Title")]
        public string JobTitle { get; set; }

        [JsonProperty("AccountId")]
        public string AccountId { get; set; }

        [JsonProperty("Account")]
        public Account Account { get; set; }

        [JsonProperty("MailingCity")]
        public string City { get; set; }

        [JsonProperty("MailingState")]
        public string StateProvince { get; set; }

        [JsonProperty("MailingCountry")]
        public string Country { get; set; }

        #endregion

        #region Education

        [JsonProperty("OnePlace__Education_Overview__c")]
        public string EducationOverview { get; set; }

        #endregion

        #region "Year of Call and Jurisdictions"

        [JsonProperty("OnePlace__Date_Admitted__c")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? YearOfCall { get; set; }

        [JsonProperty("OnePlace__Jurisdictions__c")]
        public string Jurisdictions { get; set; }

        #endregion

        #region "Osler Information"

        [JsonProperty("OnePlace__Department__c")]
        public string PracticeAreas { get; set; }

        [JsonProperty("OnePlace__Offices_Working__c")]
        public string OfficeLocations { get; set; }

        #endregion

        #region "Board Memberships"

        [JsonProperty("OnePlace__FromRelationships__r")]
        public QueryResult<BoardMembership> BoardMemberships { get; set; }

        [JsonIgnore]
        public IList<BoardMembership> BoardMembershipList
            => BoardMemberships?.Records ?? new List<BoardMembership>();

        #endregion

        #region "Contact Merging"
        
        [JsonProperty("OnePlace__Children__c")]
        public string MergedContactIds { get; set; }

        [JsonIgnore]
        public IList<string> MergedContactIdList
            => (MergedContactIds ?? string.Empty).SplitOn(';');

        #endregion

        #region Preferences

        [JsonProperty("OnePlace__Unsubscribe_Comms__c")]
        public bool? UnsubscribeCommunications { get; set; }

        [JsonProperty("OnePlace__Subscription_Preferences__c")]
        public string SubscriptionPreferences { get; set; }

        [JsonProperty("OnePlace__Communication_Prefs__c")]
        public string CommunicationPreferences { get; set; }

        #endregion

        #endregion
    }
}
