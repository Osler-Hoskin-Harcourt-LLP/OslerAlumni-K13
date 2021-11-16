using Autofac;
using CMS.Localization;
using ECA.Core.Definitions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Core.Tests.InfrastructureTests;
using OslerAlumni.Admin.Core.Infrastructure;
using Xunit;

namespace OslerAlumni.Admin.Tests.InfrastructureTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.DI)]
    public class AdminBootstrapItemTests
        : BootstrapItemTestsBase
    {
        #region "Properties"

        public override string CurrentCulture
        {
            get
            {
                return LocalizationContext.PreferredCultureCode;
            }
            set
            {
                LocalizationContext.PreferredCultureCode = value;
            }
        }

        #endregion

        public AdminBootstrapItemTests()
            : base(true)
        { }

        #region "Repositories"

        #region "Facts"

        [Fact]
        public void InjectsRepositoryClassesAsImplementationOfTheirInterfaces()
        {
            var container = GetBootstrappedContainer(
                new AdminBootstrapItem());

            var repository = container.Resolve<IMockRepository>();

            Assert.IsType<Mock1Repository>(repository);

            Assert.Equal(repository.GetName(), CorrectMockRepositoryName);
        }

        [Fact]
        public void ExcludesBaseRepositoryInterfaceFromInjection()
        {
            var container = GetBootstrappedContainer(
                new AdminBootstrapItem());

            var repository = container.ResolveOptional<IRepository>();

            Assert.Null(repository);
        }

        [Fact]
        public void InjectsCorrectSiteIntoRepository()
        {
            CurrentSite = DefaultSite;

            var container = GetBootstrappedContainer(
                new AdminBootstrapItem());

            // Assert default ui culture is injected in the repository
            var repository = container.ResolveOptional<IMockWithSiteRepository>();

            Assert.Equal(CurrentSite.SiteID, repository.SiteId);
            Assert.Equal(CurrentSite.SiteName, repository.SiteName);
        }

        #endregion

        #endregion

        #region "Services"
        
        #region "Facts"

        [Fact]
        public void InjectsServiceClassesAsImplementationOfTheirInterfaces()
        {
            var container = GetBootstrappedContainer(
                new AdminBootstrapItem());

            var service = container.Resolve<IMockService>();

            Assert.IsType<Mock1Service>(service);

            Assert.Equal(service.GetName(), CorrectMockServiceName);
        }

        [Fact]
        public void ExcludesBaseServiceInterfaceFromInjection()
        {
            var container = GetBootstrappedContainer(
                new AdminBootstrapItem());

            var service = container.ResolveOptional<IService>();

            Assert.Null(service);
        }

        #endregion

        #endregion
    }
}
