using CMS.Helpers;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(ResourcesController), Path = "/Resources", ActionName = nameof(ResourcesController.Index))]
[assembly: RegisterPageRoute(PageType_Resource.CLASS_NAME, typeof(ResourcesController), ActionName = nameof(ResourcesController.Details))]


namespace OslerAlumni.Mvc.Controllers
{
    [Authorize(Policy = "PublicPage")]
    public class ResourcesController 
        : BaseController
    {
        #region "Private fields"

        private readonly IGlobalAssetService _globalAssetService;
        private readonly IResourceService _resourceService;
        private readonly Core.Services.IAuthorizationService _authorizationService;

        #endregion

        public ResourcesController(
            IGlobalAssetService globalAssetService,
            IResourceService newsService,
            IPageUrlService pageUrlService,
            IResourceTypeItemRepository resourceTypeItemRepository,
            Core.Services.IAuthorizationService authorizationService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _globalAssetService = globalAssetService;
            _resourceService = newsService;
            _authorizationService = authorizationService;
        }

        #region "Actions"

        [HttpGet]
        public IActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new ResourceLandingPageViewModel(page)
                {
                    DefaultImageUrl = _globalAssetService.DefaultImageUrl
                };

            var featuredResource =
                _resourceService.GetFeaturedResource(page);

            if (featuredResource != null)
            {

                if (featuredResource.HideFromCompetitors && _authorizationService.CurrentUserHasCompetitorRole().GetAwaiter().GetResult())
                {
                    featuredResource = null;
                }

                bool isExternal;

                var url = GetResourceUrl(featuredResource, out isExternal);


                if (!string.IsNullOrWhiteSpace(url))
                {
                    landingPageViewModel.FeaturedItem =
                        new ResourceFeaturedItemViewModel
                        {
                            NodeGuid = featuredResource.NodeGUID,
                            FeaturedText = ResHelper.GetString(Constants.ResourceStrings.Resource.FeaturedResource),
                            Title = featuredResource.Title,
                            DefaultImageUrl = page.DefaultFeaturedImage,
                            PageUrl = url,
                            IsExternal = isExternal,
                            IsFile = featuredResource.IsFile
                        };
                }
            }

            return View(landingPageViewModel);
        }

        public IActionResult Details()
        {
            var page = _dataRetriever.Retrieve<PageType_Resource>().Page;

            if (page.HideFromCompetitors && _authorizationService.CurrentUserHasCompetitorRole().GetAwaiter().GetResult())
            {
                return NotFound();
            }

            var resourceDetailsPageViewModel =
                new ResourceDetailsPageViewModel(page)
                {
                    ResourceTypeDisplayName =
                        _resourceService.GetResourceTypesDisplayString(page.TypeArray)
                };

            return View(resourceDetailsPageViewModel);
        }

        private string GetResourceUrl(PageType_Resource resource, out bool isExternal)
        {
            isExternal = false;

            if (resource == null)
            {
                return null;
            }

            string url;

            isExternal = !string.IsNullOrWhiteSpace(resource.ExternalUrl);

            if (isExternal)
            {
                url = URLHelper.ResolveUrl(resource.ExternalUrl);
            }
            else
            {
                _pageUrlService.TryGetPageMainUrl(resource, out url);
            }

            return url;
        }


        #endregion
    }
}
