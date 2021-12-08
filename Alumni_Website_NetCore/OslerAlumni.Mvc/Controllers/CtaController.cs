using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Controllers
{
    public class CtaController : BaseController
    {
        private readonly ICtaService _ctaService;

        public CtaController(
            ICtaService ctaService,
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever) : 
            base(pageUrlService, context, dataRetriever)
        {
            _ctaService = ctaService;
        }

        public IActionResult RelatedContentCtas(
            TreeNode page)
        {
            return PartialView("_RelatedContentSection", _ctaService.GetRelatedContentCtas(page));
        }

        public IActionResult TopWidgetZoneCtas(
            TreeNode page)
        {
            return PartialView("_TopWidgetZoneSection", _ctaService.GetTopWidgetZoneCtasAsync(page));
        }
    }
}
