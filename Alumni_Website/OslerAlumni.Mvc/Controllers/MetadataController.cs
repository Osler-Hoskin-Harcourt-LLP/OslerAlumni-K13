using System.Web.Mvc;
using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Models;

namespace OslerAlumni.Mvc.Controllers
{
    public class MetadataController : BaseController
    {
        public MetadataController(IPageUrlService pageUrlService, ContextConfig context, IPageDataContextRetriever dataRetriever) : base(pageUrlService, context, dataRetriever)
        {
        }

        [ChildActionOnly]
        public ActionResult RenderMetadata(
            TreeNode page, MetadataViewModel overriddenMetaData = null)
        {
            var pageType = page as IBasePageType;

            var model = overriddenMetaData ?? new MetadataViewModel();

            if (pageType != null)
            {
                model.Title = StringHelper.GetNonEmpty(
                    model.Title,
                    pageType.DocumentPageTitle,
                    pageType.Title
                );

                model.PageDescription = StringHelper.GetNonEmpty(
                    model.PageDescription,
                    pageType.DocumentPageDescription,
                    pageType.ShortDescription
                );
            }

            return PartialView("_Metadata", model);
        }


    }
}
