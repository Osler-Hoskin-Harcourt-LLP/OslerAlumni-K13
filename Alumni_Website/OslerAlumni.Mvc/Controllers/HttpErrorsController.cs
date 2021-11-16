using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CMS.DocumentEngine;
using ECA.Content.Extensions;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Attributes.Error;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Models;

namespace OslerAlumni.Mvc.Controllers
{
    [InitializeViewBag]
    [HandleMvcException]
    public class HttpErrorsController
        : Controller
    {
        #region "Constants"

        protected static readonly StandalonePageType[] ErrorPageTypes =
        {
            StandalonePageType.PageNotFound,
            StandalonePageType.ServerError
        };

        #endregion

        #region "Private fields"

        private readonly IPageService _pageService;
        private readonly ContextConfig _context;

        #endregion

        public HttpErrorsController(
            IPageService pageService,
            ContextConfig context)
        {
            _pageService = pageService;

            _context = context;
        }

        #region "Actions"

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(
            StandalonePageType errorPageType)
        {
            if (!ErrorPageTypes.Contains(errorPageType))
            {
                return this.BadRequest();
            }

            TreeNode page;

            if (!_pageService.TryGetStandalonePage(
                    errorPageType,
                    _context.CultureName,
                    _context.Site?.SiteName,
                    out page,
                    includeAllCoupledColumns: true))
            {
                throw new ObjectNotFoundException(
                    $"Could not find a page corresponding to '{errorPageType}' error in the content tree.");
            }

            var genericPage = page.ToPageType<PageType_Page>();

            if (genericPage == null)
            {
                throw new InvalidCastException(
                    $"Incorrect page type casting from '{nameof(TreeNode)}' to '{nameof(PageType_Page)}'.");
            }

            var pageViewModel = new PageViewModel(genericPage);

            return View("Index", pageViewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            // NOTE: If we don't call this, there will be another request,
            // this time with `?404;<original URL>` appended to the Page Not Found URL
            HttpResponseHelper.SkipIisCustomErrors(ControllerContext.HttpContext);

            return Index(
                StandalonePageType.PageNotFound);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ServerError()
        {
            return Index(
                StandalonePageType.ServerError);
        }

        #endregion
    }
}
