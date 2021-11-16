using System.Linq;
using System.Collections.Generic;
using CMS.Helpers;
using CMS.Localization;
using ECA.Caching.Models;
using ECA.Caching.Services;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Repositories
{
    public class KenticoResourceStringRepository
        : IKenticoResourceStringRepository
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;

        #endregion

        public KenticoResourceStringRepository(
            ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        #region "Methods"
        
        public ResourceStringInfo GetById(
            int id)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.ResourceStrings.ResourceStringById,
                    id),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever the resource string is modified
                CacheDependencies = new List<string>
                {
                    $"{ResourceStringInfo.OBJECT_TYPE}|byid|{id}"
                }
            };

            var result = _cacheService.Get(
                () =>
                    ResourceStringInfoProvider.GetResourceStringInfo(id),
                cacheParameters);

            return result;
        }

        public ResourceStringInfo GetByName(
            string name)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.ResourceStrings.ResourceStringByName,
                    name),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever the resource string is modified
                CacheDependencies = new List<string>
                {
                    $"{ResourceStringInfo.OBJECT_TYPE}|byname|{name}"
                }
            };

            var result = _cacheService.Get(
                () =>
                    ResourceStringInfoProvider.GetResourceStringInfo(name),
                cacheParameters);

            return result;
        }

        public IEnumerable<ResourceStringInfo> GetByPrefix(
            string prefix)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.ResourceStrings.ResourceStringByPrefix,
                    prefix),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever the resource string is modified
                CacheDependencies = new List<string>
                {
                    
                    // Note since we can't bust cache based on resource string prefix, we will just bust on any resource string changes
                    $"{ResourceStringInfo.OBJECT_TYPE}|all"
                }
            };

            var result = _cacheService.Get(
                () =>
                    ResourceStringInfoProvider.GetResourceStrings()
                    .WhereStartsWith(nameof(ResourceStringInfo.StringKey), prefix),
                cacheParameters);

            return result;
        }

        public IEnumerable<ResourceStringInfo> GetByIds(
            List<int> resourceStringIds)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.ResourceStrings.ResourceStringByIds,
                    resourceStringIds.Select(i => i.ToString()).Join("|")),
                IsCultureSpecific = false,
                IsSiteSpecific = false,
                // Bust the cache whenever any of the resource strings are modified
                CacheDependencies = resourceStringIds.Select(id => $"{ResourceStringInfo.OBJECT_TYPE}|byid|{id}").ToList()
            };

            var result = _cacheService.Get(
                () =>
                    ResourceStringInfoProvider.GetResourceStrings()
                        .WhereIn(nameof(ResourceStringInfo.StringID), resourceStringIds),
                        cacheParameters);

            return result;
        }

        #endregion
    }
}
