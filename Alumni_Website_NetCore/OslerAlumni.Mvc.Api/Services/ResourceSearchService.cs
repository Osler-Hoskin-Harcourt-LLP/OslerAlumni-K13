using System.Collections.Generic;
using System.Linq;
using CMS.Helpers;
using CMS.Localization;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Api.Services
{
    public class ResourceSearchService
        : BaseSearchService<ResourceSearchRequest, Resource>
    {
        #region "Private fields"

        private readonly IResourceService _resourceService;
        private readonly IResourceTypeItemRepository _resourceTypeItemRepository;

        #endregion

        public ResourceSearchService(
            IResourceService resourceService,
            IResourceTypeItemRepository resourceTypeItemRepository,
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        {
            _resourceService = resourceService;
            _resourceTypeItemRepository = resourceTypeItemRepository;
        }

        #region "Helper methods"

        protected override SearchResponse<Resource> GetSearchResults(
            ResourceSearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            UpdateResourceTypeDisplay(
                searchResponse.Items,
                searchRequest.Culture,
                cacheParameters);

            //Even though it is another iteration on the list, I feel it is cleaner this way.
            ResolveExternalUrls(searchResponse.Items);

            if (!searchRequest.IncludeFilters)
            {
                return searchResponse;
            }

            SetFilters(
                searchResponse,
                searchRequest.Culture,
                cacheParameters);

            return searchResponse;
        }

        protected override string GetSearchResultsCacheKey(
            ResourceSearchRequest searchRequest)
        {
            return string.Format(
                GlobalConstants.Caching.Search.SearchResultsBySearchRequestWithFitlers,
                searchRequest.PageTypes.JoinSorted(
                    GlobalConstants.Caching.Separator,
                    GlobalConstants.Caching.All),
                searchRequest.IndexName,
                searchRequest.PageSize,
                searchRequest.Skip,
                searchRequest.OrderBy,
                searchRequest.OrderByDirection,
                searchRequest.ExcludedNodeGuids?
                    .Select(guid => guid.ToString())
                    .ToArray()
                    .JoinSorted(
                        GlobalConstants.Caching.Separator,
                        string.Empty),
                searchRequest.FilterForCompetitor,
                searchRequest.IncludeFilters);
        }

        protected void SetFilters(
            SearchResponse<Resource> response,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            if (response == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            var resourceTypeFilter = new SearchFilter
            {
                FieldName =
                    nameof(ResourceSearchRequest.ResourceTypes),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Resource.CategoriesFilterTitle,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,
                Options =
                    _resourceTypeItemRepository.GetAllResourceTypeItems(cultureName)?
                        .Select(filter => new SearchFilterOption
                        {
                            CodeName = filter.CodeName,
                            DisplayName = filter.DisplayName
                        })
                        .OrderBy(sf => sf?.DisplayName)
                        .ToList()
            };

            response.Filters = new List<SearchFilter>
            {
                resourceTypeFilter,
            };

            // Bust the cache if any of the resource types or associated resource strings is modified
            cacheParameters?.CacheDependencies
                .AddRange(new[]
                {
                    $"{ResourceStringInfo.OBJECT_TYPE}|byname|{Constants.ResourceStrings.Resource.CategoriesFilterTitle}",
                    _cacheService.GetCacheKey(
                        GlobalConstants.Caching.Resources.ResourceTypeItemsAll,
                        cultureName)
                });
        }

        protected void UpdateResourceTypeDisplay(
            List<Resource> resourceList,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            if ((resourceList == null) || (resourceList.Count < 1))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            resourceList.ForEach(resource =>
            {
                resource.ResourceTypes = _resourceService.GetResourceTypesDisplayString(resource.ResourceTypes, cultureName);
            });

            // Bust the cache if any of the resource types or associated resource strings is modified
            cacheParameters?.CacheDependencies
                .Add(_cacheService.GetCacheKey(
                    GlobalConstants.Caching.Resources.ResourceTypeItemsAll,
                    cultureName));
        }


        protected void ResolveExternalUrls(
            List<Resource> resourceList)
        {
            resourceList?
                .ForEach(resource =>
                    resource.ExternalUrl = URLHelper.ResolveUrl(resource.ExternalUrl));
        }

        #endregion
    }
}
