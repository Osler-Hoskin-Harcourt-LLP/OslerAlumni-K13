using System;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class JobsDetailsPageViewModel 
        : BasePageViewModel
    {
        public string Company { get; set; }

        public string JobLocation { get; set; }

        public string JobClassification { get; set; }

        public string JobCategoryCodeName { get; set; }

        public string JobCategoryDisplayName { get; set; }

        public DateTime PostedDate { get; set; }


        public JobsDetailsPageViewModel(PageType_Job page)
            : base(page)
        {
            Company = page.Company;
            JobLocation = page.JobLocation;
            JobClassification = page.JobClassificationDisplay;
            JobCategoryCodeName = page.JobCategoryCodeName;
            PostedDate = page.PostedDate;
        }
    }
}
