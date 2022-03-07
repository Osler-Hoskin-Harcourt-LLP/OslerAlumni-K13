using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_LandingPage.CLASS_NAME, typeof(EventsController), Path = "/Events", ActionName = nameof(EventsController.Index))]

namespace OslerAlumni.Mvc.Controllers
{
    [Authorize(Policy = "PublicPage")]
    public class EventsController
        : BaseController
    {
        public EventsController(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        { }

        #region "Actions"

        // GET: Events
        [HttpGet]
        public IActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_LandingPage>().Page;

            var landingPageViewModel =
                new LandingPageViewModel(page);

            return View(landingPageViewModel);
        }

        #endregion
    }
}
