using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Services;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Global
{
    public class RelatedCtasViewComponent : BaseViewComponent
    {
        private readonly ICtaService _ctaService;

        public RelatedCtasViewComponent(
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever, ICtaService ctaService) : base(pageUrlService, context, dataRetriever)
        {
            _ctaService = ctaService;
        }

        public async Task<IViewComponentResult> InvokeAsync(TreeNode page)
        {
            return View("_RelatedContentSection", _ctaService.GetRelatedContentCtas(page));
        }

    }
}
