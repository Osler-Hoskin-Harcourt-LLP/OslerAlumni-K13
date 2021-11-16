using ECA.Core.Models;

namespace OslerAlumni.Mvc.Core.Models
{
    public class UrlConfig
        : IConfig
    {
        public bool RedirectToLowerCase { get; set; }

        public bool RemoveTrailingSlash { get; set; }
    }
}
