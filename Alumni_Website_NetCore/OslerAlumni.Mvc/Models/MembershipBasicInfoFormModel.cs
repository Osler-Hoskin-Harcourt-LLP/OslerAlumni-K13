using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipBasicInfoFormModel
    {
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.FirstName)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.FirstNameRequired)]
        [LocalizedMaxLength(40)]
        public string FirstName { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.LastName)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.LastNameRequired)]
        [LocalizedMaxLength(80)]
        public string LastName { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.Email)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.EmailRequired)]
        [EmailValidation(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.EmailError)]
        [LocalizedMaxLength(80)]
        public string Email { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.AlumniEmail)]
        [EmailValidation(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.EmailError)]
        [LocalizedMaxLength(80)]
        public string AlumniEmail { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.CompanyName)]
        //[Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.CompanyNameRequired)]
        [LocalizedMaxLength(255)]
        public string CompanyName { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.JobTitle)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.JobTitleRequired)]
        [LocalizedMaxLength(128)]
        public string JobTitle { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.City)]
        [LocalizedMaxLength(40)]
        public string City { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.Province)]
        [LocalizedMaxLength(80)]
        public string Province { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.Country)]
        [LocalizedMaxLength(80)]
        public string Country { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.BarAdmissionAndJurisdiction)]
        public IEnumerable<YearAndJurisdiction> YearOfCallAndJurisdictions { get; set; }
        
        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.CurrentIndustry)]
        public string CurrentIndustry { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.LinkedInUrl, Description = Constants.ResourceStrings.Form.MembershipBasicInfo.LinkedInUrlExplanation)]        
        [RegularExpression(GlobalConstants.RegexExpressions.LinkedInUrlRegex, ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.LinkedInUrlError)]
        [LocalizedMaxLength(200)]
        public string LinkedInUrl { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.TwitterUrl, Description = Constants.ResourceStrings.Form.MembershipBasicInfo.TwitterUrlExplanation)]
        [RegularExpression(GlobalConstants.RegexExpressions.TwitterUrlRegex, ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.TwitterUrlError)]
        [LocalizedMaxLength(200)]
        public string TwitterUrl { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.InstagramUrl, Description = Constants.ResourceStrings.Form.MembershipBasicInfo.InstagramUrlExplanation)]
        [RegularExpression(GlobalConstants.RegexExpressions.InstagramUrlRegex, ErrorMessage = Constants.ResourceStrings.Form.MembershipBasicInfo.InstagramUrlError)]
        [LocalizedMaxLength(200)]
        public string InstagramUrl { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.EducationHistory)]
        public List<EducationRecord> EducationHistory { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBasicInfo.UserName)]
        public string UserName { get; set; }

        public Guid? UserGuid { get; set; }

    }
}
