namespace ECA.Mvc.Navigation.Models
{
    public class NavigationItem
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public bool? IsExternal { get; set; }

        public string NodeAliasPath { get; set; }

        public bool IsSelected { get; set; }
    }
}
