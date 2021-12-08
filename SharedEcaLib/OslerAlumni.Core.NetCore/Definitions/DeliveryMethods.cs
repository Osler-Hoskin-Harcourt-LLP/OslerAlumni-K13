using System.ComponentModel.DataAnnotations;

namespace OslerAlumni.Core.Definitions
{
    public enum DeliveryMethods
    {
        [Display(Name = GlobalConstants.ResourceStrings.Events.DeliveryMethods.InPerson)]
        InPerson,
        [Display(Name = GlobalConstants.ResourceStrings.Events.DeliveryMethods.InPersonWebinar)]
        InPersonWebinar,
        [Display(Name = GlobalConstants.ResourceStrings.Events.DeliveryMethods.Webinar)]
        Webinar,
        [Display(Name = GlobalConstants.ResourceStrings.Events.DeliveryMethods.WebinarOnDemand)]
        WebinarOnDemand
    }
}
