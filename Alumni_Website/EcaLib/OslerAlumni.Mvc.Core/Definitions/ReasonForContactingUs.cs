using System.ComponentModel.DataAnnotations;
using CMS.Helpers;
using OslerAlumni.Mvc.Core.Attributes.Html;

namespace OslerAlumni.Mvc.Core.Definitions
{
    public enum ReasonForContactingUs
    {
        [Display(Name = Constants.ResourceStrings.Form.ContactUs.ReasonForContactingUsGeneralInquiry)]
        [CustomHtmlAttributes(HtmlAttributes = "data-toggle:FileUpload")]
        [EnumStringRepresentation("GeneralInquiry")]
        GeneralInquiry,

        [CustomHtmlAttributes(HtmlAttributes = "data-toggle:FileUpload")]
        [Display(Name = Constants.ResourceStrings.Form.ContactUs.ReasonForContactingUsSubmitNews)]
        [EnumStringRepresentation("SubmitNews")]
        SubmitNews,

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.ReasonForContactingUsPostAnOpportunity)]
        [CustomHtmlAttributes(HtmlAttributes = "data-toggle:FileUpload;aria-expanded:false")]
        [EnumStringRepresentation("PostAnOpportunity")]
        PostAnOpportunity
    }
}
