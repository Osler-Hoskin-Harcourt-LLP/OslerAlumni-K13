using System.ComponentModel.DataAnnotations;

namespace OslerAlumni.Mvc.Core.Definitions
{
    public enum JobClassification
    {
        [Display(Name = Constants.ResourceStrings.Jobs.JobClassification.Osler)]
        Osler = 0,
        [Display(Name = Constants.ResourceStrings.Jobs.JobClassification.Marketplace)]
        Marketplace = 1
    }
}
