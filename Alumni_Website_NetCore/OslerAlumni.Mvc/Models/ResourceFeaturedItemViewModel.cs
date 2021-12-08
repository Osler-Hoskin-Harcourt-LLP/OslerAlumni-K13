using System;

namespace OslerAlumni.Mvc.Models
{
    public class ResourceFeaturedItemViewModel
    {
        public Guid NodeGuid { get; set; }

        public string FeaturedText { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string ImageAltText { get; set; }

        public string DefaultImageUrl { get; set; }

        public string PageUrl { get; set; }

        public bool IsExternal { get; set; }

        public bool IsFile { get; set; }
    }
}
