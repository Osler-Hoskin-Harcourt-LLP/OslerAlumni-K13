using System;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Models
{
    public class NewsDetailsPageViewModel 
        : BasePageViewModel
    {
        public NewsPageType NewsPageType { get; set; }

        public DateTime DatePublished { get; set; }

        public string ImageUrl { get; set; }

        public string ImageAltText { get; set; }

        public string VitalsHeader { get; set; }

        public string Vitals { get; set; }

        public string StoryHighlights { get; set; }

        public NewsDetailsPageViewModel(PageType_News page)
            : base(page)
        {
            NewsPageType = page.NewsPageType;
            DatePublished = page.DatePublished;
            ImageUrl = page.Image;
            ImageAltText = page.ImageAltText;
            StoryHighlights = page.StoryHighlights;
            VitalsHeader = page.VitalsHeader;
            Vitals = page.Vitals;
        }
    }
}
