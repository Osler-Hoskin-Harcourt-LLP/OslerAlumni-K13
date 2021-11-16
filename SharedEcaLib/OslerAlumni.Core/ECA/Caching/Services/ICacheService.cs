using System;
using CMS.DocumentEngine;
using ECA.Caching.Models;
using ECA.Core.Services;

namespace ECA.Caching.Services
{
    public interface ICacheService
        : IService
    {
        T Get<T>(
            Func<CacheParameters, T> func,
            CacheParameters parameters);

        T Get<T>(
            Func<CacheParameters, T> func,
            string cacheKey,
            bool isCultureSpecific = true,
            bool isSiteSpecific = true,
            int? duration = null,
            params string[] cacheDependencies);

        T Get<T>(
            Func<T> func,
            CacheParameters settings);

        T Get<T>(
            Func<T> func,
            string cacheKey,
            bool isCultureSpecific = true,
            bool isSiteSpecific = true,
            int? duration = null,
            params string[] cacheDependencies);

        string GetCacheKey(
            string baseKey,
            string cultureName = null,
            string siteName = null);
        
        /// <summary>
        /// Touches the list of cache keys that are passed in,
        /// so that dependant cached items are busted.
        /// </summary>
        /// <param name="cacheKeys">
        /// List of cache keys to "touch".
        /// </param>
        void TouchCacheKeys(
            params string[] cacheKeys);

        void TouchCacheKeys(
            TreeNode page);

        bool TryGet<T>(
            string cacheKey,
            out T obj);

        bool TrySet<T>(
            T obj,
            CacheParameters parameters);
    }
}
