using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace OslerAlumni.Mvc.Api.Handlers
{
    /// <summary>
    /// HttpControllerRouteHandler that uses an HttpControllerHandler with ASP.NET Session support.
    /// </summary>
    /// <remarks>
    /// Code taken verbatim from http://chsakell.com/2015/03/07/angularjs-feat-web-api-enable-session-state/
    /// </remarks> 
    public class SessionEnabledHttpControllerRouteHandler
        : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionEnabledControllerHandler(requestContext.RouteData);
        }
    }
}
