using ECA.Core.Models;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Services;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Models;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Global
{
    public class FooterViewComponent : BaseViewComponent
    {
        private readonly INavigationService _navigationService;
        private readonly IUserRepository _userRepository;
        private readonly IGlobalAssetService _globalAssetService;


        public FooterViewComponent(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever,
            INavigationService navigationService,
            IUserRepository userRepository,
            IGlobalAssetService globalAssetService) : base(pageUrlService, context, dataRetriever)
        {
            _navigationService = navigationService;
            _userRepository = userRepository;
            _globalAssetService = globalAssetService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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

            return View("_Footer", footer);
        }
    }
}
