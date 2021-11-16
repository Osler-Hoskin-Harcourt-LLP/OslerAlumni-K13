using System.Collections.Generic;
using System.Linq;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Admin.Tests;
using ECA.Core.Definitions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.PageURL.Services;
using Moq;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using Xunit;

namespace ECA.Admin.PageURL.Tests.ServiceTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.URL)]
    public class PageUrlServiceTests
        : AdminTestsBase
    {
        public PageUrlServiceTests()
            : base(true)
        {
            // Since the constructor will run for each test in the "Theory" separately,
            // but will maintain the context, need to check that we haven't already 
            // registered our mock service
            if (Service.IsRegistered(typeof(ISettingsService))
                && (Service.Resolve<ISettingsService>()?.GetType() == typeof(MockSettingsService)))
            {
                return;
            }

            Service.Use<ISettingsService, MockSettingsService>();
        }

        #region "Mock components"

        protected class MockSettingsService
            : ISettingsService
        {
            // These are the site settings that TreePathUtils.GetSafeUrlPath will reference internally.
            // Since the unit tests don't have a DB connection to read the site settings,
            // we need to provide the values here.
            private static readonly Dictionary<string, string>
                _settings = new Dictionary<string, string>
                {
                    { ECAGlobalConstants.Settings.AllowedUrlCharacters, null },
                    { ECAGlobalConstants.Settings.ForbiddenUrlCharactersReplacement, "-" },
                    { ECAGlobalConstants.Settings.ForbiddenUrlCharacters, "'’`‘\"”«»()–—,®™©" },
                    { ECAGlobalConstants.Settings.UrlMaxLength, "150" }
                };

            public string this[string keyName]
                => _settings[keyName.Split('.').Last()];

            public bool IsAvailable
                => true;
        }

        protected ISettingsKeyRepository GetMockSettingsKeyRepository()
        {
            var settingRepository = new Mock<ISettingsKeyRepository>();

            settingRepository
                .Setup(
                    dr => dr.GetValue<int>(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns<string, string, string>(
                    GetMockSettingIntValue);

            return settingRepository.Object;
        }

        protected int GetMockSettingIntValue(
            string keyName,
            string culture,
            string siteName)
        {
            return ValidationHelper.GetInteger(
                Service.Resolve<ISettingsService>()[keyName], 
                0);
        }

        #endregion

        #region "Facts"

        [Theory]
        [InlineData(
            "/", 
            "/")]
        [InlineData(
            "/Log In",
            "/log-in")]
        [InlineData(
            "/Spotlights/Hugh O'Reilly",
            "/spotlights/hugh-o-reilly")]
        [InlineData(
            "/Spotlights/Don McGowan",
            "/spotlights/don-mcgowan")]
        [InlineData(
            "/Directory/Al-Nawaz Nanji",
            "/directory/al-nawaz-nanji")]
        [InlineData(
            "/Directory/Geneviève Chabot",
            "/directory/genevieve-chabot")]
        [InlineData(
            "/Ressources/Gouvernance d’entreprise et des investisseurs/2018/D’entreprise en démarrage à entreprise en croissance : tirez des leçons des chefs de file",
            "/ressources/gouvernance-d-entreprise-et-des-investisseurs/2018/d-entreprise-en-demarrage-a-entreprise-en-croissance-tirez-des-lecons-des-chefs-de-file")]
        [InlineData(
            "/Length-Test/Gouvernance d’entreprise et des investisseurs/2018/D’entreprise en démarrage à entreprise en croissance : tirez des leçons des chefs de file de l’industrie qui sont passés par là",
            "/length-test/gouvernance-d-entreprise-et-des-investisseurs/2018/d-entreprise-en-demarrage-a-entreprise-en-croissance-tirez-des-lecons-des-chefs-de-fil")]
        public void GeneratesCorrectUrlPath(
            string namePath,
            string urlPath)
        {
            CurrentSite = DefaultSite;

            var urlService = new PageUrlService(
                null,
                null,
                null,
                GetMockSettingsKeyRepository(),
                null,
                new ContextConfig
                {
                    // TODO: [VI] Inject these
                    AllowedCultureCodes = GlobalConstants.Cultures.AllowedCultureCodes,
                    BasePageType = PageType_BasePageType.CLASS_NAME
                });

            var page = new Mock<TreeNode>();

            page
                .Setup(p => p.NodeSiteName)
                .Returns(CurrentSite.SiteName);

            page
                .Setup(p => p.NodeAliasPath) // TODO##
                .Returns(namePath);

            var generatedUrl =
                urlService.GetUrlPath(page.Object);

            Assert.Equal(urlPath, generatedUrl);
        }

        #endregion
    }
}
