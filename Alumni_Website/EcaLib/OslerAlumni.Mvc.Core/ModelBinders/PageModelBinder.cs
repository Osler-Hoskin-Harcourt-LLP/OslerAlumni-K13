using System.Web.Mvc;
using CMS.DocumentEngine;
using ECA.Content.Extensions;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.ModelBinders
{
    public class PageModelBinder<T>
        : IModelBinder
        where T : TreeNode, new()
    {
        public object BindModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            var routeValues = controllerContext.RouteData.Values;

            var page = routeValues[Constants.RouteParams.Page] as TreeNode;

            return page?.ToPageType<T>();
        }
    }
}
