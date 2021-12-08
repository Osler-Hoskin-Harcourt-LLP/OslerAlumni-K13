using System;
using System.Collections.Generic;
using CMS.Membership;
using CMS.SiteProvider;

namespace ECA.Core.Models
{
    public sealed class ContextConfig
        : IDisposable
    {
        #region "Properties"

        public string CultureName { get; set; }

        public UserInfo User { get; set; }

        public SiteInfo Site { get; set; }

        public bool IsPreviewMode { get; set; }

        public Dictionary<string, string> AllowedCultureCodes { get; set; }

        public string BasePageType { get; set; }

        #endregion

        #region "Methods"

        public void Dispose()
        {
            // Don't have any unmanaged resources, so do nothing
        }

        #endregion
    }
}
