using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using CMS.Membership;
using CMS.SiteProvider;
using ECA.Core.Extensions;
using ECA.Core.Infrastructure;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Mvc.Recaptcha.Models;
using ECA.Mvc.Recaptcha.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Core.Models;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Extensions.OslerControls.RichTextField.Models.Helpers;
using OslerAlumni.OnePlace.Factories;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;

using CultureInfo = System.Globalization.CultureInfo;

namespace OslerAlumni.Mvc.Infrastructure
{
    public class MvcBootstrapItem
        : IBootstrapItem
    {
        #region "Methods"

        public void ConfigureDependencies(
            ContainerBuilder builder,
            HttpConfiguration httpConfiguration = null,
            params Assembly[] assemblies)
        {
            // Find all classes that implement IService, IRepository interfaces
            // and register them as implementations of their respective interfaces,
            // so that they are picked up by the DI framework
            // and injected wherever those interfaces are used
            RegisterRepositories(
                builder,
                assemblies);

            RegisterServices(
                builder,
                assemblies);

            RegisterFactories(
                builder,
                assemblies);

            RegisterHelpers(builder,
                assemblies);

            // Register controllers for DI
            builder
                .RegisterControllers(assemblies)
                // Note that we have to use anonymous functions/context,
                // so that context values are evaluated at the time of injection
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(ContextConfig),
                    (parameter, context) =>
                        GetCurrentContext());

            // Register action filters for property injection
            builder
                .RegisterFilterProvider();
        }

        public static ContextConfig GetCurrentContext()
        {
            var allowedCultureCodes = GlobalConstants.Cultures.AllowedCultureCodes;
            
            return new ContextConfig
            {
                CultureName = GetCurrentCulture(allowedCultureCodes),
                User = GetCurrentUser(),
                Site = GetCurrentSite(),
                IsPreviewMode =  IsPreviewMode(),
                AllowedCultureCodes = allowedCultureCodes,
                BasePageType = PageType_BasePageType.CLASS_NAME
            };
        }

        #endregion

        #region "Helper methods"

        private static string GetCurrentCulture(
            Dictionary<string, string> allowedCultureCodes)
        {
            var cultureName = CultureInfo.CurrentUICulture.Name;

            string cultureKey;

            if (!allowedCultureCodes.TryGetKeyByOrdinalValue(cultureName, out cultureKey))
            {
                cultureName = GlobalConstants.Cultures.Default;
            }

            return cultureName;
        }

        private static SiteInfo GetCurrentSite()
        {
            SiteInfo currentSite = null;

            // Kentico tries to access the request context when determining the current site.
            // There doesn't seem to be a more elegant way of checking if the request context has been defined,
            // which is why we are catching the "Request is not available in this context" error instead.
            try
            {
                currentSite = SiteContext.CurrentSite;
            }
            catch (Exception)
            {
                // Do nothing
            }

            return currentSite ?? new SiteInfo
            {
                // Imitate the site
                SiteName = GlobalConstants.SiteCodeName
            };
        }

        private static UserInfo GetCurrentUser()
        {
            UserInfo currentUser = null;

            // Kentico tries to access the request context when determining the current user.
            // There doesn't seem to be a more elegant way of checking if the request context has been defined,
            // which is why we are catching the "Request is not available in this context" error instead.
            try
            {
                currentUser = MembershipContext.AuthenticatedUser;

                //Kentico Returns Public User by default so make sure it doesn't get returned.
                currentUser = currentUser.IsPublic() ? null : currentUser;
            }
            catch (Exception)
            {
                // Do nothing
            }

            return currentUser;
        }

        private static bool IsPreviewMode()
        {
            var previewFeature = HttpContext.Current?.Kentico()?
                .GetFeature<IPreviewFeature>();

            return previewFeature?.Enabled ?? false;
        }

        private void RegisterRepositories(
            ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            builder
                .RegisterImplementations<IRepository>(
                    assemblies,
                    "Repository")
                // Note that we have to use anonymous functions/context,
                // so that context values are evaluated at the time of injection
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(ContextConfig),
                    (parameter, context) =>
                        GetCurrentContext());
        }

        private void RegisterServices(
            ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            builder
                .RegisterImplementations<IService>(
                    assemblies,
                    "Service",
                    new[]
                    {
                        typeof(IEmailService),
                        typeof(IMvcEmailService),
                        typeof(IOnePlaceDataService),
                        typeof(IPasswordPolicyService),
                        typeof(IIpLocatorService)
                    })
                // Note that we have to use anonymous functions/context,
                // so that context values are evaluated at the time of injection
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(ContextConfig),
                    (parameter, context) =>
                        GetCurrentContext());

            builder
                .RegisterType<MvcEmailService>()
                .As<IEmailService>()
                .As<IMvcEmailService>()
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(EmailConfig),
                    (parameter, context) =>
                        context.Resolve<IConfigurationService>().GetConfig<EmailConfig>())
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(ContextConfig),
                    (parameter, context) =>
                        GetCurrentContext());

            builder
                .RegisterType<OnePlaceDataService>()
                .As<IOnePlaceDataService>()
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(OnePlaceConfig),
                    (parameter, context) =>
                        context.Resolve<IConfigurationService>().GetConfig<OnePlaceConfig>())
                .InstancePerLifetimeScope();

            builder
                .RegisterType<PasswordPolicyService>()
                .As<IPasswordPolicyService>()
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(PasswordPolicyConfig),
                    (parameter, context) =>
                        context.Resolve<IConfigurationService>().GetConfig<PasswordPolicyConfig>());

            // TODO: [VI] Revise this
            #region "Recaptcha"

            // Registering this config a different way. Whatever approach we think is the best.
            builder.Register(
                b =>
                {
                    var cofigurationService = b.Resolve<IConfigurationService>();

                    var reCaptchaApiUrl =
                        cofigurationService.GetWebConfigSetting<string>(
                            nameof(GoogleRecaptchaConfig.ReCaptchaApiUrl),
                            nameof(GoogleRecaptchaConfig));
                    var reCaptchaApiSecretKey =
                        cofigurationService.GetWebConfigSetting<string>(
                            nameof(GoogleRecaptchaConfig.ReCaptchaApiSecretKey),
                            nameof(GoogleRecaptchaConfig));
                    var reCaptchaApiPublicKey =
                        cofigurationService.GetWebConfigSetting<string>(
                            nameof(GoogleRecaptchaConfig.ReCaptchaApiPublicKey),
                            nameof(GoogleRecaptchaConfig));

                    return new GoogleRecaptchaConfig(
                        reCaptchaApiUrl,
                        reCaptchaApiSecretKey,
                        reCaptchaApiPublicKey);
                });

            // Registering this in a different way to reduce coupling to our project.
            builder
                .RegisterType<GoogleRecaptchaService>()
                .As<IGoogleRecaptchaService>();


            builder
                .RegisterType<IpLocatorService>()
                .As<IIpLocatorService>()
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(OslerNetworkConfig),
                    (parameter, context) =>
                        context.Resolve<IConfigurationService>().GetConfig<OslerNetworkConfig>());
            #endregion
        }

        private void RegisterFactories(
            ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            builder
                .RegisterType<DataSubmissionServiceFactory>()
                .As<IDataSubmissionServiceFactory>();
        }

        private void RegisterHelpers(
            ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            builder.RegisterType<InlineWidgetResolver>();

        }

        #endregion
    }
}
