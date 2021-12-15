using CMS.DocumentEngine;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Models;
using System;
using System.Threading.Tasks;

namespace OslerAlumniWebsite.ViewComponents.Global
{
    public class MetaDataViewComponent : BaseViewComponent
    {
        public MetaDataViewComponent(IPageUrlService pageUrlService, ContextConfig context, IPageDataContextRetriever dataRetriever) : base(pageUrlService, context, dataRetriever)
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(MetadataViewModel overriddenMetaData = null)
        {

            try
            {
                var page = _dataRetriever.Retrieve<TreeNode>().Page;

                var model = overriddenMetaData ?? new MetadataViewModel();

                if (page != null)
                {
                    model.Title = StringHelper.GetNonEmpty(
                        model.Title,
                        page.DocumentPageTitle,
                        page.GetStringValue("Title", string.Empty)
                    );

                    model.PageDescription = StringHelper.GetNonEmpty(
                        model.PageDescription,
                        page.DocumentPageDescription,
                        page.GetStringValue("ShortDescription", string.Empty)
                    );
                }

                return View("_Metadata", model);
            }
            catch (Exception e)
            {
                return Content("");
            }


        }

    }
}
