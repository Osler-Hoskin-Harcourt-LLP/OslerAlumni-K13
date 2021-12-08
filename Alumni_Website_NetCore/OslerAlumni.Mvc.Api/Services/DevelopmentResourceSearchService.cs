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
    public class DevelopmentResourceSearchService
        : BaseSearchService<DevelopmentResourceSearchRequest, DevelopmentResource>
    {

        #region "Private fields"

        private readonly IDevelopmentResourceTypeItemRepository _developmentResourceTypeItemRepository;
        private readonly IDevelopmentResourceService _developmentResourceService;

        #endregion

        public DevelopmentResourceSearchService(
            IDevelopmentResourceTypeItemRepository developmentResourceTypeItemRepository,
            IDevelopmentResourceService developmentResourceService,
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        {
            _developmentResourceTypeItemRepository = developmentResourceTypeItemRepository;
            _developmentResourceService = developmentResourceService;
        }

        #region "Helper methods"

        protected override SearchResponse<DevelopmentResource> GetSearchResults(
            DevelopmentResourceSearchRequest searchRequest,
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
            DevelopmentResourceSearchRequest searchRequest)
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
            SearchResponse<DevelopmentResource> response,
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
                    nameof(DevelopmentResourceSearchRequest.DevelopmentResourceTypes),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.DevelopmentResource.CategoriesFilterTitle,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,
                Options =
                    _developmentResourceTypeItemRepository.GetAllDevelopmentResourceTypeItems(cultureName)?
                        .Select(filter => new SearchFilterOption
                        {
                            CodeName = filter.CodeName,
                            DisplayName = filter.DisplayName
                        })
                        .OrderBy(sf=>sf?.DisplayName)
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
                    $"{ResourceStringInfo.OBJECT_TYPE}|byname|{Constants.ResourceStrings.DevelopmentResource.CategoriesFilterTitle}",
                    _cacheService.GetCacheKey(
                        GlobalConstants.Caching.DevelopmentResources.DevelopmentResourceTypeItemsAll,
                        cultureName)
                });
        }

        protected void UpdateResourceTypeDisplay(
            List<DevelopmentResource> resourceList,
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

            resourceList.ForEach(developmentResource =>
                {
                    developmentResource.DevelopmentResourceTypes =
                        _developmentResourceService.GetResourceTypesDisplayString(developmentResource
                            .DevelopmentResourceTypes, cultureName);
                });

            // Bust the cache if any of the resource types or associated resource strings is modified
            cacheParameters?.CacheDependencies
                .Add(_cacheService.GetCacheKey(
                    GlobalConstants.Caching.DevelopmentResources.DevelopmentResourceTypeItemsAll,
                    cultureName));
        }


        protected void ResolveExternalUrls(
            List<DevelopmentResource> resourceList)
        {
            resourceList?
                .ForEach(resource =>
                    resource.ExternalUrl = URLHelper.ResolveUrl(resource.ExternalUrl));
        }

        #endregion

    }
}
