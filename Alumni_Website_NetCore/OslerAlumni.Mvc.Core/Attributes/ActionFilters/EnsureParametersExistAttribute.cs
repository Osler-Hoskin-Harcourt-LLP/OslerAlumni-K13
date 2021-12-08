

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// Ensure that certain parameters that a controller requires exist and are not null.
    /// Otherwise redirect to a 404 page.
    ///     
    /// Usage:
    /// Decorate the controller action by:
    /// 
    /// [EnsureParametersExist("page")]
    /// public ActionResult RequestPasswordReset(PageType_Page page)
    /// </summary>
    public class EnsureParametersExistAttribute
        : ActionFilterAttribute
    {
        #region "Private fields"

        private readonly string[] _parameterNames;

        #endregion

        public EnsureParametersExistAttribute(
            params string[] parameterNames)
        {
            _parameterNames = parameterNames;
        }

        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            var viewDataDictionary = ((Controller)filterContext.Controller).ViewData;

            var emptyParameters = _parameterNames
                .Any(parameterName =>
                    !viewDataDictionary.ContainsKey(parameterName)
                    || (viewDataDictionary[parameterName] == null));

            if (emptyParameters)
            {
                filterContext.Result = new NotFoundResult();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
