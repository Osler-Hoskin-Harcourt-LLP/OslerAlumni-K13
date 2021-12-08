using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Extensions;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Services
{
    public class ResourceService
        : ServiceBase, IResourceService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        private readonly IResourceTypeItemRepository _resourceTypeItemRepository;
        private readonly ContextConfig _context;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceService"/> class.
        /// </summary>
        /// <param name="documentRepository">The document repository.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="resourceTypeItemRepository"></param>
        public ResourceService(
            IDocumentRepository documentRepository,
            ICacheService cacheService,
            IResourceTypeItemRepository resourceTypeItemRepository,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
            _resourceTypeItemRepository = resourceTypeItemRepository;
            _context = context;
        }

        #region "Methods"

        /// <summary>
        /// Gets the featured resource.
        /// </summary>
        /// <param name="landingPage">The landing page.</param>
        /// <returns></returns>
        public PageType_Resource GetFeaturedResource(
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
                    GlobalConstants.Caching.Resources.FeaturedItemByLandingPage,
                    landingPage.NodeGUID),
                IsCultureSpecific = true,
                CultureCode = landingPage.DocumentCulture,
                IsSiteSpecific = true,
                SiteName = landingPage.NodeSiteName,
                // Bust the cache whenever the featured resource is modified for the resource landing page
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
                        .GetRelatedDocuments<PageType_Resource>(
                            landingPage,
                            featuredFieldName,
                            out relationships,
                            PageType_Resource.CLASS_NAME,
                            false,
                            new[]
                            {
                                nameof(PageType_Resource.Types),
                                nameof(PageType_Resource.Title),
                                nameof(PageType_Resource.ExternalUrl),
                                nameof(PageType_Resource.IsFile),
                                nameof(PageType_Resource.HideFromCompetitors)
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
                            // Bust the cache whenever the featured resource page itself is modified
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


        /// <inheritdoc />
        public string GetResourceTypesDisplayString(string resourceTypes, string culture = null)
        {
            var types = resourceTypes?.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            return GetResourceTypesDisplayString(types, culture);
        }

        public string GetResourceTypesDisplayString(string[] resourceTypeArray, string culture = null)
        {
             var resourceTypes = _resourceTypeItemRepository
                .GetByCodeNames(resourceTypeArray, culture)
                .WhereNotNull()
                .Select(rt => rt.DisplayName).ToList();

            resourceTypes.Sort();

            return resourceTypes.Join(", ");
        }

        public List<PageType_Resource> GetLatestResources(int top, bool filterForCompetitor)
        {
            if (top <= 0)
            {
                return null;
            }
            var cacheParameters = new CacheParameters
            {
                CacheKey = GlobalConstants.Caching.Prefix +
                                      $"{nameof(ResourceService)}|{nameof(GetLatestResources)}|top|{top}|filterForCompetitor|{filterForCompetitor}",
                AllowNullValue = false,
                CultureCode = _context.CultureName,
                CacheDependencies = new List<string>()
                {
                    string.Format(GlobalConstants.Caching.Pages.PagesByType, _context.Site.SiteName,
                        PageType_Resource.CLASS_NAME)
                }
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var whereCondition = new WhereCondition();

                    if (filterForCompetitor)
                    {
                        whereCondition = whereCondition.WhereFalse(nameof(PageType_Resource.HideFromCompetitors));
                    }

                    var featuredItems = _documentRepository
                        .GetDocuments(
                            pageTypeName: PageType_Resource.CLASS_NAME,
                            columnNames:
                            new[]
                            {
                                nameof(PageType_Resource.Title),
                                nameof(PageType_Resource.DatePublished),
                                nameof(PageType_Resource.ExternalUrl),
                                nameof(PageType_Resource.IsFile)
                            },
                            top: top,
                            orderDirection: OrderDirection.Descending,
                            orderByColumns: new[] { nameof(PageType_Resource.DatePublished) },
                            whereCondition: whereCondition
                        );

                    return featuredItems.Select(fi => fi.ToPageType<PageType_Resource>()).ToList();
                },
                cacheParameters);

            return result;
        }

        #endregion
    }
}
