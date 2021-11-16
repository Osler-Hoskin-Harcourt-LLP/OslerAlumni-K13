using System.Globalization;

namespace ECA.Mvc.PageURL.Models
{
    public class PageUrlConstraintSettings
    {
        /// <summary>
        /// Name of the route parameter that specifies the MVC controller.
        /// </summary>
        public string ControllerRouteParameter { get; set; }

        /// <summary>
        /// Name of the route parameter that specifies the MVC controller action.
        /// </summary>
        public string ActionRouteParameter { get; set; }

        /// <summary>
        /// Name of the route parameter that specifies the requested culture.
        /// </summary>
        public string CultureRouteParameter { get; set; }

        /// <summary>
        /// Name of the route parameter that specifies the target Kentico page.
        /// </summary>
        public string PageRouteParameter { get; set; }

        /// <summary>
        /// Name of the Kentico Page Type field that stores
        /// the reference to the target MVC controller.
        /// </summary>
        public string ControllerFieldName { get; set; }

        /// <summary>
        /// Name of the Kentico Page Type field that stores
        /// the reference to the target MVC controller action.
        /// </summary>
        public string ActionFieldName { get; set; }

        /// <summary>
        /// Default culture that should be used if the a correct, allowed culture
        /// has not been specified in the route.
        /// </summary>
        public CultureInfo DefaultCulture { get; set; }

        /// <summary>
        /// Name of the controller that should be used for URL redirection in the constraint.
        /// </summary>
        public string RedirectControllerName { get; set; }

        /// <summary>
        /// Name of the redirect controller action that should be used
        /// to initiate a redirect to the main URL of the page.
        /// </summary>
        public string MainUrlRedirectActionName { get; set; }

        /// <summary>
        /// Name of the route parameter that specifies the URL item that triggered the redirect.
        /// </summary>
        public string UrlItemRouteParameter { get; set; }
    }
}
