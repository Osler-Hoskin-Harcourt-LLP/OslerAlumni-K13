using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Services;
using org.pdfclown.objects;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class SitemapService : ServiceBase, ISitemapService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapService"/> class.
        /// </summary>
        /// <param name="documentRepository">The document repository.</param>
        /// <param name="mapper">The mapper.</param>
        public SitemapService(IDocumentRepository documentRepository, ICacheService cacheService)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Gets the sitemap.
        /// </summary>
        /// <returns></returns>
        public string GetSitemap(string culture)
        {
            return _cacheService.Get(() =>
            {
                IEnumerable<TreeNode> documents = new List<TreeNode>();

                if (culture == "en")
                {
                    documents = _documentRepository
                    .GetDocuments(cultureName: "en-CA", columnNames: new string[] { nameof(TreeNode.DocumentModifiedWhen) },
                        whereCondition: new WhereCondition($"ClassName IN ('{PageType_DevelopmentResource.CLASS_NAME}', '{PageType_Page.CLASS_NAME}', '{PageType_BoardOpportunity.CLASS_NAME}', '{PageType_Home.CLASS_NAME}', '{PageType_Job.CLASS_NAME}', '{PageType_LandingPage.CLASS_NAME}', '{PageType_News.CLASS_NAME}', '{PageType_Page.CLASS_NAME}', '{PageType_Profile.CLASS_NAME}')"))
                    .Where(n => DocumentURLProvider.GetUrl(n) != "~/");
                }

                if (culture == "fr")
                {
                    documents = _documentRepository
                     .GetDocuments(cultureName: "fr-CA", columnNames: new string[] { nameof(TreeNode.DocumentModifiedWhen) },
                         whereCondition: new WhereCondition($"ClassName IN ('{PageType_DevelopmentResource.CLASS_NAME}', '{PageType_Page.CLASS_NAME}', '{PageType_BoardOpportunity.CLASS_NAME}', '{PageType_Home.CLASS_NAME}', '{PageType_Job.CLASS_NAME}', '{PageType_LandingPage.CLASS_NAME}', '{PageType_News.CLASS_NAME}', '{PageType_Page.CLASS_NAME}', '{PageType_Profile.CLASS_NAME}')"))
                     .Where(n => DocumentURLProvider.GetUrl(n) != "~/");
                }

                return GetSitemapDocument(documents.Select(n => new SitemapNode() { Url = DocumentURLProvider.GetAbsoluteLiveSiteURL(n), LastModified = n.DocumentModifiedWhen }));
            }, new CacheParameters() { CacheKey = nameof(GetSitemap) + culture, Duration = 600 });

        }

        #region "Private Methods"

        /// <summary>
        /// Gets the sitemap document.
        /// </summary>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <returns></returns>
        private string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(xmlns + "urlset");
            foreach (var sitemapNode in sitemapNodes)
            {
                var urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(
                        $"{sitemapNode.Url}")),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")));
                root.Add(urlElement);
            }

            var document = new XDocument(root);
            return document.ToString();
        }

        #endregion
    }
}
