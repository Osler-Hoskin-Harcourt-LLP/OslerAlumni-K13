using System;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.WebApi;
using CMS.DataEngine;
using ECA.Core.Extensions;
using ECA.Core.Infrastructure;

namespace OslerAlumni.Admin.Core.Infrastructure
{
    /// <remarks>
    /// This is based on the code orginally written by Philip Proplesch @ ecentricarts.
    /// </remarks>
    public class AutofacHttpApplication
        : CMSHttpApplication, IContainerProviderAccessor
    {
        private static readonly string[] NAMESPACE_PREFIXES =
        {
            "ECA",
            "OslerAlumni",
            "CMSApp"
        };

        // Provider that holds the application container.
        private static IContainerProvider _containerProvider;

        /// <summary>
        /// Instance property that will be used by Autofac HttpModules
        /// to resolve and inject dependencies.
        /// </summary>
        public IContainerProvider ContainerProvider
            => _containerProvider;

        protected void ConfigureDependencies(
            HttpConfiguration httpConfiguration)
        {
            var builder =
                new ContainerBuilder();

            // Get relevant assemblies
            var assemblies = AppDomain.CurrentDomain
                .GetCustomAssemblies(
                    NAMESPACE_PREFIXES);

            // Go through all the bootstrapper items defined in the referenced assemblies,
            // in order to apply dependency configurations defined in them
            Bootstrapper.Bootstrap(
                builder,
                httpConfiguration,
                assemblies);

            var container = builder.Build();

            // Use Autofac for dependency resolving
            httpConfiguration.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);

            // Plug in Autofac dependency container to the application
            _containerProvider =
                new ContainerProvider(container);
        }
    }
}
