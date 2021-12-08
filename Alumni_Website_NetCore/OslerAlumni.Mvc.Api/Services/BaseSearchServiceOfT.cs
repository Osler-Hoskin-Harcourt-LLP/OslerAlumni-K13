using System.Collections.Generic;
using System.Linq;
using CMS.Search;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Api.Models;

namespace OslerAlumni.Mvc.Api.Services
{
    public abstract class BaseSearchService<TRequest, TResult>
        : ServiceBase, ISearchService<TRequest, TResult>
        where TRequest : SearchRequest<TResult>, new()
        where TResult : class, ISearchable, new()
    {
        #region "Private fields"

        protected readonly ICacheService _cacheService;
        protected readonly ISearchService _searchService;

        #endregion

        protected BaseSearchService(
            ICacheService cacheService,
            ISearchService searchService)
        {
            _cacheService = cacheService;
            _searchService = searchService;
        }

        #region "Methods"

        public virtual SearchResponse<TResult> Search(
            TRequest searchRequest)
        {
            if (searchRequest == null)
            {
                return null;
            }
            // There is no need to cache in the preview mode
            if (searchRequest.IsPreviewMode
                // We can't cache every single variation of keyword-based search
                // and/or filtered search. There is no value in caching every single page
                // of search results, beyond the first page
                || (searchRequest.PageNumber > 1)
                || searchRequest.IsKeywordOrFilteredSearch())
            {
                return GetSearchResults(searchRequest);
            }

            // Use caching only for the first page of results,
            // when keyword search and filters are not in use
            var cacheParameters = new CacheParameters
            {
                CacheKey =
                    GetSearchResultsCacheKey(searchRequest),
                IsCultureSpecific = true,
                CultureCode = searchRequest.Culture,
                IsSiteSpecific = false,
                // Bust the cache whenever the associated search index is modified
                CacheDependencies = new List<string>
                {
                    string.Format(
                        GlobalConstants.Caching.Search.SearchIndexByName,
                        searchRequest.IndexName)
                }
            };

            var result = _cacheService.Get(
                cp =>
                    GetSearchResults(searchRequest, cp),
                cacheParameters);

            return result;
        }

        #endregion

        #region "Helper methods"

        protected virtual SearchResponse<TResult> GetSearchResults(
            TRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            SearchIndexInfo searchIndex;

            var searchResponse = _searchService.Search(
                searchRequest,
                out searchIndex);

            if (cacheParameters != null)
            {
                // Bust the search results cache whenever a page of the page type,
                // that is included in the search is added/updated/deleted.
                // We are looping through the sites that are included in the search index,
                // since search API is theoretically site context agnostic,
                // and could be serving one or more sites.
                foreach (SiteInfo site in searchIndex.AssignedSites)
                {
                    cacheParameters.CacheDependencies.AddRange(
                        searchRequest.PageTypes
                            .Select(pageType => $"nodes|{site.SiteName}|{pageType}|all")
                            .ToList());
                }
            }

            return searchResponse;
        }

        protected virtual string GetSearchResultsCacheKey(
            TRequest searchRequest)
        {
            return string.Format(
                GlobalConstants.Caching.Search.SearchResultsBySearchRequest,
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
                searchRequest.FilterForCompetitor);
        }

        #endregion
    }
}
