using ECA.Core.Models;

namespace OslerAlumni.OnePlace.Models
{
    public class OnePlaceConfig
        : IConfig
    {
        public string Url { get; set; }

        public string ApiVersion { get; set; }

        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string SecurityToken { get; set; }
    }
}
