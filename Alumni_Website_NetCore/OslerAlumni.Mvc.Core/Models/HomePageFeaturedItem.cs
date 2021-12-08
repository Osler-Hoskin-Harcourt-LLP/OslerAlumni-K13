namespace OslerAlumni.Mvc.Core.Models
{
    public class HomePageFeaturedItem
    {
        public string Url { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsExternal { get; set; }

        public bool IsFile { get; set; }
    }
}
