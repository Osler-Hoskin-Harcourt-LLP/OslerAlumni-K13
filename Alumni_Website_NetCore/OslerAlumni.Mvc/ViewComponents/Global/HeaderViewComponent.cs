using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Models;
using ECA.Mvc.Navigation.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Global
{
    public class HeaderViewComponent : BaseViewComponent
    {
        private readonly IUserRepository _userRepository;
        private readonly IGlobalAssetService _globalAssetService;
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        public HeaderViewComponent(
            IUserRepository userRepository,
            IGlobalAssetService globalAssetService,
            INavigationService navigationService,
            IPageService pageService,
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever) : base(pageUrlService, context, dataRetriever)
        {
            _userRepository = userRepository;
            _globalAssetService = globalAssetService;
            _navigationService = navigationService;
            _pageService = pageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var page = _dataRetriever.Retrieve<TreeNode>().Page;
            var currentUser = _userRepository.CurrentUser;

            var isAuthenticated = currentUser != null;

            string searchPageUrl;

            _pageUrlService.TryGetPageMainUrl(StandalonePageType.Search, _context.CultureName, out searchPageUrl);

            var header = new HeaderViewModel
            {
                LanguageToggle =
                    GetLanguageToggle(page),

                // NOTE: We don't know what type of a page the nav items are linked to, so this will use multi-doc query
                PrimaryNavigationItems =
                    _navigationService.GetNavigation(
                        NavigationType.Primary,
                        isAuthenticated),

                // NOTE: We don't know what type of a page the nav items are linked to, so this will use multi-doc query
                SecondaryNavigationItems =
                    _navigationService.GetNavigation(
                        NavigationType.Secondary,
                        isAuthenticated),

                DesktopLogoImageUrl = _globalAssetService.GetWebsiteLogoUrl(),

                MobileLogoImageUrl = _globalAssetService.GetWebsiteMobileLogoUrl(),

                GlobalSearchPageUrl = searchPageUrl,

                IsAuthenticatedUser = isAuthenticated,

                UserFirstName = currentUser?.FirstName,

                IsLoggedInViaOslerNetwork = _userRepository.IsSystemUser(currentUser?.UserName)
            };

            SetSelectedNavItems(header?.PrimaryNavigationItems, page?.NodeAliasPath);

            SetSelectedNavItems(header?.SecondaryNavigationItems, page?.NodeAliasPath);

            return View("_Header", header);
        }

        protected LanguageToggleViewModel GetLanguageToggle(TreeNode page)
        {
            if (page == null)
            {
                return null;
            }

            Dictionary<string, string> cultureUrls;

            if (!_pageService.TryGetPageCultureUrls(
                    page,
                    true,
                    false,
                    out cultureUrls,
                    // The culture version of the page that is pulled as part of this call is cached
                    // and can be re-used when the user switches cultures or someone else requests
                    // the other culture version of the page
                    includeAllCoupledColumns: true)
                || (cultureUrls.Count < 1))
            {
                return null;
            }

            var url = cultureUrls.First().Value;

            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            return new LanguageToggleViewModel
            {
                ShowToggle = true,
                LinkUrl = $"{url}{Request.Query}"
            };
        }

        private void SetSelectedNavItems(IEnumerable<NavigationItem> navigationItems, string currentPageNodeAliasPath)
        {
            if (navigationItems != null && !string.IsNullOrWhiteSpace(currentPageNodeAliasPath))
            {
                foreach (var navigationItem in navigationItems)
                {
                    if (!string.IsNullOrWhiteSpace(navigationItem?.NodeAliasPath))
                    {
                        navigationItem.IsSelected = currentPageNodeAliasPath.StartsWith(navigationItem.NodeAliasPath);
                    }
                }
            }
        }
    }
}
