using System.Collections.Generic;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using OslerAlumni.Mvc.Api.Definitions;
using OslerAlumni.Mvc.Api.Helpers;
using OslerAlumni.Mvc.Api.Models;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Api.Services
{
    public class GlobalSearchService
        : BaseSearchService<GlobalSearchRequest, GlobalResult>
    {
        public GlobalSearchService(
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        { }

        protected override SearchResponse<GlobalResult> GetSearchResults(
            GlobalSearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            ResolveExternalUrls(searchResponse.Items);

            if (!searchRequest.IncludeFilters)
            {
                return searchResponse;
            }

            SetFilters(searchResponse, searchRequest.Culture);

            return searchResponse;
        }

        private void SetFilters(
            SearchResponse<GlobalResult> response,
            string cultureName)
        {
            if (response == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }

            response.Filters = new List<SearchFilter>
            {
                GetPageTypeFilter(cultureName),
            };
        }

        private SearchFilter GetPageTypeFilter(string cultureName)
        {
            return new SearchFilter
            {
                FieldName =
                    nameof(GlobalSearchRequest.PageTypesFilter),
                Title =
                    ResHelper.GetString(
                        Constants.ResourceStrings.Search.PageTypeFilters.Title,
                        cultureName),
                Type =
                    FilterType.MultipleChoice,

                Options = EnumHelpers.ToSearchFilters<PageTypeFilter>(cultureName)
                    
            };
        }

        protected void ResolveExternalUrls(
            List<GlobalResult> resultList)
        {
            resultList?
                .ForEach(result =>
                    {
                        result.ExternalUrl = URLHelper.GetAbsoluteUrl(result.ExternalUrl);
                        result.PageUrl = URLHelper.GetAbsoluteUrl(result.PageUrl);
                    }
                );


        }
    }
}
