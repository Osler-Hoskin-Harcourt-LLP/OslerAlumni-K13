using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ECA.Mvc.Core.Services;
using OslerAlumni.Mvc.Core.Models;
using OslerAlumni.Mvc.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Handlers
{
    /// <summary>
    /// Creates an object that implements the <see cref="IHttpHandler"/> interface and passes the request context to it.
    /// Configures the current thread to use the culture specified by the 'culture' URL parameter.
    /// </summary>
    public class MultiCultureMvcRouteHandler
        : MvcRouteHandler
    {
        #region "Private fields"

        private readonly IDependencyResolver _diResolver;
        private readonly string _cultureParameterName;
        private readonly CultureInfo _defaultCulture;

        #endregion

        /// <summary>
        /// Creates a new instance of the <see cref="MultiCultureMvcRouteHandler"/> class.
        /// </summary>
        /// <param name="diResolver"></param>
        /// <param name="cultureParameterName"></param>
        /// <param name="defaultCulture">
        /// Culture used when the requested culture does not exist.
        /// </param>
        public MultiCultureMvcRouteHandler(
            IDependencyResolver diResolver,
            string cultureParameterName,
            CultureInfo defaultCulture)
        {
            _diResolver = diResolver;

            _cultureParameterName = cultureParameterName;
            _defaultCulture = defaultCulture;
        }

        #region "Methods"

        /// <summary>
        /// Returns the HTTP handler by using the specified HTTP context. 
        /// <see cref="Thread.CurrentCulture"/> and <see cref="Thread.CurrentUICulture"/>
        /// of the current thread are set to the culture specified by the 'culture' URL parameter.
        /// </summary>
        /// <param name="requestContext">Request context.</param>
        /// <returns>HTTP handler.</returns>
        protected override IHttpHandler GetHttpHandler(
            RequestContext requestContext)
        {
            var cookieStoreRepository =
                _diResolver.GetService<ICookieStoreRepository>();

            var cultureService = 
                _diResolver.GetService<ICultureService>();

            var routeValues = requestContext.RouteData.Values;

            string cultureKey;

            var culture = cultureService.GetCurrentCulture(
                routeValues,
                _cultureParameterName,
                _defaultCulture,
                out cultureKey);

            // Make sure that the route value of the culture is using the standardized value
            // that we obtained from our dictionary
            routeValues[_cultureParameterName] = cultureKey;

            // Set the cookie
            var cookie = cookieStoreRepository.Get<OslerCookie>();

            if (cookie.UserCulturePreference != cultureKey)
            {
                cookie.UserCulturePreference = cultureKey;
                cookieStoreRepository.Save(cookie);
            }

            // Set the culture
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            return base.GetHttpHandler(requestContext);
        }

        #endregion
    }
}
