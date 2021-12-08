using System.ComponentModel.DataAnnotations;
using CMS.Helpers;

namespace OslerAlumni.Mvc.Core.Definitions
{
    public enum ContactUsOpportunityTypes
    {
        [Display(Name = Constants.ResourceStrings.Form.ContactUs.OpportunityTypeDefaultOption)]
        [EnumStringRepresentation("")]
        SelectOpportunityType,
        
        [Display(Name = Constants.ResourceStrings.Form.ContactUs.OpportunityTypeJob)]
        [EnumStringRepresentation("JobOpportunity")]
        JobOpportunity,

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.OpportunityTypeBoard)]
        [EnumStringRepresentation("BoardOpportunity")]
        BoardOpportunity
    }
}
