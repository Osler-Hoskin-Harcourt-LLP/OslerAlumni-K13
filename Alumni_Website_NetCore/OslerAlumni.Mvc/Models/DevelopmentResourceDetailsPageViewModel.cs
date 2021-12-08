using System;

using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class DevelopmentResourceDetailsPageViewModel
        : BasePageViewModel
    {
        public DateTime DatePublished { get; set; }

        public string Authors { get; set; }

        public string ResourceTypeDisplayName { get; set; }

        public DevelopmentResourceDetailsPageViewModel(PageType_DevelopmentResource page)
            : base(page)
        {
            DatePublished = page.DatePublished;
            Authors = page.Authors;
        }
    }
}
