using System.Web.Mvc;
using ECA.Core.Models;
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
[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(PagesController))]

namespace OslerAlumni.Mvc.Controllers
{
    [OslerAuthorize]
    public class PagesController
        : BaseController
    {
        public PagesController(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        { }

        #region "Actions"

        [HttpGet]
        public ActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            var pageViewModel = new PageViewModel(page);

            return View(pageViewModel);
        }

        #endregion
    }
}
