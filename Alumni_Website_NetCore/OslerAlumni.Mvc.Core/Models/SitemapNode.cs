using System;

namespace OslerAlumni.Mvc.Core.Models
{
    /// <summary>
    /// Represents a page or URL in your sitemap.
    /// </summary>
    public sealed class SitemapNode
    { 
        public DateTime? LastModified { get; set; }

        public string Url { get; set; }
    }
}
