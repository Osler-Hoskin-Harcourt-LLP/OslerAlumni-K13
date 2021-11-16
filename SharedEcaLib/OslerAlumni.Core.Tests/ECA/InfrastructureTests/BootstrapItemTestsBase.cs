using System.Reflection;
using Autofac;
using ECA.Core.Infrastructure;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;

namespace ECA.Core.Tests.InfrastructureTests
{
    public abstract class BootstrapItemTestsBase
        : TestsBase
    {
        protected BootstrapItemTestsBase()
            : this(false)
        { }

        protected BootstrapItemTestsBase(
            bool initializeKentico)
            : base(initializeKentico)
        { }

        #region "Repositories"

        #region "Constants"

        protected const string CorrectMockRepositoryName = "Mock1Repository";

        #endregion

        #region "Mock components"

        /* The order in which mock repositories are defined matters,
         * since if there are multiple implementations of the same interface,
         * Autofac picks the last one when resolving an interface
         */

        protected interface IMockRepository
        {
            string GetName();
        }

        protected interface IMockWithCultureRepository
        {
            string CultureName { get; set; }
        }

        protected interface IMockWithSiteRepository
        {
            int SiteId { get; set; }

            string SiteName { get; set; }
        }

        /// <summary>
        /// Correct repository implementation:
        /// - Implements IRepository interface
        /// - Name ends with "Repository"
        /// </summary>
        protected class Mock1Repository
            : IMockRepository, IRepository
        {
            public string GetName()
            {
                return CorrectMockRepositoryName;
            }
        }

        /// <summary>
        /// Incorrect repository implementation:
        /// - Does not implement IRepository interface
        /// </summary>
        protected class Mock2Repository
            : IMockRepository
        {
            public string GetName()
            {
                return "Mock2Repository";
            }
        }

        /// <summary>
        /// Incorrect repository implementation:
        /// - Name does not end with "Repository"
        /// </summary>
        protected class MockRepository3
            : IMockRepository, IRepository
        {
            public string GetName()
            {
                return "MockRepository3";
            }
        }

        protected class Mock4Repository
            : IMockWithCultureRepository, IRepository
        {
            public string CultureName { get; set; }

            public Mock4Repository(ContextConfig context)
            {
                CultureName = context.CultureName;
            }
        }

        protected class Mock5Repository
            : IMockWithSiteRepository, IRepository
        {
            public int SiteId { get; set; }

            public string SiteName { get; set; }

            public Mock5Repository(ContextConfig context)
            {
                SiteId = context.Site.SiteID;

                SiteName = context.Site.SiteName;
            }
        }

        #endregion

        #endregion

        #region "Services"

        #region "Constants"

        protected const string CorrectMockServiceName = "Mock1Service";

        #endregion

        #region "Mock components"

        /* The order in which mock services are defined matters,
         * since if there are multiple implementations of the same interface,
         * Autofac picks the last one when resolving an interface
         */

        protected interface IMockService
        {
            string GetName();
        }

        /// <summary>
        /// Correct service implementation:
        /// - Implements IService interface
        /// - Name ends with "Service"
        /// </summary>
        protected class Mock1Service
            : IMockService, IService
        {
            public string GetName()
            {
                return CorrectMockServiceName;
            }

            public void Dispose()
            { }
        }

        /// <summary>
        /// Incorrect service implementation:
        /// - Does not implement IService interface
        /// </summary>
        protected class Mock2Service
            : IMockService
        {
            public string GetName()
            {
                return "Mock2Service";
            }
        }

        /// <summary>
        /// Incorrect service implementation:
        /// - Name does not end with "Service"
        /// </summary>
        protected class MockService3
            : IMockService, IService
        {
            public string GetName()
            {
                return "MockService3";
            }

            public void Dispose()
            { }
        }

        #endregion

        #endregion

        #region "Methods"

        protected IContainer GetBootstrappedContainer(
            IBootstrapItem bootstrapper,
            Assembly[] assemblies = null)
        {
            var builder =
                new ContainerBuilder();

            if (assemblies == null)
            {
                assemblies = new[]
                {
                    Assembly.GetExecutingAssembly()
                };
            }

            bootstrapper.ConfigureDependencies(
                builder,
                null,
                assemblies);

            return builder.Build();
        }

        #endregion
    }
}
