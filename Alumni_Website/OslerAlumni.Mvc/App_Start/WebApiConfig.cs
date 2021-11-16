using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.WebHost;
using OslerAlumni.Mvc.Api.Handlers;
using OslerAlumni.Mvc.Api.RouteProviders;

namespace OslerAlumni.Mvc
{
    public static class WebApiConfig
    {
        #region "Constants"

        public const string GlobalPrefix = "api";

        #endregion

        #region "Methods"

        public static bool IsApiRoute(string path)
        {
            return path?.StartsWith("/" + GlobalPrefix) ?? false;
        }

        public static void RegisterRoutes(
            HttpConfiguration config)
        {
            EnableSessionForWebApiRoutes();

            // Enables attribute routing
            // All Web API routes will be automatically prefixed with /api/
            config.MapHttpAttributeRoutes(
                new GlobalRoutePrefixProvider(GlobalPrefix));
        }

        #endregion

        #region "Helper methods"

        private static void EnableSessionForWebApiRoutes()
        {
            // NOTE: This is a hacky way of doing it, but seems to be the leanest solution
            // that works with attribute routing
            // Based on https://www.wiliam.com.au/wiliam-blog/enabling-session-state-in-web-api,
            // which in turn is based on https://www.apress.com/us/book/9781430259800
            var httpControllerRouteHandler = typeof(HttpControllerRouteHandler)
                .GetField(
                    "_instance",
                    BindingFlags.Static | BindingFlags.NonPublic);

            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(
                    null,
                    new Lazy<HttpControllerRouteHandler>(
                        () => new SessionEnabledHttpControllerRouteHandler(),
                        true));
            }
        }

        #endregion
    }
}