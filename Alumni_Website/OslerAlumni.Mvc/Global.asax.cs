using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CMS.Base;
using ECA.Mvc.PageURL.Macros;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Web.Mvc;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Core.Infrastructure;
using OslerAlumni.Mvc.Core.Models;
using OslerAlumni.Mvc.Extensions;

namespace OslerAlumni.Mvc
{
    public class MvcApplication
        : AutofacHttpApplication
    {
        #region "Private fields"

        protected static IDependencyResolver _diResolver;

        #endregion

        #region "Application events"

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var appBuilder = ApplicationBuilder.Current;

            ApplicationConfig.ConfigureDefaults();

            // Registers Kentico MVC features.
            // Note: Call this before initializing DI,
            // so that all necessary Kentico features that other components might depend on,
            // e.g. preview mode, are enabled
            ApplicationConfig.RegisterFeatures(
                appBuilder);




            // Defines classes for DI
            // Note: call this early, so that DI can be used in all of the following config classes
            _diResolver = ConfigureDependencies(
                GlobalConfiguration.Configuration);

            // Registers customized components, e.g. file system providers
            ApplicationConfig.RegisterCustomComponents(
                _diResolver);

            ApplicationConfig.RegisterModelBinders();

            GlobalConfiguration.Configure(config =>
            {
                // Registers Web API routes
                WebApiConfig.RegisterRoutes(config);

                // Registers Swagger UI
                // Note: putting Swagger pages under the same route as our Web API, 
                // lets it share the user session, since it is going to be enabled for all API routes
                SwaggerConfig.Register(
                    _diResolver,
                    config,
                    WebApiConfig.GlobalPrefix);
            });

            // Registers MVC routes
            RouteConfig.RegisterRoutes(
                _diResolver,
                RouteTable.Routes);


        }

        protected void Application_Error()
        {
            // Sets 404 HTTP exceptions to be handled via IIS
            // (behavior is specified in the "httpErrors" section in the Web.config file)
            var error = Server.GetLastError();

            if ((error as HttpException)?.GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                Server.ClearError();

                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        #endregion

        #region "Methods"

        protected override IDependencyResolver ConfigureDependencies(
            HttpConfiguration httpConfiguration)
        {
            var diResolver =
                base.ConfigureDependencies(
                    httpConfiguration);
            
            Startup.DIResolver = diResolver;
            OslerHtmlHelperExtensions.DIResolver = diResolver;
            PageUrlMacroMethods.DIResolver = diResolver;

            return diResolver;
        }

        protected void RedirectPermanent(
            string url)
        {
            Response.Clear();

            Response.StatusCode = (int)HttpStatusCode.MovedPermanently;

            Response.AddHeader(
                nameof(HttpResponseHeader.Location),
                url);

            Response.End();
        }


        /// <summary>
        /// Sets No cache headers on Http requests.
        /// This is needed because otherwise even after being logged out,
        /// if the user hits the back button they would still be able to 
        /// see previously protected resource. Also all our caching happens
        /// on server side any way.
        /// https://stackoverflow.com/questions/19315742/after-logout-if-browser-back-button-press-then-it-go-back-last-screen
        /// </summary>
        private void DisableClientCache()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        #endregion
    }
}
