using System;
using CMS.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Api.Attributes;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    /// <summary>
    /// Class for the individual news articles,
    /// as represented in the search results from the search API.
    /// </summary>
    [SearchPageType(PageType_Event.CLASS_NAME)]
    public class Event
        : Page, ISearchable
    {
        /// <summary>
        /// Date and time of the start date of the event.
        /// </summary>
        [JsonProperty("startDate")]
        [AzureSearchField(nameof(PageType_Event.StartDate))]
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Date and time of the end date of the event.
        /// </summary>
        [JsonProperty("endDate")]
        [AzureSearchField(nameof(PageType_Event.EndDate))]
        public DateTimeOffset EndDate { get; set; }


        /// <summary>
        /// Location of the Event.
        /// </summary>
        [JsonIgnore]
        [AzureSearchField(nameof(PageType_Event.Location))]
        public Guid Location { get; set; }


        /// <summary>
        /// True if event is hosted by Osler.
        /// </summary>
        [JsonProperty("hostedByOsler")]
        [AzureSearchField(nameof(PageType_Event.HostedByOsler))]
        public bool HostedByOsler { get; set; }

        /// <summary>
        /// Display Date of the event
        /// </summary>
        [JsonProperty("displayDate")]
        [AzureSearchField(nameof(PageType_Event.DisplayDate))]
        public string DisplayDate { get; set; }


        /// <summary>
        /// Display Time of the event
        /// </summary>
        [JsonProperty("displayTime")]
        [AzureSearchField(nameof(PageType_Event.DisplayTime))]
        public string DisplayTime { get; set; }


        /// <summary>
        /// Link to the event page
        /// </summary>
        [JsonProperty("eventUrl")]
        [AzureSearchField(nameof(PageType_Event.ExternalURL))]
        public string EventUrl { get; set; }


        /// <summary>
        /// Type of Event. eg) Webinar, In-Person, etc
        /// </summary>
        [JsonProperty("deliveryMethods")]
        [JsonConverter(typeof(StringEnumConverter))]
        [AzureSearchField(nameof(PageType_Event.DeliveryMethods))]
        public DeliveryMethods DeliveryMethods { get; set; }


        /// <summary>
        /// Type of Event. eg) Webinar, In-Person, etc
        /// </summary>
        [JsonProperty("deliveryMethodsDisplay")]
        public string DeliveryMethodsDisplay
            => ResHelper.GetString(((Enum) DeliveryMethods).GetDisplayName(), Culture);


        /// <summary>
        /// Display value for Location
        /// </summary>
        [JsonProperty("location")]
        public string LocationDisplay { get; set; }


        /// <summary>
        /// City of Event
        /// </summary>
        [JsonProperty("city")]
        [AzureSearchField(nameof(PageType_Event.City))]
        public string City { get; set; }

        /// <summary>
        /// OnePlace Function Reference Id
        /// </summary>
        [JsonProperty("onePlaceFunctionId")]
        [AzureSearchField(nameof(PageType_Event.OnePlaceFunctionId))]
        public string OnePlaceFunctionId { get; set; }

        /// <summary>
        /// Should show list of attendees
        /// </summary>
        [JsonProperty("showAttendees")]
        [AzureSearchField(nameof(PageType_Event.ShowAttendees))]
        public bool ShowAttendees { get; set; }


        [JsonIgnore]
        [AzureSearchField(nameof(PageType_Event.SortOrder))]
        public int SortOrder { get; set; }

        [JsonIgnore]
        [AzureSearchField(nameof(PageType_Event.SortDummyDateTimeTicks))]
        public long SortDummyDateTimeTicks { get; set; }
    }
}
