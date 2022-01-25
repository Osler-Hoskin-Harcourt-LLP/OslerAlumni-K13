using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Global
{
    public class BreadCrumbsViewComponent : BaseViewComponent
    {
        private readonly IBreadCrumbService _breadCrumbService;

        public BreadCrumbsViewComponent(IPageUrlService pageUrlService, ContextConfig context, IPageDataContextRetriever dataRetriever, IBreadCrumbService breadCrumbService) : base(pageUrlService, context, dataRetriever)
        {
            _breadCrumbService = breadCrumbService;
        }

        public async Task<IViewComponentResult> InvokeAsync(TreeNode page, bool? showBreadCrumbs = true)
        {
            BreadCrumbsViewModel model = null;

            if (!showBreadCrumbs.HasValue || showBreadCrumbs.Value)
            {
                model = new BreadCrumbsViewModel();

                model.BreadCrumbs = _breadCrumbService.GetBreadCrumbs(page?.NodeAliasPath);
            }

            return View("_BreadCrumbs", model);
        }
    }
}
