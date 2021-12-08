using System;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class BoardOpportunityDetailsPageViewModel
        : BasePageViewModel
    {
        public string Company { get; set; }

        public string BoardOpportunityLocation { get; set; }


        public string JobCategoryCodeName { get; set; }

        public string JobCategoryDisplayName { get; set; }

        public string BoardOpportunityTypeDisplayName { get; set; }


        public string SourceDisplayName { get; set; }
        public string SourceCompanyLogo { get; set; }
        public string SourceCompanyLogoAltText { get; set; }
        public DateTime PostedDate { get; set; }


        public BoardOpportunityDetailsPageViewModel(PageType_BoardOpportunity page)
            : base(page)
        {
            Company = page.Company;
            BoardOpportunityLocation = page.BoardOpportunityLocation;
            JobCategoryCodeName = page.JobCategoryCodeName;
            PostedDate = page.PostedDate;
        }
    }
}
