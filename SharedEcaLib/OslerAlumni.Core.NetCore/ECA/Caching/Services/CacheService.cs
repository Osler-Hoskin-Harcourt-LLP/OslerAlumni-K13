using System;
using System.Linq;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Core.Definitions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;

namespace ECA.Caching.Services
{
    public class CacheService
        : ServiceBase, ICacheService
    {
        #region "Private fields"

        private readonly AppCacheConfig _cacheConfig;
        private readonly ContextConfig _context;

        #endregion

        public CacheService(
            ISettingsKeyRepository settingsKeyRepository,
            ContextConfig context)
            : this(
                new AppCacheConfig
                {
                    Enabled = settingsKeyRepository.GetValue<bool>(
                        ECAGlobalConstants.Settings.Caching.CacheEnabled),
                    CacheExpiryMinutes = settingsKeyRepository.GetValue<int>(
                        ECAGlobalConstants.Settings.Caching.CacheExpiryMinutes)
                },
                context)
        { }

        public CacheService(
            AppCacheConfig config,
            ContextConfig context)
        {
            _cacheConfig = config;
            _context = context;
        }

        #region "Methods"

        public T Get<T>(
            Func<T> func,
            CacheParameters settings)
        {
            return Get(
                cp => func(),
                settings);
        }

        public T Get<T>(
            Func<CacheParameters, T> func,
            CacheParameters parameters)
        {
            CacheSettings cacheSettings;

            if (!TryGetCacheSettings(parameters, out cacheSettings))
            {
                return func(parameters);
            }

            return Get(func, parameters, cacheSettings);
        }

        public T Get<T>(
            Func<T> func,
            string cacheKey,
            bool isCultureSpecific = true,
            bool isSiteSpecific = true,
            int? duration = null,
            params string[] cacheDependencies)
        {
            return Get(
                cp => func(),
                cacheKey,
                isCultureSpecific,
                isSiteSpecific,
                duration,
                cacheDependencies);
        }

        public T Get<T>(
            Func<CacheParameters, T> func,
            string cacheKey,
            bool isCultureSpecific = true,
            bool isSiteSpecific = true,
            int? duration = null,
            params string[] cacheDependencies)
        {
            return Get(
                func,
                new CacheParameters
                {
                    CacheKey = cacheKey,
                    IsCultureSpecific = isCultureSpecific,
                    IsSiteSpecific = isSiteSpecific,
                    Duration = duration,
                    CacheDependencies = cacheDependencies.ToList()
                });
        }

        public string GetCacheKey(
            string baseKey,
            string cultureName = null,
            string siteName = null)
        {
            return CacheHelper.BuildCacheItemName(
                new[]
                {
                    baseKey,
                    cultureName,
                    siteName
                }
                    .Where(p => !string.IsNullOrWhiteSpace(p)));
        }

        /// <inheritdoc />
        public void TouchCacheKeys(
            params string[] cacheKeys)
        {
            CacheHelper.TouchKeys(
                cacheKeys,
                logTasks: true,
                ensureKeys: false);
        }

        public void TouchCacheKeys(
            TreeNode page)
        {
            // This will touch all of the page-related cache keys
            // (see https://docs.kentico.com/k11/configuring-kentico/optimizing-website-performance/configuring-caching/setting-cache-dependencies
            // for the full reference)
            page?.ClearCache();
        }

        public bool TryGet<T>(
            string cacheKey,
            out T obj)
        {
            return CacheHelper.TryGetItem(
                cacheKey,
                out obj);
        }

        public bool TrySet<T>(
            T obj,
            CacheParameters parameters)
        {
            CacheSettings cacheSettings;

            if (!TryGetCacheSettings(parameters, out cacheSettings))
            {
                return false;
            }

            // Forcefully remove the cached item, so that it is replaced by the new value
            // (N.B. Have to do it this way, because we are using dynamic caching by passing a function instead of setting a value,
            // and Kentico is going to wait for the cache expiration otherwise)
            CacheHelper.Remove(cacheSettings.CacheItemName);

            Get(cp => obj, parameters, cacheSettings);

            return true;
        }

        #endregion

        #region "Helper methods"

        protected T Get<T>(
            Func<CacheParameters, T> func,
            CacheParameters parameters,
            CacheSettings cacheSettings)
        {
            var cacheKey = cacheSettings.CacheItemName;

            object preliminaryResult;

            // NOTE: If a cache is declared as a cache dependency, but doesn't yet have a cached value,
            // Kentico automatically sets its cached value to an instance of DummyItem. This means that 
            // if we wanted to dynamically cache a value against that key later, our dynamic function
            // would not actually be evaluated, since Kentico would consider the dummy object as
            // an existing cached value. That is why we are explicitly removing the dummy object here,
            // so that it does not interfere with out caching mechanism.
            if (TryGet(cacheKey, out preliminaryResult)
                && (preliminaryResult is DummyItem))
            {
                CacheHelper.Remove(cacheKey);
            }

            // If we are in preview mode do not cache!
            if (_context.IsPreviewMode)
            {
                return func(parameters);
            }

            var cachedResult = CacheHelper.Cache(
                cs =>
                {
                    var result = func(parameters);

                    var dependencies = parameters.CacheDependencies;

                    // Update cache settings with the dependencies (both initial and dynamically obtained)
                    if ((dependencies != null) && (dependencies.Count > 0))
                    {
                        // Remove duplicates
                        dependencies = dependencies.Distinct().ToList();

                        cacheSettings.CacheDependency =
                            CacheHelper.GetCacheDependency(dependencies);
                    }

                    // Return the actual data to be cached
                    return result;
                },
                cacheSettings);

            // If null value is not allowed to be cached, dump the cache
            if (!parameters.AllowNullValue && (cachedResult == null))
            {
                CacheHelper.Remove(cacheKey);
            }

            return cachedResult;
        }

        protected bool TryGetCacheSettings(
            CacheParameters parameters,
            out CacheSettings cacheSettings)
        {
            cacheSettings = null;

            if (!_cacheConfig.Enabled)
            {
                return false;
            }

            if (parameters == null)
            {
                return false;
            }

            // If the site name is not provided, use current site name
            if (string.IsNullOrEmpty(parameters.SiteName))
            {
                parameters.SiteName = _context.Site?.SiteName;
            }

            // If the culture code is not provided, use current culture code
            if (string.IsNullOrEmpty(parameters.CultureCode))
            {
                parameters.CultureCode = _context.CultureName;
            }

            // If a specific value is provided, it has priority
            var minutes = parameters.Duration ?? _cacheConfig.CacheExpiryMinutes;

            var cacheNameParts = new[]
            {
                parameters.CacheKey,
                parameters.IsCultureSpecific
                    ? parameters.CultureCode
                    : null,
                parameters.IsSiteSpecific
                    ? parameters.SiteName
                    : null
            };

            cacheSettings =
                new CacheSettings(
                    minutes,
                    parameters.IsSlidingExpiration,
                    cacheNameParts
                        .Where(p => !string.IsNullOrEmpty(p))
                        .ToArray<object>());

            return true;
        }

        #endregion
    }
}
