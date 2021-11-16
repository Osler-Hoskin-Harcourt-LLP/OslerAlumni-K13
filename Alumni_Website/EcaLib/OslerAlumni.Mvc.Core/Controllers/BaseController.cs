using System;
using System.Web.Mvc;
using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Newtonsoft.Json;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Attributes.Error;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Controllers
{
    [InitializeViewBag]
    [HandleMvcException]
    [AddActionParametersToViewBag(Constants.RouteParams.Page)]
    public abstract class BaseController
        : Controller
    {
        #region "Private fields"

        protected readonly IPageUrlService _pageUrlService;

        protected readonly ContextConfig _context;

        protected readonly IPageDataContextRetriever _dataRetriever;

        #endregion

        protected BaseController(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
        {
            _pageUrlService = pageUrlService;

            _context = context;

            ViewBag.Context = context;
            _dataRetriever = dataRetriever;

            try
            {
                ViewBag.Page = _dataRetriever.Retrieve<TreeNode>().Page;
            }
            catch (Exception e)
            {
                //nothing, this is just for the controller action does not associate with any page. such as account logout
            }
        }

        #region "Methods"
        
        /// <summary>
        /// Not to be confused with the built-in Json Extension method.
        /// This is so that our custom serializations rules get applied.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ContentResult JsonContent(
            object obj)
        {
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        protected ActionResult RedirectToPage(
            StandalonePageType standalonePageType, object queryStringObj = null)
        {
            string url;

            if (!_pageUrlService.TryGetPageMainUrl(
                    standalonePageType,
                    _context.CultureName,
                    out url))
            {
                return HttpNotFound();
            }

            if (queryStringObj != null)
            {
                url = _pageUrlService.GetUrl(
                    url,
                    queryStringObj);
            }

            return new RedirectResult(url);
        }

        #endregion
    }
}
