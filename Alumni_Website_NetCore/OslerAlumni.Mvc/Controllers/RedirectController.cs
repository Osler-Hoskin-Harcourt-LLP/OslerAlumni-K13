using CMS.SiteProvider;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Services;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Extensions;

namespace OslerAlumni.Mvc.Controllers
{
    public class RedirectController
        : Controller
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly IPageUrlService _pageUrlService;
        private readonly ContextConfig _context;

        #endregion

        public RedirectController(
            IEventLogRepository eventLogRepository,
            IPageUrlService pageUrlService,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _pageUrlService = pageUrlService;

            _context = context;
        }

        #region "Actions"

        // GET: Redirect
        public ActionResult MainUrl(
            CustomTable_PageURLItem urlItem)
        {
            if ((urlItem == null) || urlItem.IsMainURL)
            {
                return this.BadRequest();
            }

            string redirectUrl;

            if (!_pageUrlService.TryGetPageMainUrl(
                    urlItem.NodeGUID,
                    urlItem.Culture,
                    out redirectUrl,
                   SiteContext.CurrentSiteName))
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(MainUrl),
                    $"Failed to obtain the main URL of a requested page: \r\n\r\nNode GUID: {urlItem.NodeGUID} \r\nCulture: {urlItem.Culture} \r\nPage URL record ID: {urlItem.ItemID}");
                
                return NotFound();
            }

            return new RedirectResult(
                redirectUrl,
                ShouldRedirectPermanently(urlItem, redirectUrl));
        }

        #endregion

        #region "Helper methods"

        protected bool ShouldRedirectPermanently(
            CustomTable_PageURLItem urlItem,
            string redirectUrl)
        {
            // Don't do a permanent (301) redirect on the root,
            // since it could go to either English or French home page,
            // depending on the current cultural context of the site
            return !_pageUrlService.IsRootUrl(
                urlItem?.URLPath);
        }

        #endregion
    }
}
