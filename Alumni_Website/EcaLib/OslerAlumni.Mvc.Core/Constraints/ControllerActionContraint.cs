using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Routing;
using ECA.Core.Extensions;

namespace OslerAlumni.Mvc.Core.Constraints
{
    public class ControllerActionContraint
        : IRouteConstraint
    {
        #region "Private fields"
        
        private readonly string _controllerParameterName;
        private readonly Dictionary<string, IList<KeyValuePair<HttpMethod, string>>> 
            _blockedControllerActions;

        #endregion

        public ControllerActionContraint(
            string controllerParameterName,
            Dictionary<string, IList<KeyValuePair<HttpMethod, string>>> blockedControllerActions)
        {
            _controllerParameterName = controllerParameterName;
            _blockedControllerActions = blockedControllerActions;
        }

        #region "Methods"

        public bool Match(
            HttpContextBase httpContext, 
            Route route, 
            string parameterName, 
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var controllerName = values[_controllerParameterName]?.ToString();

            IList<KeyValuePair<HttpMethod, string>> blockedActionNames;

            if (!_blockedControllerActions.TryGetValueByOrdinalKey(
                    controllerName,
                    out blockedActionNames))
            {
                return true;
            }

            var actionName = values[parameterName]?.ToString();
            var httpMethod = httpContext.Request.HttpMethod;

            if (!blockedActionNames.Any(ban =>
                    string.Equals(ban.Key.Method, httpMethod, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(ban.Value, actionName, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
