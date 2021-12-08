using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Services;
using System;
using System.Text;

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
        public IActionResult Index(string culture)
        {
            try
            {
                return Content(_sitemapService.GetSitemap(culture), "application/xml", Encoding.UTF8);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }
    }
}
