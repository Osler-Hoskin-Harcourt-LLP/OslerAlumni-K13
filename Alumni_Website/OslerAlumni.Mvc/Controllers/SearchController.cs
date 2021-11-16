using System.Web.Mvc;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(SearchController), Path = "/Search", ActionName = nameof(SearchController.Index))]

namespace OslerAlumni.Mvc.Controllers
{
    [OslerAuthorize]
    public class SearchController
        : BaseController
    {
        public SearchController(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        { }

        #region "Actions"
        
        [HttpGet]
        public ActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new LandingPageViewModel(page);

            return View(landingPageViewModel);
        }

        #endregion
    }
}
