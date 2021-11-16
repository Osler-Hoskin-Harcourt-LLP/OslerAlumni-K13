using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CMS.Activities.Loggers;
using ECA.Core.Extensions;
using ECA.Core.Infrastructure;

namespace OslerAlumni.Mvc.Core.Infrastructure
{
    public class AutofacHttpApplication
        : HttpApplication
    {
        #region "Constants"

        private static readonly string[] NAMESPACE_PREFIXES =
        {
            "ECA",
            "OslerAlumni",
            "CMSApp"
        };

        #endregion

        #region "Methods"

        protected virtual IDependencyResolver ConfigureDependencies(
            HttpConfiguration httpConfiguration)
        {
            var builder =
                new ContainerBuilder();

            // Get relevant assemblies
            var assemblies = AppDomain.CurrentDomain
                .GetCustomAssemblies(
                    NAMESPACE_PREFIXES);

            builder.RegisterSource(new CMSRegistrationSource());

            // Enable property injection in view pages
            builder.RegisterSource(
                new ViewRegistrationSource());

            // Register web abstraction classes
            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterType<MembershipActivityLogger>().As<IMembershipActivityLogger>().InstancePerRequest();

            // Go through all the bootstrapper items defined in the referenced assemblies,
            // in order to apply dependency configurations defined in them
            Bootstrapper.Bootstrap(
                builder,
                httpConfiguration,
                assemblies);

            var container =
                builder.Build();

            // Use Autofac for dependency resolving in Web API
            httpConfiguration.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);

            // Use Autofac for dependency resolving in MVC
            var mvcDIResolver =
                new AutofacDependencyResolver(container);

            DependencyResolver.SetResolver(
                mvcDIResolver);

            return mvcDIResolver;
        }

        #endregion
    }
}
