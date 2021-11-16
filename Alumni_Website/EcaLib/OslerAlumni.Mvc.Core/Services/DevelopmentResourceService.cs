using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Services
{
    public class DevelopmentResourceService
        : ServiceBase, IDevelopmentResourceService
    {
        #region "Private fields"

        private readonly IDevelopmentResourceTypeItemRepository _developmentResourceTypeItemRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceService"/> class.
        /// </summary>
        /// <param name="developmentResourceTypeItemRepository"></param>
        /// <param name="documentRepository">The document repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public DevelopmentResourceService(
            IDevelopmentResourceTypeItemRepository developmentResourceTypeItemRepository,
            IDocumentRepository documentRepository,
            ICacheService cacheService)
        {
            _developmentResourceTypeItemRepository = developmentResourceTypeItemRepository;
            _documentRepository = documentRepository;
            _cacheService = cacheService;
        }

        #region "Methods"

        /// <summary>
        /// Gets the featured resource.
        /// </summary>
        /// <param name="landingPage">The landing page.</param>
        /// <returns></returns>
        public PageType_DevelopmentResource GetFeaturedDevelopmentResource(
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
                    GlobalConstants.Caching.DevelopmentResources.FeaturedItemByLandingPage,
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
                        .GetRelatedDocuments<PageType_DevelopmentResource>(
                            landingPage,
                            featuredFieldName,
                            out relationships,
                            PageType_DevelopmentResource.CLASS_NAME,
                            false,
                            new[]
                            {
                                nameof(PageType_DevelopmentResource.DevelopmentResourceTypes),
                                nameof(PageType_DevelopmentResource.Title),
                                nameof(PageType_DevelopmentResource.ExternalUrl),
                                nameof(PageType_DevelopmentResource.IsFile)
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
            var resourceTypes = _developmentResourceTypeItemRepository
                .GetByCodeNames(resourceTypeArray, culture)
                .WhereNotNull()
                .Select(rt => rt.DisplayName).ToList();

            resourceTypes.Sort();

            return resourceTypes.Join(", ");
        }
        #endregion
    }
}
