using System.Globalization;
using Autofac;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Core.Tests.InfrastructureTests;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Infrastructure;
using Xunit;

using CultureHelper = ECA.Mvc.Tests.Helpers.CultureHelper;

namespace OslerAlumni.Mvc.Tests.InfrastructureTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.DI)]
    public class MvcBootstrapItemTests
        : BootstrapItemTestsBase
    {
        #region "Properties"

        public override string CurrentCulture
        {
            get
            {
                return CultureHelper.GetCurrentCulture();
            }
            set
            {
                CultureHelper.SetCurrentCulture(value);
            }
        }

        #endregion

        public MvcBootstrapItemTests()
            : base(true)
        { }

        #region "Repositories"

        [Fact]
        public void InjectsRepositoryClassesAsImplementationOfTheirInterfaces()
        {
            var container = GetBootstrappedContainer(
                new MvcBootstrapItem());

            var repository = container.Resolve<IMockRepository>();

            Assert.IsType<Mock1Repository>(repository);

            Assert.Equal(repository.GetName(), CorrectMockRepositoryName);
        }

        [Fact]
        public void ExcludesBaseRepositoryInterfaceFromInjection()
        {
            var container = GetBootstrappedContainer(
                new MvcBootstrapItem());

            var repository = container.ResolveOptional<IRepository>();

            Assert.Null(repository);
        }

        [Fact]
        public void InjectsCorrectCultureIntoRepository()
        {
            var container = GetBootstrappedContainer(
                new MvcBootstrapItem());

            // Assert default ui culture is injected in the repository
            var repository = container.ResolveOptional<IMockWithCultureRepository>();

            var currentUICultureName = CultureInfo.CurrentUICulture.Name;
            string cultureKey;

            if (GlobalConstants.Cultures.AllowedCultureCodes
                    .TryGetKeyByOrdinalValue(currentUICultureName, out cultureKey))
            {
                Assert.Equal(currentUICultureName, repository.CultureName);
            }
            else
            {
                Assert.Equal(GlobalConstants.Cultures.Default, repository.CultureName);
            }

            // Force Update English Culture
            CurrentCulture = TestConstants.Cultures.English;

            // Assert English Culture is injected in the repository
            repository = container.ResolveOptional<IMockWithCultureRepository>();
            Assert.Equal(TestConstants.Cultures.English, repository.CultureName);

            // Force Update French Culture
            CurrentCulture = TestConstants.Cultures.French;

            // Assert French Culture is injected in the repository
            repository = container.ResolveOptional<IMockWithCultureRepository>();
            Assert.Equal(TestConstants.Cultures.French, repository.CultureName);
        }

        [Fact]
        public void InjectsCorrectSiteIntoRepository()
        {
            CurrentSite = DefaultSite;

            var container = GetBootstrappedContainer(
                new MvcBootstrapItem());

            // Assert current site is injected in the repository
            var repository = container.ResolveOptional<IMockWithSiteRepository>();

            Assert.Equal(CurrentSite.SiteID, repository.SiteId);
            Assert.Equal(CurrentSite.SiteName, repository.SiteName);
        }

        #endregion
        
        #region "Services"

        [Fact]
        public void InjectsServiceClassesAsImplementationOfTheirInterfaces()
        {
            var container = GetBootstrappedContainer(
                new MvcBootstrapItem());

            var service = container.Resolve<IMockService>();

            Assert.IsType<Mock1Service>(service);

            Assert.Equal(service.GetName(), CorrectMockServiceName);
        }

        [Fact]
        public void ExcludesBaseServiceInterfaceFromInjection()
        {
            var container = GetBootstrappedContainer(
                new MvcBootstrapItem());

            var service = container.ResolveOptional<IService>();

            Assert.Null(service);
        }
        
        #endregion
    }
}
