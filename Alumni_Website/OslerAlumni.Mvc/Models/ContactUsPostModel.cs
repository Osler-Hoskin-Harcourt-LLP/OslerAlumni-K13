using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using CMS.Helpers;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Kentico.Models.Forms;

namespace OslerAlumni.Mvc.Models
{
    public class ContactUsPostModel : IContactUsFormItem
    {

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.ReasonForContactingUs)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.ReasonForContactingUsRequired)]
        public ReasonForContactingUs ReasonForContactingUsEnum { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.OpportunityType)]
        [RequiredIf(nameof(ReasonForContactingUsEnum),
            ReasonForContactingUs.PostAnOpportunity,
            ErrorMessage = Constants.ResourceStrings.Form.ContactUs.OpportunityTypeRequired)]
        public string OpportunityType { get; set; }

        public string OpportunityTypeDisplay =>
            (OpportunityType.ToEnum<ContactUsOpportunityTypes>()).GetLocalizedDisplayName();

        public ContactUsOpportunityTypes OpportunityTypesEnum { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.FirstName)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.FirstNameRequired)]
        [LocalizedMaxLength(200)]
        public string FirstName { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.LastName)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.LastNameRequired)]
        [LocalizedMaxLength(200)]
        public string LastName { get; set; }

        public string ReasonForContactingUsDisplay
            => ((Enum) ReasonForContactingUsEnum).GetLocalizedDisplayName();

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.CompanyName)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.CompanyNameRequired)]
        [LocalizedMaxLength(255)]
        public string CompanyName { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.PhoneNumber)]
        [LocalizedMaxLength(40, ErrorMessage = Constants.ResourceStrings.Form.ContactUs.PhoneNumberError)]
        public string PhoneNumber { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.Email)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.EmailRequired)]
        [EmailValidation(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.EmailError)]
        [LocalizedMaxLength(200)]
        public string Email { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.ContactUs.Subject)]
        [LocalizedMaxLength(200, ErrorMessage = Constants.ResourceStrings.Form.ContactUs.SubjectError)]
        public string Subject { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.ContactUs.Message)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.MessageRequired)]
        [LocalizedMaxLength(750, ErrorMessage = Constants.ResourceStrings.Form.ContactUs.MessageError)]
        public string Message { get; set; }

        [Description(Constants.ResourceStrings.Form.ContactUs.FileUploadExplanation)]
        [Display(Name = Constants.ResourceStrings.Form.ContactUs.FileUpload)]
        [FileTypeValidation(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.FileUploadError)]
        [MaxFileSizeValidation(ErrorMessage = Constants.ResourceStrings.Form.ContactUs.FileUploadError)]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileUpload { get; set; }

        [Required(ErrorMessage = Constants.ResourceStrings.Form.GlobalCaptchaRequired)]
        [ValidateGoogleCaptcha(ErrorMessage = Constants.ResourceStrings.Form.GlobalCaptchaError)]
        public string GoogleCaptchaUserResponse { get; set; }

        public string Attachment { get; set; }

        public int? ContactUsId { get; set; }

        public string UserName { get; set; }

        public Guid? UserGuid { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
