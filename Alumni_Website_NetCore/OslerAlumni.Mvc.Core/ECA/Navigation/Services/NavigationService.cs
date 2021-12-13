using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Kentico.Models;
using ECA.Mvc.Navigation.Models;
using ECA.Mvc.Navigation.Repositories;
using ECA.PageURL.Services;
using OslerAlumni.Core.Repositories;

namespace ECA.Mvc.Navigation.Services
{
    public class NavigationService
        : ServiceBase, INavigationService
    {
        #region "Private fields"

        private readonly IDocumentRepository _documentRepository;
        private readonly IUserRepository _userRepository;
        private readonly INavigationItemRepository _navigationItemRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly ICacheService _cacheService;
        private readonly IPageUrlService _pageUrlService;
        private readonly ContextConfig _context;

        #endregion

        public NavigationService(
            IDocumentRepository documentRepository,
            IUserRepository userRepository,
            INavigationItemRepository navigationItemRepository,
            ISettingsKeyRepository settingsKeyRepository,
            ICacheService cacheService,
            IPageUrlService pageUrlService,
            ContextConfig context)
        {
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _navigationItemRepository = navigationItemRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _cacheService = cacheService;
            _pageUrlService = pageUrlService;

            _context = context;
        }

        #region "Methods"

        /// <inheritdoc />
        public IEnumerable<NavigationItem> GetNavigation(
            NavigationType navigationType,
            bool includeProtected = false,
            string pageTypeName = null)
        {
            var cultureName = LocalizationContext.CurrentCulture.CultureCode;
            var siteName = SiteContext.CurrentSiteName;

            //If LoggedInViaOlserNetwork remove ProfileAndPreferences link
            var loggedInViaOlserNetwork = _userRepository.IsSystemUser(_userRepository.CurrentUser?
                .UserName);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Navigation.NavigationByTypeAndPageType,
                    navigationType,
                    includeProtected,
                    pageTypeName.ReplaceIfEmpty(ECAGlobalConstants.Caching.All),
                    loggedInViaOlserNetwork),
                IsCultureSpecific = true,
                CultureCode = cultureName,
                IsSiteSpecific = true,
                SiteName = siteName
            };

            var result = _cacheService.Get(
                cp =>
                {
                    var pathSettingName = string.Empty;

                    switch (navigationType)
                    {
                        case NavigationType.Primary:
                            pathSettingName = ECAGlobalConstants.Settings.Navigation.PrimaryNavigationPath;
                            break;
                        case NavigationType.Secondary:
                            pathSettingName = ECAGlobalConstants.Settings.Navigation.SecondaryNavigationPath;
                            break;
                        case NavigationType.Footer:
                            pathSettingName = ECAGlobalConstants.Settings.Navigation.FooterNavigationPath;
                            break;
                    }

                    var path = _settingsKeyRepository.GetValue<string>(pathSettingName)
                        .ReplaceIfEmpty("/");

                    // Bust the cache on setting change
                    cp.CacheDependencies.Add(
                        $"{SettingsKeyInfo.OBJECT_TYPE}|byname|{pathSettingName}");

                    var navItems =
                        _navigationItemRepository.GetNavigationItems(
                            path,
                            includeProtected,
                            showProfilesAndPreferences: !loggedInViaOlserNetwork);

                    // Bust the cache on the change to any of the nav items
                    cp.CacheDependencies.Add(
                        _cacheService.GetCacheKey(
                            string.Format(
                                ECAGlobalConstants.Caching.Navigation.NavigationItemsByPath,
                                path,
                                includeProtected,
                                loggedInViaOlserNetwork),
                            cultureName,
                            siteName));

                    if (navItems == null)
                    {
                        return null;
                    }

                    var navItemsList = navItems.ToList();

                    cacheParameters.CacheDependencies
                        .AddRange(navItemsList
                            .Select(ni =>
                                $"nodeguid|{ni.NodeSiteName}|{ni.PageGUID}")
                            .ToList());

                    return ToNavigation(
                        navItemsList,
                        pageTypeName);
                },
                cacheParameters);

            return result;
        }

        #endregion

        #region "Helper methods"

        protected IEnumerable<NavigationItem> ToNavigation(
            IEnumerable<PageType_NavigationItem> navigationItems,
            string pageTypeName)
        {
            if (navigationItems == null)
            {
                return null;
            }

            var navigationItemList = navigationItems.ToList();

            if (navigationItemList.Count < 1)
            {
                return null;
            }

            // NOTE: If we don't know what type of a page the nav items are linked to, this will use multi-doc query
            // Pulling in the pages to ensure that we are only linking to published documents
            var linkedPages = _documentRepository
                .GetDocuments(
                    navigationItemList
                        .Where(ni => ni.PageGUID != Guid.Empty)
                        .Select(ni => ni.PageGUID),
                    pageTypeName)
                ?.ToList();

            var linkedPageUrls = linkedPages?
                .ToDictionary(
                    page => page.NodeGUID,
                    page =>
                    {
                        string url;

                        _pageUrlService.TryGetPageMainUrl(page, out url);

                        return new Tuple<string, string>(url, page.NodeAliasPath);
                    });

            return navigationItemList
                .Select(navItem =>
                {
                    Tuple<string, string> linkedPage;
                    string url = null;
                    string nodeAliasPath = null;

                    if ((linkedPageUrls != null) && (navItem.PageGUID != Guid.Empty))
                    {
                        if (!linkedPageUrls.TryGetValue(navItem.PageGUID, out linkedPage))
                        {
                            // If the nav item is linked to a page that no longer exists,
                            // or is not published yet,
                            // we should exclude that nav item
                            return null;
                        }
                        url = linkedPage?.Item1;

                        nodeAliasPath = linkedPage?.Item2;
                    }


                    bool? isExternal = null;

                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        isExternal = false;
                    }
                    else if (!string.IsNullOrWhiteSpace(navItem.ExternalURL))
                    {
                        isExternal = true;
                        url = navItem.ExternalURL;
                    }

                    return new NavigationItem
                    {
                        Title = navItem.Title,
                        NodeAliasPath = nodeAliasPath,
                        Url = URLHelper.ResolveUrl(url),
                        IsExternal = isExternal
                    };
                })
                .Where(navItem => navItem != null)
                .ToList();
        }

        #endregion
    }
}
