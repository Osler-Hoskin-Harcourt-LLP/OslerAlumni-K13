using System;

namespace OslerAlumni.Mvc.Core.Models
{
    public class CtaViewModel
    {
        public string Title { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }

        public string LinkText { get; set; }

        public string Content { get; set; }

        public string ImageAltText { get; set; }

        public bool? IsExternal { get; set; }

        public Guid CtaGuid { get; set; }
        public Guid? LinkedPageGuid { get; set; }
    }
}
