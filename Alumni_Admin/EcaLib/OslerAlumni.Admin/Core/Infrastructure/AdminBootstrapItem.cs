using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using ECA.Core.Extensions;
using ECA.Core.Infrastructure;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Admin.Core.Repositories;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.OnePlace.Factories;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.Admin.Core.Infrastructure
{
    public class AdminBootstrapItem
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

            builder
                .RegisterType<DataSubmissionServiceFactory>()
                .As<IDataSubmissionServiceFactory>();
        }

        public static ContextConfig GetCurrentContext()
        {
            var allowedCultureCodes = GlobalConstants.Cultures.AllowedCultureCodes;

            return new ContextConfig
            {
                CultureName = GetCurrentCulture(allowedCultureCodes),
                User = GetCurrentUser(),
                Site = GetCurrentSite(),
                AllowedCultureCodes = allowedCultureCodes,
                BasePageType = PageType_BasePageType.CLASS_NAME
            };
        }

        #endregion

        #region "Helper methods"

        private static string GetCurrentCulture(
            Dictionary<string, string> allowedCultureCodes)
        {
            var cultureName = LocalizationContext.PreferredCultureCode;

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
            catch (HttpException)
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

        private void RegisterRepositories(
            ContainerBuilder builder,
            params Assembly[] assemblies)
        {
            builder
                .RegisterImplementations<IRepository>(
                    assemblies,
                    "Repository",
                    new[]
                    {
                        typeof(IAdminUserRepository),
                        typeof(IUserRepository)
                    })
                // Note that we have to use anonymous functions/context,
                // so that context values are evaluated at the time of injection
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(ContextConfig),
                    (parameter, context) =>
                        GetCurrentContext());

            // This is an overridden implementation of user repository that provides
            // user account functionality specific to the Admin site and its features
            builder
                .RegisterType<AdminUserRepository>()
                .As<IAdminUserRepository>()
                .As<IUserRepository>()
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
                        typeof(IOnePlaceDataService)
                    })
                // Note that we have to use anonymous functions/context,
                // so that context values are evaluated at the time of injection
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(ContextConfig),
                    (parameter, context) =>
                        GetCurrentContext());

            builder
                .RegisterType<EmailService>()
                .As<IEmailService>()
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

        }

        #endregion
    }
}
