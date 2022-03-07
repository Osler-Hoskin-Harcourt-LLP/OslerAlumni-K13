using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(CareersController), Path = "/Careers", ActionName = nameof(CareersController.Index))]


namespace OslerAlumni.Mvc.Controllers
{
    [Authorize(Policy = "PublicPage")]
    public class CareersController
        : BaseController
    {

        public CareersController(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
        }

        #region "Actions"
        [HttpGet]
        public IActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            var pageViewModel = new PageViewModel(page);

            return View(pageViewModel);
        }

        #endregion
    }
}
