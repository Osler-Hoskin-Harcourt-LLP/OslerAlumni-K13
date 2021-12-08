using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Mvc.Navigation.Kentico.Models;
using ECA.Mvc.Navigation.Kentico.Providers;
using ECA.PageURL.Definitions;

namespace ECA.Mvc.Navigation.Repositories
{
    public class NavigationItemRepository
        : INavigationItemRepository
    {
        #region "Private fields"

        private readonly ICacheService _cacheService;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly ContextConfig _context;

        #endregion

        public NavigationItemRepository(
            ICacheService cacheService,
            ISettingsKeyRepository settingsKeyRepository,
            ContextConfig context)
        {
            _cacheService = cacheService;
            _settingsKeyRepository = settingsKeyRepository;
            _context = context;
        }

        #region "Methods"

        public IEnumerable<PageType_NavigationItem> GetNavigationItems(
            string path,
            bool includeProtected = false,
            string cultureName = null,
            string siteName = null,
            bool showProfilesAndPreferences = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            cultureName =
                cultureName.ReplaceIfEmpty(_context.CultureName);
            siteName =
                siteName.ReplaceIfEmpty(_context.Site?.SiteName);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    ECAGlobalConstants.Caching.Navigation.NavigationItemsByPath,
                    path,
                    includeProtected,
                    showProfilesAndPreferences),
                IsCultureSpecific = true,
                CultureCode = cultureName,
                IsSiteSpecific = true,
                SiteName = siteName,
                // Bust the cache whenever the nav container folder
                // or one of the nav item pages in that folder is modified
                CacheDependencies = new List<string>
                {
                    $"node|{siteName}|{path}",
                    $"node|{siteName}|{path}|childnodes"
                }
            };

            var result = _cacheService.Get(
                () =>
                {
                    path =
                        $"{path}{(path.EndsWith("/") ? string.Empty : "/")}%";

                    var query = PageType_NavigationItemProvider
                        .GetPageType_NavigationItems()
                        .OnSite(siteName)
                        .Culture(cultureName)
                        .CombineWithDefaultCulture(false)
                        .Path(path)
                        .LatestVersion(_context.IsPreviewMode)
                        .Published(!_context.IsPreviewMode);

                    if (!includeProtected)
                    {
                        query = query.WhereEquals(
                            nameof(PageType_NavigationItem.IsProtected), false);
                    }

                    if (!showProfilesAndPreferences)
                    {
                        var profileAndPreferences = _settingsKeyRepository.GetValue<Guid>(
                            StandalonePageType.ProfileAndPreferences.ToStringRepresentation());

                        var notProfileAndPreferencesPage = new WhereCondition()
                            .WhereNull(nameof(PageType_NavigationItem.PageGUID))
                            .Or()
                            .WhereNotEquals(nameof(PageType_NavigationItem.PageGUID),
                                profileAndPreferences);

                        query = query.And(notProfileAndPreferencesPage);
                    }

                    return query
                        .OrderBy(nameof(TreeNode.NodeOrder))
                        .ToList();
                },
                cacheParameters);

            return result;
        }

        #endregion
    }
}
