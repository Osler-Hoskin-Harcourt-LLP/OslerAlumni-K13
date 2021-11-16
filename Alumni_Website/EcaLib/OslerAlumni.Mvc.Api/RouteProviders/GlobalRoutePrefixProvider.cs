using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace OslerAlumni.Mvc.Api.RouteProviders
{
    /// <summary>
    /// Applies a global prefix to all routes.
    /// </summary>
    /// <remarks>
    /// Based on https://www.strathweb.com/2015/10/global-route-prefixes-with-attribute-routing-in-asp-net-web-api/
    /// </remarks>
    public class GlobalRoutePrefixProvider
        : DefaultDirectRouteProvider
    {
        #region "Private fields"

        private readonly string _globalPrefix;

        #endregion

        public GlobalRoutePrefixProvider(
            string globalPrefix)
        {
            _globalPrefix = globalPrefix;
        }

        #region "Methods"

        protected override string GetRoutePrefix(
            HttpControllerDescriptor controllerDescriptor)
        {
            var routePrefix = base.GetRoutePrefix(
                controllerDescriptor);

            var prefixes = new []
            {
                _globalPrefix,
                routePrefix
            };

            return string.Join("/", prefixes);
        }

        #endregion
    }
}
