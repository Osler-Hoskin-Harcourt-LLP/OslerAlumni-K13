using CMS.DocumentEngine;
using CMS.Membership;
using ECA.Core.Models;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Models;
using ECA.Mvc.Navigation.Services;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterPageRoute(PageType_Home.CLASS_NAME, typeof(HomeController))]

namespace OslerAlumni.Mvc.Controllers
{
    public class HomeController
        : BaseController
    {
        #region "Private fields"

        private readonly IUserRepository _userRepository;
        private readonly IGlobalAssetService _globalAssetService;
        private readonly INavigationService _navigationService;

        private readonly IHomeService _homeService;
        private readonly IAuthorizationService _authorizationService;


        #endregion

        public HomeController(
             IAuthorizationService authorizationService,
             IUserRepository userRepository,
             IGlobalAssetService globalAssetService,
             INavigationService navigationService,
             IPageUrlService pageUrlService,
             IHomeService homeService,
             ContextConfig context,
             IPageDataContextRetriever dataRetriever)
             : base(pageUrlService, context, dataRetriever)
        {
            _authorizationService = authorizationService;
            _userRepository = userRepository;
            _globalAssetService = globalAssetService;
            _navigationService = navigationService;
            _homeService = homeService;
        }

        #region "Actions"

        // GET: Home
        [HttpGet]
        public IActionResult Index()
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
                homePageViewModel.FeaturedEventsViewModel = GetFeaturedEventsViewModel(page).GetAwaiter().GetResult();
                homePageViewModel.FeaturedResourcesViewModel = GetFeaturedResourcesViewModel(page).GetAwaiter().GetResult();
            }

            return View(homePageViewModel);
        }

        #endregion

        #region "Child actions"

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

        private async Task<FeaturedEventsViewModel> GetFeaturedEventsViewModel(PageType_Home page)
        {
            var featuredEventsViewModel = new FeaturedEventsViewModel()
            {
                EventsHeader = page.EventsHeader,
                EventsLinkText = page.EventsLinkText,
                EventsNoRecordsDisplay = page.EventsNoRecordsDisplay,
                EventsLink = GetUrlLink(page.EventsLinkedPageGUID, page.DocumentCulture)
            };

            featuredEventsViewModel.EventItems = _homeService.GetFeaturedEventItems(
                page.EventsNumberOfItemsToDisplay, await _authorizationService.CurrentUserHasCompetitorRole());

            return featuredEventsViewModel;
        }

        private async Task<FeaturedResourcesViewModel> GetFeaturedResourcesViewModel(PageType_Home page)
        {
            var featuredResourcesViewModel = new FeaturedResourcesViewModel()
            {
                ResourcesHeader = page.ResourcesHeader,
                ResourcesLinkText = page.ResourcesLinkText,
                ResourcesNoRecordsDisplay = page.ResourcesNoRecordsDisplay,
                ResourcesLink = GetUrlLink(page.ResourcesLinkedPageGUID, page.DocumentCulture)
            };

            featuredResourcesViewModel.ResourceItems = _homeService.GetFeaturedResourceItems(
                page.ResourcesNumberOfItemsToDisplay, await _authorizationService.CurrentUserHasCompetitorRole());

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





        #endregion
    }
}
