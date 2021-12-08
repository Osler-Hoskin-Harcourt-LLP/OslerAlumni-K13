using System;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Kentico.Models.Forms
{
    public interface IContactUsFormItem
    {
        int? ContactUsId { get; set; }
        string UserName { get; set; }
        Guid? UserGuid { get; set; }
        ReasonForContactingUs ReasonForContactingUsEnum { get; set; }
        string ReasonForContactingUsDisplay { get; }
        string CompanyName { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Message { get; set; }
        string PhoneNumber { get; set; }
        string Subject { get; set; }
        string Attachment { get; set; }

        string OpportunityType { get; set; }
        string OpportunityTypeDisplay { get; }


    }
}
