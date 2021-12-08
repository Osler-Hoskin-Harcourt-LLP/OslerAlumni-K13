using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ECA.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// Useful for setting the current document properties to the viewbag.
    /// The properties can then be passed on to partial views / methods without the need to add it to the viewmodel
    /// This would keep some partial controls independant from the page they are on.
    /// 
    ///
    /// Usage:
    /// Decorate the controller action by:
    /// 
    /// [AddActionParametersToViewBag("page")]
    /// public ActionResult Index(TreeNode page)
    /// </summary>
    public class AddActionParametersToViewBagAttribute
        : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        #region "Private fields"

        private string[] _parameterNames;

        #endregion

        public AddActionParametersToViewBagAttribute(
            params string[] parameterNames)
        {
            _parameterNames = parameterNames;
        }

        public override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var controller = (Controller) filterContext.Controller;

            if (controller == null)
            {
                return;
            }

            var actionParameters = filterContext.ActionArguments;

            if ((_parameterNames == null) || (_parameterNames.Length < 1))
            {
                _parameterNames = actionParameters.Keys.ToArray();
            }

            foreach (var parameterName in _parameterNames)
            {
                if (!actionParameters.ContainsKey(parameterName))
                {
                    continue;
                }

                controller.ViewData[parameterName.ToPascalCase()] = 
                    actionParameters[parameterName];
            }
        }
    }
}
