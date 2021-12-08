using ECA.Mvc.Recaptcha.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Core.Controllers;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// Useful for setting certain global properties in the ViewBag
    /// to be available for all views.
    /// TODO: [VI] Revise this
    /// </summary>
    public class InitializeViewBagAttribute 
        : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var configurationService = CMS.Core.Service.Resolve<IConfigurationService>();
            var reCaptchaApiPublicKey = configurationService.GetWebConfigSetting<string>(
                nameof(GoogleRecaptchaConfig.ReCaptchaApiPublicKey),
                nameof(GoogleRecaptchaConfig));

            var gtmCodeKey = configurationService.GetWebConfigSetting<string>(
                nameof(GlobalConstants.Config.GTMCode));

            ((Controller)context.Controller).ViewBag.GoogleReCaptchaApiPublicKey =
                reCaptchaApiPublicKey;

            ((Controller)context.Controller).ViewBag.GTMCode =
                gtmCodeKey;
        }
    }
}
