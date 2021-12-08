using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Services;
using ECA.Mvc.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using OslerAlumni.Mvc.Core.Models;
using OslerAlumni.Mvc.Core.Repositories;
using System.Globalization;

namespace OslerAlumni.Mvc.Core.Services
{
    public class CultureService
        : ServiceBase, ICultureService
    {
        #region "Private fields"

        private readonly ICookieStoreRepository _cookieStoreRepository;
        private readonly ContextConfig _context;
        private readonly IActionContextAccessor _actionContext;


        #endregion

        public CultureService(
            ICookieStoreRepository cookieStoreRepository,
            ContextConfig context, IActionContextAccessor actionContext)
        {
            _cookieStoreRepository = cookieStoreRepository;

            _context = context;

            _actionContext = actionContext;
        }

        #region "Methods"

        /// <summary>
        /// Uses <see cref="HttpContext.Current" /> to obtain current culture.
        /// </summary>
        /// <param name="cultureParameterName"></param>
        /// <param name="defaultCulture"></param>
        /// <param name="cultureKey"></param>
        /// <returns></returns>
        public CultureInfo GetCurrentCulture(
            string cultureParameterName,
            CultureInfo defaultCulture,
            out string cultureKey)
        {
            // This basically gets us the route based on the URL structure of the current request;
            // route value dictionary will be populated accordingly
            var routeData = _actionContext.ActionContext.RouteData;

            return GetCurrentCulture(
                routeData?.Values,
                cultureParameterName,
                defaultCulture,
                out cultureKey);
        }

        public CultureInfo GetCurrentCulture(
            RouteValueDictionary routeValues,
            string cultureParameterName,
            CultureInfo defaultCulture,
            out string cultureKey)
        {
            CultureInfo culture = null;

            string cultureName;

            // Get the requested culture key from the route
            cultureKey =
                routeValues?[cultureParameterName]?.ToString();

            // Get the matching full culture name from the list of allowed cultures
            // (e.g. "en-CA" for the culture key "en")
            if (_context.AllowedCultureCodes
                    .TryGetValueByOrdinalKey(cultureKey, out cultureName)
                && !string.IsNullOrWhiteSpace(cultureName))
            {
                try
                {
                    culture = new CultureInfo(cultureName);
                }
                catch
                {
                    // ignore
                }
            }

            // Get culture from cookie
            if (culture == null)
            {
                if (_cookieStoreRepository.Exists<OslerCookie>())
                {
                    try
                    {
                        cultureKey = _cookieStoreRepository
                            .Get<OslerCookie>()
                            .UserCulturePreference;

                        // Get the matching full culture name from the list of allowed cultures
                        // (e.g. "en-CA" for the culture key "en")
                        if (_context.AllowedCultureCodes
                                .TryGetValueByOrdinalKey(cultureKey, out cultureName)
                            && !string.IsNullOrWhiteSpace(cultureName))
                        {
                            culture = new CultureInfo(cultureName);
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }

            // Get the matching full culture name from the list of allowed cultures
            // (e.g. "en-CA" for the culture key "en")
            if ((culture == null)
                || !_context.AllowedCultureCodes.TryGetKeyByOrdinalValue(
                        culture.Name,
                        out cultureKey))
            {
                culture = defaultCulture;

                _context.AllowedCultureCodes
                    .TryGetKeyByOrdinalValue(
                        defaultCulture.Name,
                        out cultureKey);
            }

            return culture;
        }

        #endregion
    }
}
