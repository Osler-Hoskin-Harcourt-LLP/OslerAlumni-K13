using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Helpers;
using OslerAlumni.Mvc.Core.Models;
using System.Text;

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
            filterContext.HttpContext.Request.EnableBuffering();
            if (!((Controller)filterContext.Controller).ModelState.IsValid)
            {
                string culture = filterContext.HttpContext.Request.Cookies[Constants.FormCulture];


                filterContext.Result =
                    new ValidationErrorJsonResult(((Controller)filterContext.Controller).ModelState, culture);
            }
        }

        private string GetCultureFromFormOrBody(HttpRequest request)
        {
            string culture;
            StringValues temp;
            if (request.ContentType.ToLower().Contains("form") )
            {
                culture = request.Form[Constants.FormCulture];
            }
            else
            {
                request.Body.Position = 0;
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    string rawMessage = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    dynamic data = JObject.Parse(rawMessage);
                    culture = data.form_culture;
                }
            }

            return culture;
        }
    }
}
