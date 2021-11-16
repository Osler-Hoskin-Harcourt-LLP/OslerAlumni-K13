using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace OslerAlumni.Mvc.Api.Handlers
{
    /// <summary>
    /// HttpControllerHandler that supports ASP.NET Session.
    /// </summary>
    /// <remarks>
    /// Code taken verbatim from http://chsakell.com/2015/03/07/angularjs-feat-web-api-enable-session-state/
    /// </remarks>    
    public class SessionEnabledControllerHandler 
        : HttpControllerHandler, IRequiresSessionState
    {
        public SessionEnabledControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }
}
