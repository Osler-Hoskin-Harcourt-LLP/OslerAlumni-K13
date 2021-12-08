using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            if (!((Controller)filterContext.Controller).ModelState.IsValid)
            {
                filterContext.Result = 
                    new ValidationErrorJsonResult(((Controller)filterContext.Controller).ModelState);
            }
        }
    }
}
