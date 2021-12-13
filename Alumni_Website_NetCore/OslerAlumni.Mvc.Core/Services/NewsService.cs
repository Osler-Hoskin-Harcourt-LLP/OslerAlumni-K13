using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Models;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class NewsService
        : ServiceBase, INewsService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        private readonly ContextConfig _context;

        #endregion

        public NewsService(
            IDocumentRepository documentRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
            _context = context;
        }

        #region "Methods"
        
        public PageType_News GetFeaturedNews(
            PageType_LandingPage landingPage)
        {
            if (landingPage == null)
            {
                return null;
            }

            var featuredFieldName =
                nameof(landingPage.Fields.FeaturedItems);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.News.FeaturedItemByLandingPage,
                    landingPage.NodeGUID),
                IsCultureSpecific = true,
                CultureCode = landingPage.DocumentCulture,
                IsSiteSpecific = true,
                SiteName = landingPage.NodeSiteName,
                // Bust the cache whenever the featured news is modified for the news landing page
                // or the page type relationship itself is changed for the Landing Page page type
                CacheDependencies = new List<string>
                {
                    $"nodeid|{landingPage.NodeID}|relationships",
                    string.Format(
                        ECAGlobalConstants.Caching.Classes.RelationshipByClassAndField,
                        PageType_LandingPage.CLASS_NAME,
                        featuredFieldName)
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    IList<RelationshipInfo> relationships;

                    var featuredItem = _documentRepository
                        .GetRelatedDocuments<PageType_News>(
                            landingPage,
                            featuredFieldName,
                            out relationships,
                            PageType_News.CLASS_NAME,
                            false,
                            new[]
                            {
                                nameof(PageType_News.Type),
                                nameof(PageType_News.Title),
                                nameof(PageType_News.Image),
                                nameof(PageType_News.ImageAltText),
                            })
                        ?.FirstOrDefault();

                    if (relationships != null)
                    {
                        // NOTE: We are using relationship information directly,
                        // so that cache dependencies are added even if
                        // the pages in the relationship are not currently in the published state,
                        // and are therefore not going to be part of the cached data
                        foreach (var relationship in relationships)
                        {
                            // Bust the cache whenever the featured news page itself is modified
                            cp.CacheDependencies.Add(
                                $"nodeid|{relationship.RightNodeId}");

                            // Bust the cache whenever the relationship itself
                            // (e.g. order of related pages) is modified
                            cp.CacheDependencies.Add(
                                $"{RelationshipInfo.OBJECT_TYPE_ADHOC}|byid|{relationship.RelationshipID}");
                        }
                    }

                    return featuredItem;
                },
                cacheParameters);

            return result;
        }



        public List<PageType_News> GetLatestNews(int top)
        {
            if (top <= 0)
            {
                return null;
            }

            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                           $"{nameof(NewsService)}|{nameof(GetLatestNews)}|top|{top}",
                AllowNullValue = false,
                CultureCode = LocalizationContext.CurrentCulture.CultureCode,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, SiteContext.CurrentSiteName,
                        PageType_News.CLASS_NAME)
                }
            };

            var result = _cacheService.Get(
                cp =>
                {

                    var featuredItems = _documentRepository
                        .GetDocuments(
                            pageTypeName: PageType_News.CLASS_NAME,
                            columnNames:
                            new[]
                            {
                                nameof(PageType_News.Title),
                                nameof(PageType_News.DatePublished)
                            },
                            top: top,
                            orderDirection: OrderDirection.Descending,
                            orderByColumns: new[] {nameof(PageType_News.DatePublished)});

                    return featuredItems.Select(fi => fi.ToPageType<PageType_News>()).ToList();
                },
                cacheParameters);

            return result;
        }
        #endregion
    }
}
