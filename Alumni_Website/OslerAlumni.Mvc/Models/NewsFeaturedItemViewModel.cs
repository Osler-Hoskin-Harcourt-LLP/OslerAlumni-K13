using System;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Models
{
    public class NewsFeaturedItemViewModel
    {
        public Guid NodeGuid { get; set; }

        public NewsPageType NewsPageType { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string ImageAltText { get; set; }

        public string DefaultImageUrl { get; set; }

        public string PageUrl { get; set; }
    }
}