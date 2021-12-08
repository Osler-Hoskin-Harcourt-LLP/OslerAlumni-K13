using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using ECA.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var controllerName = values[_controllerParameterName]?.ToString();

            IList<KeyValuePair<HttpMethod, string>> blockedActionNames;

            if (!_blockedControllerActions.TryGetValueByOrdinalKey(
                    controllerName,
                    out blockedActionNames))
            {
                return true;
            }

            var actionName = values[routeKey]?.ToString();
            var httpMethod = httpContext.Request.Method;

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
