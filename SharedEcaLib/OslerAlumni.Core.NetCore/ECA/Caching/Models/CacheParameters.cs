using System.Collections.Generic;

namespace ECA.Caching.Models
{
    public class CacheParameters
    {
        #region "Private fields"

        private List<string> _cacheDependencies;

        #endregion

        #region "Properties"

        public bool AllowNullValue { get; set; } = false;

        public string CacheKey { get; set; }

        public string CultureCode { get; set; }

        public string SiteName { get; set; }

        public int? Duration { get; set; }
        
        public bool IsCultureSpecific { get; set; } = true;

        public bool IsSiteSpecific { get; set; } = true;

        public bool IsSlidingExpiration { get; set; } = false;

        public List<string> CacheDependencies
        {
            get
            {
                return _cacheDependencies ?? (_cacheDependencies = new List<string>());
            }
            set
            {
                _cacheDependencies = value;
            }
        }

        #endregion
    }
}
