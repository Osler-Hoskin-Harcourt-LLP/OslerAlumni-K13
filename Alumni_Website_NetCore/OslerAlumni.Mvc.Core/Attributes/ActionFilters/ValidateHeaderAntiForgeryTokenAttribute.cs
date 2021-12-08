

using Microsoft.AspNetCore.Mvc.Filters;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /**
     * This action filter is need in cases where Ajax requests are being made to Post to Apis.
     * By default csrf token is only validated on form data not JSON. This allows the token to
     * be passed as a header in those situations instead.
     */
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]

    public class ValidateHeaderAntiForgeryTokenAttribute 
        : ActionFilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var httpContext = context.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie, httpContext.Request.Headers["__requestverificationtoken"]);
        }
    }
}
