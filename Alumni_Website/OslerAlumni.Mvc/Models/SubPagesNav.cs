namespace OslerAlumni.Mvc.Models
{
    public class SubPagesNav
    {
        public string NodeAlias { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }

        public bool IsSelected { get; set; }
    }
}
