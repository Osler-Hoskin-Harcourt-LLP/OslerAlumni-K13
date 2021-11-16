using System.Web.Mvc;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// Ensures that the ViewModel State is valid BEFORE the corresponding controller action is invoked.
    /// If the sate is invalid, an error summary is returned back to the caller as JSON.
    /// </summary>
    public class ValidateModelAttribute 
        : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                filterContext.Result = 
                    new ValidationErrorJsonResult(filterContext.Controller.ViewData.ModelState);

                HttpResponseHelper.SkipIisCustomErrors(filterContext.HttpContext);
            }
        }
    }
}
