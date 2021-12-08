using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipOslerInfoFormModel
    {
        [Display(Name = Constants.ResourceStrings.Form.OslerInformation.YearsAtOsler)]
        public string YearsAtOsler { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.OslerInformation.OslerLocations)]
        public List<string> OslerLocations { get; set; }
        [Display(Name = Constants.ResourceStrings.Form.OslerInformation.OslerPracticeAreas)]
        public List<string> OslerPracticeAreas { get; set; }
    }
}
