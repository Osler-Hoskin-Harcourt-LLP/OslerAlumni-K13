using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using ECA.Core.Extensions;

namespace OslerAlumni.Mvc.Core.Constraints
{
    public class CultureConstraint
        : IRouteConstraint
    {
        #region "Private fields"

        private readonly Dictionary<string, string> _allowedCultureCodes;
        private readonly bool _isOptional;

        #endregion

        public CultureConstraint(
            Dictionary<string, string> allowedCultureCodes,
            bool isOptional)
        {
            _allowedCultureCodes = allowedCultureCodes;
            _isOptional = isOptional;
        }

        #region "Methods"

        public bool Match(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if ((_allowedCultureCodes == null) || (_allowedCultureCodes.Count < 1))
            {
                return true;
            }

            var cultureKey = values[parameterName]?.ToString();

            // If culture value is absent from the route,
            // allow it only if the parameter is supposed to be optional
            if (string.IsNullOrWhiteSpace(cultureKey))
            {
                return _isOptional;
            }

            // Get the correct version of the culture language key
            // (e.g. if "eN" is provided instead of "en", use "en")
            cultureKey = _allowedCultureCodes.Keys
                .GetItemOrdinalOrDefault(cultureKey);

            // If the requested culture is not in the list of allowed cultures,
            // block the route
            if (string.IsNullOrWhiteSpace(cultureKey))
            {
                return false;
            }

            // Make sure that the route value of the culture is using the standardized value
            // that we obtained from our dictionary
            values[parameterName] = cultureKey;

            return true;
        }

        #endregion
    }
}
