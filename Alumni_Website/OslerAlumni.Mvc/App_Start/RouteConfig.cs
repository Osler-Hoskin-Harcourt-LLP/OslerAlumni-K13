using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using ECA.Mvc.Navigation.Extensions;
using ECA.Mvc.PageURL.Models;
using Kentico.Web.Mvc;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Constraints;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Handlers;

namespace OslerAlumni.Mvc
{
    public class RouteConfig
    {
        #region "Constants"
        
        public static readonly string HttpErrorsControllerName = 
            typeof(HttpErrorsController).GetControllerName();

        public static readonly string NotFoundActionName = 
            nameof(HttpErrorsController.NotFound);

        #endregion

        #region "Methods"

        public static void RegisterRoutes(
            IDependencyResolver diResolver,
            RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Ignores favicon and other .ico files
            routes.IgnoreRoute("{*iconPath}", new { iconPath = @".+\.ico$" });

            // Maps default Kentico routes (e.g. attachment or media library) to appropriate handlers
            routes.Kentico().MapRoutes();

            /*
             Note:
             Thread.CurrentThread.CurrentUICulture and Thread.CurrentThread.CurrentCulture are properties of the .NET framework
             and use the System.Globalization.CultureInfo type. They are not directly comparable with the
             Kentico CMS.Localization.CultureInfo type, or the CurrentUICulture and CurrentCulture properties of the
             CMS.Localization.LocalizationContext class.
             */

            var allowedCultureCodes =
                GlobalConstants.Cultures.AllowedCultureCodes;

            var defaultCulture =
                CultureInfo.GetCultureInfo(GlobalConstants.Cultures.Default);

            var customRoutes = GetCustomRoutes(
                routes,
                diResolver,
                allowedCultureCodes,
                defaultCulture);

            // A route value determines the culture of the current thread
            customRoutes.ForEach(route =>
            {
                route.RouteHandler = new MultiCultureMvcRouteHandler(
                    diResolver,
                    Constants.RouteParams.Culture,
                    defaultCulture);
            });
        }

        #endregion

        #region "Helper methods"

        protected static List<Route> GetCustomRoutes(
            RouteCollection routes,
            IDependencyResolver diResolver,
            Dictionary<string, string> allowedCultureCodes,
            CultureInfo defaultCulture)
        {
            return new List<Route>
            {
                // TODO##
                //routes.MapRoute(
                //    name: "KenticoRoute",
                //    url: "{culture}/{*path}",
                //    defaults: new
                //    {
                //        controller = HttpErrorsControllerName,
                //        action = NotFoundActionName,
                //        culture = UrlParameter.Optional
                //    },
                //    constraints: new
                //    {
                //        culture = new CultureConstraint(
                //            allowedCultureCodes,
                //            true),
                //        path = new PageUrlConstraint(
                //            diResolver,
                //            GetPageUrlConstraintSettings(defaultCulture))
                //    }),
                routes.MapRoute(
                    name: "MvcRoute",
                    url: "{controller}/{action}",
                    defaults: new
                    {
                        controller = HttpErrorsControllerName,
                        action = NotFoundActionName
                    }),
                routes.MapRoute(
                    name: "MvcLocalizedRoute",
                    url: "{culture}/{controller}/{action}",
                    defaults: new
                    {
                        controller = HttpErrorsControllerName,
                        action = NotFoundActionName
                    })
                //routes.MapRoute(
                //    name: "NotFoundRoute",
                //    url: "{*url}",
                //    defaults: new
                //    {
                //        controller = HttpErrorsControllerName,
                //        action = NotFoundActionName
                //    }
                //)
            };
        }
        
        protected static PageUrlConstraintSettings GetPageUrlConstraintSettings(
            CultureInfo defaultCulture)
        {
            return new PageUrlConstraintSettings
            {
                ControllerRouteParameter =
                    Constants.RouteParams.Controller,
                ActionRouteParameter =
                    Constants.RouteParams.Action,
                CultureRouteParameter =
                    Constants.RouteParams.Culture,
                PageRouteParameter =
                    Constants.RouteParams.Page,
                ControllerFieldName =
                    nameof(PageType_BasePageType.DefaultController),
                ActionFieldName =
                    nameof(PageType_BasePageType.DefaultAction),
                DefaultCulture =
                    defaultCulture,
                RedirectControllerName =
                    typeof(RedirectController).GetControllerName(),
                MainUrlRedirectActionName =
                    nameof(RedirectController.MainUrl),
                UrlItemRouteParameter =
                    Constants.RouteParams.UrlItem
            };
        }

        #endregion
    }
}
