using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Models;
using ECA.Mvc.Navigation.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.Ajax.Utilities;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_Home.CLASS_NAME, typeof(HomeController))]

namespace OslerAlumni.Mvc.Controllers
{
    public class HomeController
        : BaseController
    {
        #region "Private fields"

        private readonly IAuthorizationService _authorizationService;
        private readonly IUserRepository _userRepository;
        private readonly IGlobalAssetService _globalAssetService;
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        private readonly IBreadCrumbService _breadCrumbService;
        private readonly IHomeService _homeService;


        #endregion

        public HomeController(
            IAuthorizationService authorizationService,
            IUserRepository userRepository,
            IGlobalAssetService globalAssetService,
            INavigationService navigationService,
            IPageUrlService pageUrlService,
            IPageService pageService,
            IBreadCrumbService breadCrumbService,
            IHomeService homeService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _authorizationService = authorizationService;
            _userRepository = userRepository;
            _globalAssetService = globalAssetService;
            _navigationService = navigationService;
            _pageService = pageService;
            _breadCrumbService = breadCrumbService;
            _homeService = homeService;
        }

        #region "Actions"

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_Home>().Page;
            var homePageViewModel = new HomePageViewModel(page)
            {
                IsAuthenticated = _userRepository.CurrentUser != null
            };

            homePageViewModel.HomeBannerLoggedOutViewModel.LoggedOutBannerLink = GetUrlLink(page.LoggedOutBannerLinkedPageGUID, page.DocumentCulture);

            if (homePageViewModel.IsAuthenticated)
            {
                homePageViewModel.HomeBannerLoggedInViewModel.BannerLink = GetUrlLink(page.BannerLinkedPageGUID, page.DocumentCulture);
                homePageViewModel.SpotlightLink = GetUrlLink(page.SpotlightLinkedPageGuid, page.DocumentCulture);
                homePageViewModel.AlumniMovesLink = GetUrlLink(page.AlumniMovesLinkedPageGuid, page.DocumentCulture);
                homePageViewModel.FeaturedCommunityNewsSectionViewModel = GetCommunityNewsSectionViewModel(page);
                homePageViewModel.FeaturedJobOpportunitiesViewModel = GetFeaturedJobOpportunitiesViewModel(page);
                homePageViewModel.FeaturedBoardOpportunitiesViewModel = GetFeaturedBoardOpportunitiesViewModel(page);
                homePageViewModel.FeaturedEventsViewModel = GetFeaturedEventsViewModel(page);
                homePageViewModel.FeaturedResourcesViewModel = GetFeaturedResourcesViewModel(page);
            }

            return View(homePageViewModel);
        }

        #endregion

        #region "Child actions"

        [ChildActionOnly]
        public ActionResult Header(
            TreeNode page)
        {
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

            return PartialView("_Header", header);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {

            var isAuthenticated = _userRepository.CurrentUser != null;

            var footer = new FooterViewModel
            {
                // NOTE: We don't know what type of a page the nav items are linked to, so this will use multi-doc query
                FooterNavigationItems =
                    _navigationService.GetNavigation(
                        NavigationType.Footer,
                        isAuthenticated),

                LogoImageUrl = _globalAssetService.GetWebsiteLogoUrl()
            };

            return PartialView("_Footer", footer);
        }

        [ChildActionOnly]
        public ActionResult BreadCrumbs(TreeNode page, bool? showBreadCrumbs = true)
        {
            BreadCrumbsViewModel model = null;

            if (showBreadCrumbs.HasValue && showBreadCrumbs.Value)
            {
                model = new BreadCrumbsViewModel();

                model.BreadCrumbs = _breadCrumbService.GetBreadCrumbs(page?.NodeAliasPath);
            }

            return PartialView("_BreadCrumbs", model);
        }


        #endregion

        #region "Methods"

        private FeaturedCommunityNewsSectionViewModel GetCommunityNewsSectionViewModel(PageType_Home page)
        {
            var newsSectionViewModel = new FeaturedCommunityNewsSectionViewModel()
            {
                NewsHeader = page.NewsHeader,
                NewsLinkedPageGuid = page.NewsLinkedPageGuid != Guid.Empty ? page.NewsLinkedPageGuid : (Guid?) null,
                NewsLinkText = page.NewsLinkText,                
                NewsNoRecordsDisplay = page.NewsNoRecordsDisplay,
                NewsLink = GetUrlLink(page.NewsLinkedPageGuid, page.DocumentCulture),
            };

            newsSectionViewModel.CommunityNewsItems = _homeService.GetFeaturedNewsItems(page.NewsNumberOfItemsToDisplay);

            return newsSectionViewModel;
        }

        private FeaturedJobOpportunitiesViewModel GetFeaturedJobOpportunitiesViewModel(PageType_Home page)
        {
            var featuredJobOpportunitiesViewModel = new FeaturedJobOpportunitiesViewModel()
            {
                JobsHeader = page.JobsHeader,
                JobsLinkText = page.JobsLinkText,
                JobsNoRecordsDisplay = page.JobsNoRecordsDisplay,
                JobsLink = GetUrlLink(page.JobLinkedPageGUID, page.DocumentCulture)
            };


            featuredJobOpportunitiesViewModel.JobOpportunityItems =
                _homeService.GetFeaturedJobItems(page.JobsNumberOfItemsToDisplay);

            return featuredJobOpportunitiesViewModel;
        }

        private FeaturedBoardOpportunitiesViewModel GetFeaturedBoardOpportunitiesViewModel(PageType_Home page)
        {
            var featuredJobOpportunitiesViewModel = new FeaturedBoardOpportunitiesViewModel()
            {
                BoardsHeader = page.BoardsHeader,
                BoardLinkText = page.BoardsLinkText,
                BoardsNoRecordsDisplay = page.BoardsNoRecordsDisplay,
                BoardsLink = GetUrlLink(page.BoardsLinkedPageGUID, page.DocumentCulture)
            };

            featuredJobOpportunitiesViewModel.BoardOpportunityItems = _homeService.GetFeaturedBoardOpportunityItems(page.BoardsNumberOfItemsToDisplay);

            return featuredJobOpportunitiesViewModel;
        }

        private FeaturedEventsViewModel GetFeaturedEventsViewModel(PageType_Home page)
        {
            var featuredEventsViewModel = new FeaturedEventsViewModel()
            {
                EventsHeader = page.EventsHeader,
                EventsLinkText = page.EventsLinkText,
                EventsNoRecordsDisplay = page.EventsNoRecordsDisplay,
                EventsLink = GetUrlLink(page.EventsLinkedPageGUID, page.DocumentCulture)
            };

            featuredEventsViewModel.EventItems = _homeService.GetFeaturedEventItems(
                page.EventsNumberOfItemsToDisplay, _authorizationService.CurrentUserHasCompetitorRole());

            return featuredEventsViewModel;
        }

        private FeaturedResourcesViewModel GetFeaturedResourcesViewModel(PageType_Home page)
        {
            var featuredResourcesViewModel = new FeaturedResourcesViewModel()
            {
                ResourcesHeader = page.ResourcesHeader,
                ResourcesLinkText = page.ResourcesLinkText,
                ResourcesNoRecordsDisplay = page.ResourcesNoRecordsDisplay,
                ResourcesLink = GetUrlLink(page.ResourcesLinkedPageGUID, page.DocumentCulture)
            };

            featuredResourcesViewModel.ResourceItems = _homeService.GetFeaturedResourceItems(
                page.ResourcesNumberOfItemsToDisplay, _authorizationService.CurrentUserHasCompetitorRole());

            return featuredResourcesViewModel;
        }


        private string GetUrlLink(Guid? linkedPageGuid, string culture)
        {
            if (linkedPageGuid.HasValue && linkedPageGuid.Value != Guid.Empty)
            {
                string url;

                if (_pageUrlService.TryGetPageMainUrl(linkedPageGuid.Value,
                    culture, out url))
                {
                    return url;
                }
            }

            return string.Empty;
        }

        protected LanguageToggleViewModel GetLanguageToggle(
            TreeNode page)
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
                LinkUrl = $"{url}{Request.Url?.Query}"
            };
        }

        private void SetSelectedNavItems(IEnumerable<NavigationItem> navigationItems, string currentPageNodeAliasPath)
        {
            if (navigationItems != null && !string.IsNullOrWhiteSpace(currentPageNodeAliasPath))
            {
                navigationItems.ForEach(navigationItem =>
                {
                    if (!string.IsNullOrWhiteSpace(navigationItem?.NodeAliasPath))
                    {
                        navigationItem.IsSelected = currentPageNodeAliasPath.StartsWith(navigationItem.NodeAliasPath);
                    }
                });
            }
        }

        #endregion
    }
}
