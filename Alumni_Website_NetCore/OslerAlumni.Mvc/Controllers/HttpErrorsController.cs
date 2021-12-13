using CMS.DocumentEngine;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Content.Extensions;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Models;
using System;
using System.Linq;
using System.Net;

namespace OslerAlumni.Mvc.Controllers
{
    [InitializeViewBag]
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

        [HttpGet]
        [HttpPost]
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
                    LocalizationContext.CurrentCulture.CultureCode,
                    SiteContext.CurrentSiteName,
                    out page,
                    includeAllCoupledColumns: true))
            {
                throw new InvalidOperationException(
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

        [HttpGet]
        [HttpPost]
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;

            return Index(
                StandalonePageType.PageNotFound);
        }

        [HttpGet]
        [HttpPost]
        public ActionResult ServerError()
        {
            return Index(
                StandalonePageType.ServerError);
        }

        #endregion
    }
}
