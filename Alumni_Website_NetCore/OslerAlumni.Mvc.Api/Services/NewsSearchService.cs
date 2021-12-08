using System.Collections.Generic;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using OslerAlumni.Mvc.Api.Models;

namespace OslerAlumni.Mvc.Api.Services
{
    public class NewsSearchService
        : BaseSearchService<NewsSearchRequest, News>
    {
        public NewsSearchService(
            ICacheService cacheService,
            ISearchService searchService)
            : base(
                cacheService,
                searchService)
        { }

        #region "Helper methods"

        protected override SearchResponse<News> GetSearchResults(
            NewsSearchRequest searchRequest,
            CacheParameters cacheParameters = null)
        {
            var searchResponse = base.GetSearchResults(
                searchRequest,
                cacheParameters);

            if (searchResponse == null)
            {
                return null;
            }

            ResolveImageUrls(searchResponse.Items);

            return searchResponse;
        }

        protected void ResolveImageUrls(
            List<News> newsList)
        {
            newsList?
                .ForEach(news =>
                    news.ImageUrl = URLHelper.ResolveUrl(news.ImageUrl));
        }

        #endregion
    }
}
