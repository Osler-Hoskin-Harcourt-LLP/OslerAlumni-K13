using System.Collections.Generic;
using System.Linq;
using CMS.Search;
using CMS.Search.Azure;
using ECA.Caching.Models;
using ECA.Caching.Services;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Repositories
{
    public class KenticoSearchIndexRepository
        : IKenticoSearchIndexRepository
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;

        #endregion

        public KenticoSearchIndexRepository(
            ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        #region "Methods"
        
        public SearchIndexInfo GetByName(
            string name)
        {
            // Converts the Kentico index code name to a valid Azure Search index name (if necessary)
            name = NamingHelper.GetValidIndexName(name);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.Search.SearchIndexByName,
                    name),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever the search index is modified
                CacheDependencies = new List<string>
                {
                    $"{SearchIndexInfo.OBJECT_TYPE}|byname|{name}"
                }
            };

            var result = _cacheService.Get(
                () => 
                    SearchIndexInfoProvider.GetSearchIndexInfo(name),
                cacheParameters);

            return result;
        }

        public IList<SearchIndexInfo> GetAzureSearchIndexes()
        {
            return GetSearchIndexesByType(
                SearchIndexInfo.AZURE_SEARCH_PROVIDER);
        }

        #endregion

        #region "Helper methods"

        protected IList<SearchIndexInfo> GetSearchIndexesByType(
            string indexType)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.Search.SearchIndexesByType,
                    indexType),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the search indexes is modified
                CacheDependencies = new List<string>
                {
                    $"{SearchIndexInfo.OBJECT_TYPE}|all"
                }
            };

            var result = _cacheService.Get(
                () =>
                    SearchIndexInfoProvider.GetSearchIndexes()
                        .WhereEquals(
                            nameof(SearchIndexInfo.IndexProvider),
                            indexType)
                        .ToList(),
                cacheParameters);

            return result;
        }

        #endregion
    }
}
