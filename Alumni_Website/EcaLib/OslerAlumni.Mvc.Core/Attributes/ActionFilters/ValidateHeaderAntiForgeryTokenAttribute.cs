using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /**
     * This action filter is need in cases where Ajax requests are being made to Post to Apis.
     * By default csrf token is only validated on form data not JSON. This allows the token to
     * be passed as a header in those situations instead.
     */
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]

    public class ValidateHeaderAntiForgeryTokenAttribute 
        : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            var httpContext = filterContext.HttpContext;
            var cookie = httpContext.Request.Cookies[AntiForgeryConfig.CookieName];
            AntiForgery.Validate(cookie?.Value, httpContext.Request.Headers["__requestverificationtoken"]);
        }
    }
}
