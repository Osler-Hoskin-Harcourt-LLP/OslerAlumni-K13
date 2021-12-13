using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Repositories;

namespace ECA.PageURL.Services
{
    public class PageUrlService
        : ServiceBase, IPageUrlService
    {
        #region "Private fields"

        private readonly object _lockUrlCreate = new object();

        private readonly IKenticoClassRepository _classRepository;
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IPageUrlItemRepository _pageUrlItemRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly ICacheService _cacheService;

        private readonly ContextConfig _context;

        #endregion

        public PageUrlService(
            IKenticoClassRepository classRepository,
            IEventLogRepository eventLogRepository,
            IPageUrlItemRepository pageUrlItemRepository,
            ISettingsKeyRepository settingsKeyRepository,
            ICacheService cacheService,
            ContextConfig context)
        {
            _classRepository = classRepository;
            _eventLogRepository = eventLogRepository;
            _pageUrlItemRepository = pageUrlItemRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _cacheService = cacheService;

            _context = context;
        }

        #region "Methods"

        public string ConvertToUrl(
            CustomTable_PageURLItem urlItem)
        {
            if (urlItem == null)
            {
                return null;
            }

            string culturePrefix;

            if (!_context.AllowedCultureCodes
                    .TryGetKeyByOrdinalValue(urlItem.Culture, out culturePrefix))
            {
                culturePrefix = urlItem.Culture;
            }

            return $"/{culturePrefix}{urlItem.URLPath}";
        }

        public bool DeleteUrls(
            TreeNode page)
        {
            if (page == null)
            {
                return true;
            }

            return _pageUrlItemRepository.Delete(
                page.NodeGUID,
                page.DocumentCulture,
                page.NodeSiteID);
        }

        /// <summary>
        /// Generates a valid URL path for the provided page (based on its DocumentNamePath)
        /// and recursively checks if the URL path is in use by any other page in the same culture.
        /// Returns the URL path with the next available increment appended.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="urlItem">
        /// Output parameter that will contain the existing URL path record,
        /// if the URL path is in use by the specified page.
        /// </param>
        /// <returns></returns>
        public string GetAvailableUrlPath(
            TreeNode page,
            out CustomTable_PageURLItem urlItem)
        {
            urlItem = null;

            if (page == null)
            {
                return null;
            }

            var urlPath = GetUrlPath(
                page);

            return GetNextAvailableUrlPath(
                page,
                urlPath,
                out urlItem);
        }

        /// <summary>
        /// Gets the available URL path.
        /// </summary>
        /// <param name="urlItem">The URL item.</param>
        /// <param name="siteName">Site name.</param>
        /// <returns></returns>
        public string GetAvailableUrlPath(
            CustomTable_PageURLItem urlItem,
            string siteName)
        {
            if (urlItem == null)
            {
                return null;
            }

            var urlPath = GetUrlPath(
                urlItem.URLPath,
                siteName);

            return GetNextAvailableUrlPath(
                urlItem.NodeGUID,
                urlItem.Culture,
                siteName,
                urlPath);
        }

        /// <summary>
        /// Returns the provided URL with the query string
        /// populated from the provided object properties.
        /// </summary>
        /// <param name="url">Original URL.</param>
        /// <param name="queryStrObj">
        /// Anonymous object, whose property values should be turned into a query string.
        /// </param>
        /// <returns></returns>
        public string GetUrl(
            string url,
            object queryStrObj)
        {
            if ((queryStrObj == null)
                || string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            var queryStrValues = queryStrObj
                .AsDictionary();

            if (queryStrValues == null)
            {
                return url;
            }

            var queryStr = string.Join(
                "&",
                queryStrValues
                    .Select(kvp =>
                        $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value?.ToString())}"));

            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                url = $"{url}?{queryStr}";
            }

            return url;
        }



        /// <summary>
        /// Generates a valid URL path for the provided page based on its DocumentNamePath.
        /// Removes forbidden URL characters.
        /// Does NOT include culture prefix.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetUrlPath(
            TreeNode page)
        {
            if (page == null)
            {
                return null;
            }

            return "";
            // TODO##
            //return GetUrlPath(
            //    page.DocumentNamePath,
            //    page.NodeSiteName);
        }

        public string GetUrlPath(
            string namePath,
            string siteName)
        {
            string url = null;

            try
            {
                // TODO##
                //url =
                //    TreePathUtils.GetSafeUrlPath(
                //            namePath,
                //            siteName)
                //        ?.ToLowerCSafe();

                var maxLength = _settingsKeyRepository
                    .GetValue<int>(
                        ECAGlobalConstants.Settings.UrlMaxLength,
                        siteName);

                if ((maxLength > 0) && ((url?.Length ?? 0) > maxLength))
                {
                    url = url.Substring(0, maxLength);
                }
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetUrlPath),
                    ex);
            }

            return url;
        }

        /// <summary>
        /// Checks if the page allows for being navigated to directly
        /// and if it should have a URL as a result.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool IsNavigable(
            TreeNode page)
        {
            if (page == null)
            {
                return false;
            }

            // TODO##
            //if (page.DocumentMenuItemHideInNavigation)
            //{
            //    return false;
            //}

            if (string.Equals(
                    page.NodeClassName,
                    _context.BasePageType,
                    StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return _classRepository.IsChildClass(
                page.NodeClassName,
                _context.BasePageType,
                KenticoClassType.PageType);
        }

        public bool IsRootUrl(
            string url)
        {
            return string.IsNullOrEmpty(url) || "/".Equals(url);
        }

        public string ResolveToAbsolute(
            string url,
            SiteInfo site)
        {
            var absoluteUrl = URLHelper.GetAbsoluteUrl(url);

            if ((site == null) || URLHelper.ContainsProtocol(absoluteUrl))
            {
                return absoluteUrl;
            }

            return URLHelper.CombinePath(
                url,
                '/',
                site.SitePresentationURL,
                null);
        }

        /// <inheritdoc cref="TryGetPageMainUrl(Guid, string, out string, string)"/>
        public bool TryGetPageMainUrl(
            TreeNode page,
            out string url)
        {
            url = null;

            if (page == null)
            {
                return false;
            }

            return TryGetPageMainUrl(
                page.NodeGUID,
                page.DocumentCulture,
                out url,
                page.NodeSiteName);
        }

        /// <summary>
        /// Attempts to get the main page URL
        /// and apply culture-based localization to it.
        /// </summary>
        /// <param name="nodeGuid"></param>
        /// <param name="cultureName"></param>
        /// <param name="siteName"></param>
        /// <param name="url"></param>
        /// <returns>
        /// Main URL of the page, including the culture prefix,
        /// if applicable.
        /// </returns>
        public bool TryGetPageMainUrl(
            Guid nodeGuid,
            string cultureName,
            out string url,
            string siteName = null)
        {
            url = null;

            if (nodeGuid == Guid.Empty)
            {
                return false;
            }

            siteName = siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName);

            var cacheParameters = new CacheParameters
            {
                // Explicitly allowing for null values to be cached, so that we don't keep querying the DB
                // if a page doesn't have a main URL in a specific culture
                AllowNullValue = true,
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.PageUrls.MainUrlItemByNodeGuid,
                    nodeGuid),
                IsCultureSpecific = true,
                CultureCode = cultureName,
                IsSiteSpecific = true,
                SiteName = siteName,
                // Bust the cache whenever the page or any of its associated URL items is modified
                // (ECA.Admin.PageURL.Modules.UrlItemChangeModule makes sure this key is touched
                // when any of the page's URL items is modified)
                CacheDependencies = new List<string>
                {
                    $"nodeguid|{siteName}|{nodeGuid}"
                }
            };

             url = _cacheService.Get(
                () =>
                    _pageUrlItemRepository.GetPageUrl(
                        nodeGuid,
                        cultureName,
                        siteName),
                cacheParameters);

            if (string.IsNullOrEmpty(url))
            {
                return false;
            }


            return true;
        }

        public bool TryGetPageMainUrl(
            StandalonePageType standalonePageType,
            string cultureName,
            out string url,
            string siteName = null)
        {
            var nodeGuid = _settingsKeyRepository.GetValue<Guid>(
                standalonePageType.ToStringRepresentation());

            return TryGetPageMainUrl(
                nodeGuid,
                cultureName,
                out url,
                siteName);
        }






        #endregion

        #region "Helper methods"

        /// <summary>
        /// Returns a dummy instance of <see cref="CustomTable_PageURLItem"/>,
        /// configured to redirect from the provided urlPath to the site's home page.
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="cultureName"></param>
        /// <param name="cacheParameters"></param>
        /// <returns></returns>
        protected CustomTable_PageURLItem GetHomeRedirectUrlItem(
            string urlPath,
            string cultureName,
            CacheParameters cacheParameters = null)
        {
            var homeSettingName =
                StandalonePageType.Home.ToStringRepresentation();

            var urlItem = new CustomTable_PageURLItem
            {
                NodeGUID = _settingsKeyRepository.GetValue<Guid>(
                    homeSettingName),
                Culture = cultureName,
                URLPath = urlPath,
                IsMainURL = false,
                IsCustomURL = false,
                //RedirectType = AliasActionModeEnum.RedirectToMainURL.ToString() TODO##
            };

            // Bust the cache whenever the home page setting changes
            cacheParameters?.CacheDependencies
                .Add(
                    $"{SettingsKeyInfo.OBJECT_TYPE}|byname|{homeSettingName}");

            return urlItem;
        }

        /// <summary>
        /// Checks if the provided URL path is in use by any other page of the same culture as the specified page.
        /// If so, appends an increment to the URL path and repeats the check recursively,
        /// until it finds the increment that is not in use.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="baseUrlPath"></param>
        /// <param name="urlItem">
        /// Output parameter that will contain the existing URL path record,
        /// if the URL path is in use by the specified page.
        /// </param>
        /// <returns></returns>
        protected string GetNextAvailableUrlPath(
            TreeNode page,
            string baseUrlPath,
            out CustomTable_PageURLItem urlItem)
        {
            urlItem = null;

            if (page == null)
            {
                return null;
            }

            var maxLength = _settingsKeyRepository
                .GetValue<int>(
                    ECAGlobalConstants.Settings.UrlMaxLength,
                    page.NodeSiteName);

            var urlPath = baseUrlPath;
            var index = 0;

            while (!IsUrlPathAvailable(page, urlPath, out urlItem))
            {
                urlPath = baseUrlPath;
                index++;

                var suffix = $"-{index}";

                // Make sure that the combined length of the URL path
                // and the new suffix will not actually exceed the max length.
                // If it does, trim the end of the URL path, not the suffix.
                if ((maxLength > 0)
                    && (urlPath.Length + suffix.Length > maxLength))
                {
                    urlPath = urlPath.Substring(0, maxLength - suffix.Length);
                }

                urlPath = $"{urlPath}{suffix}";
            }

            return urlPath;
        }

        /// <summary>
        /// Checks if the provided URL path is in use by any other page of the same culture as the specified page.
        /// If so, appends an increment to the URL path and repeats the check recursively,
        /// until it finds the increment that is not in use.
        /// </summary>
        /// <param name="nodeGuid">The node unique identifier.</param>
        /// <param name="cultureName">The culture.</param>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="baseUrlPath">The base URL path.</param>
        /// <returns></returns>
        protected string GetNextAvailableUrlPath(
            Guid nodeGuid,
            string cultureName,
            string siteName,
            string baseUrlPath)
        {
            if (nodeGuid == Guid.Empty)
            {
                return null;
            }

            var maxLength = _settingsKeyRepository
                .GetValue<int>(
                    ECAGlobalConstants.Settings.UrlMaxLength,
                    siteName);

            var urlPath = baseUrlPath;
            var index = 0;

            while (!IsUrlPathAvailable(nodeGuid, cultureName, siteName, urlPath))
            {
                urlPath = baseUrlPath;
                index++;

                var suffix = $"-{index}";

                // Make sure that the combined length of the URL path
                // and the new suffix will not actually exceed the max length.
                // If it does, trim the end of the URL path, not the suffix.
                if ((maxLength > 0)
                    && (urlPath.Length + suffix.Length > maxLength))
                {
                    urlPath = urlPath.Substring(0, maxLength - suffix.Length);
                }

                urlPath = $"{urlPath}{suffix}";
            }

            return urlPath;
        }

        /// <summary>
        /// Checks if the provided URL path is in use by any other page of the same culture as the specified page.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="urlPath"></param>
        /// <param name="urlItem">
        /// Output parameter that will contain the existing URL path record,
        /// if the URL path is in use by the specified page.
        /// </param>
        /// <returns>
        /// True, if the URL path is in use by the specified page or if it is not in use at all (in the current culture).
        /// False, otherwise.
        /// </returns>
        protected bool IsUrlPathAvailable(
            TreeNode page,
            string urlPath,
            out CustomTable_PageURLItem urlItem)
        {
            urlItem = null;

            if ((page == null)
                || (page.NodeGUID == Guid.Empty))
            {
                return false;
            }

            urlItem = _pageUrlItemRepository
                .GetPageUrlItemByUrlPath(
                    urlPath,
                    page.DocumentCulture,
                    page.NodeGUID,
                    page.NodeSiteName);

            return (urlItem == null)
                   || (urlItem.NodeGUID == page.NodeGUID);
        }

        /// <summary>
        /// Checks if the provided URL path is in use by any other page of the same culture as the specified page.
        /// </summary>
        /// <param name="nodeGuid">The node unique identifier.</param>
        /// <param name="cultureName">The culture.</param>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="urlPath">The URL path.</param>
        /// <returns>
        /// True, if the URL path is in use by the specified page or if it is not in use at all (in the current culture).
        /// False, otherwise.
        /// </returns>
        protected bool IsUrlPathAvailable(
            Guid nodeGuid,
            string cultureName,
            string siteName,
            string urlPath)
        {
            if (nodeGuid == Guid.Empty)
            {
                return false;
            }

            var urlItem = _pageUrlItemRepository
                .GetPageUrlItemByUrlPath(
                    urlPath,
                    cultureName,
                    nodeGuid,
                    siteName);

            return (urlItem == null);
        }

        #endregion
    }
}
