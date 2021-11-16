using System;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipPreferencesFormModel
    {
        public Guid? UserGuid { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipPreferences.IncludeEmailInDirectory)]
        public bool IncludeEmailInDirectory { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipPreferences.DisplayImageInDirectory)]
        public bool DisplayImageInDirectory { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipPreferences.SubscribeToEmailUpdates)]
        public bool SubscribeToEmailUpdates { get; set; }
        
    }
}
