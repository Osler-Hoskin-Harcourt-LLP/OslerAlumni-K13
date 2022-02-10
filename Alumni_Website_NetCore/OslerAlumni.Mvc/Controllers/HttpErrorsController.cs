using CMS.DocumentEngine;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Content.Extensions;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Helpers;
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
        private readonly IPageDataContextInitializer _pageDataContextInitializer;

        #endregion

        public HttpErrorsController(
            IPageService pageService,
            ContextConfig context,
            IPageDataContextInitializer pageDataContextInitializer)
        {
            _pageService = pageService;

            _context = context;
            _pageDataContextInitializer = pageDataContextInitializer;
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

            var statusCodeReExecuteFeature =
            HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            TreeNode page;

            var culture = CustomCultureHelper.GetCultureCodeFromUrl(Request.Path);
            LocalizationContext.CurrentCulture = culture;
            if (!_pageService.TryGetStandalonePage(
                    errorPageType,
                   culture.CultureCode,
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

            _pageDataContextInitializer.Initialize(genericPage);

            var pageViewModel = new PageViewModel(genericPage);

            return View("Index", pageViewModel);
        }

        [HttpGet]
        [HttpPost]
        public ActionResult Error(string statusCode)
        {
            if (statusCode == "404")
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;

                return Index(
                    StandalonePageType.PageNotFound);
            }
            else
            {
                return Index(
               StandalonePageType.ServerError);
            }

        }

        #endregion
    }
}
