using System;
using System.Text;
using System.Web.Mvc;
using Amazon.Runtime.Internal.Util;
using ECA.Core.Models;
using ECA.PageURL.Services;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Api.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ISitemapService _sitemapService;

		/// <summary>
		/// Initializes a new instance of the <see cref="SitemapController"/> class.
		/// </summary>
		/// <param name="sitemapService">The sitemap service.</param>
		public SitemapController(ISitemapService sitemapService)
        {
            _sitemapService = sitemapService;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(string culture)
        {
            try
            {
                return Content(_sitemapService.GetSitemap(culture), "application/xml", Encoding.UTF8);
            }
            catch (Exception ex)
            {

                return HttpNotFound();
            }
        }
    }
}
