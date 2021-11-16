using System.Web.Mvc;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(NewsController), Path = "/Community-News", ActionName = nameof(JobsController.Index))]
[assembly: RegisterPageRoute(PageType_News.CLASS_NAME, typeof(NewsController), ActionName = nameof(JobsController.Details))]

namespace OslerAlumni.Mvc.Controllers
{
    [OslerAuthorize]
    public class NewsController
        : BaseController
    {
        #region "Private fields"

        private readonly IGlobalAssetService _globalAssetService;
        private readonly INewsService _newsService;

        #endregion

        public NewsController(
            IGlobalAssetService globalAssetService,
            INewsService newsService,
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _globalAssetService = globalAssetService;
            _newsService = newsService;
        }

        #region "Actions"

        // GET: News
        [HttpGet]
        public ActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new NewsLandingPageViewModel(page)
                {
                    DefaultImageUrl = _globalAssetService.DefaultImageUrl
                };

            var featuredNews = 
                _newsService.GetFeaturedNews(page);

            string url;

            if ((featuredNews != null)
                    && _pageUrlService.TryGetPageMainUrl(featuredNews, out url))
            {
                landingPageViewModel.FeaturedItem =
                    new NewsFeaturedItemViewModel
                    {
                        NodeGuid = featuredNews.NodeGUID,
                        NewsPageType = featuredNews.NewsPageType,
                        Title = featuredNews.Title,
                        ImageUrl = featuredNews.Image,
                        ImageAltText = featuredNews.ImageAltText,
                        DefaultImageUrl = page.DefaultFeaturedImage,
                        PageUrl = url
                    };
            }

            return View(landingPageViewModel);
        }
        
        public ActionResult Details()
        {
            var page = _dataRetriever.Retrieve<PageType_News>().Page;

            var newsDetailsPageViewModel = new NewsDetailsPageViewModel(page);

            return View(newsDetailsPageViewModel);
        }

        #endregion
    }
}
