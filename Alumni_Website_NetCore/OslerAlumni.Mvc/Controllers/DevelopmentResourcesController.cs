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
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(DevelopmentResourcesController), Path = "/Careers/Career-Resources", ActionName = nameof(DevelopmentResourcesController.Index))]
[assembly: RegisterPageRoute(PageType_DevelopmentResource.CLASS_NAME, typeof(DevelopmentResourcesController), ActionName = nameof(DevelopmentResourcesController.Details))]

namespace OslerAlumni.Mvc.Controllers
{
    [Authorize]
    public class DevelopmentResourcesController
        : BaseController
    {
        #region "Private fields"

        private readonly IGlobalAssetService _globalAssetService;
        private readonly IDevelopmentResourceService _developmentResourceService;

        #endregion

        public DevelopmentResourcesController(
            IGlobalAssetService globalAssetService,
            IDevelopmentResourceService developmentResourceService,
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _globalAssetService = globalAssetService;
            _developmentResourceService = developmentResourceService;
        }

        #region "Actions"

        [HttpGet]
        public IActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new DevelopmentResourceLandingPageViewModel(page)
                {
                    DefaultImageUrl = _globalAssetService.DefaultImageUrl
                };

            var featuredResource =
                _developmentResourceService.GetFeaturedDevelopmentResource(page);

            if (featuredResource != null)
            {
                bool isExternal;

                string url = GetResourceUrl(featuredResource, out isExternal);

                if (!string.IsNullOrWhiteSpace(url))
                {
                    landingPageViewModel.FeaturedItem =
                        new ResourceFeaturedItemViewModel
                        {
                            NodeGuid = featuredResource.NodeGUID,
                            FeaturedText =
                                ResHelper.GetString(Constants.ResourceStrings.DevelopmentResource
                                    .FeaturedDevelopmentResource),
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

        public ActionResult Details()
        {
            var page = _dataRetriever.Retrieve<PageType_DevelopmentResource>().Page;

            var resourceDetailsPageViewModel =
                new DevelopmentResourceDetailsPageViewModel(page)
                {
                    ResourceTypeDisplayName =
                        _developmentResourceService.GetResourceTypesDisplayString(page.DevelopmentResourceTypeArray)
                };


            return View(resourceDetailsPageViewModel);
        }

        #endregion

        #region HelperMethods

        private string GetResourceUrl(PageType_DevelopmentResource resource, out bool isExternal)
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
