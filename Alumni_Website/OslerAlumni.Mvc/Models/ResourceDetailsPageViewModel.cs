using System;
using System.Collections.Generic;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class ResourceDetailsPageViewModel
        : BasePageViewModel
    {
        public DateTime DatePublished { get; set; }

        public string Authors { get; set; }

        public string ResourceTypeDisplayName { get; set; }

        public ResourceDetailsPageViewModel(PageType_Resource page)
            : base(page)
        {
            DatePublished = page.DatePublished;
            Authors = page.Authors;
        }
    }
}
