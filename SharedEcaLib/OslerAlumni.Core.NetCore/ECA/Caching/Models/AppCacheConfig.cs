using ECA.Core.Models;

namespace ECA.Caching.Models
{
    public class AppCacheConfig
        : IConfig
    {
        public bool Enabled { get; set; }

        public int CacheExpiryMinutes { get; set; }
    }
}
