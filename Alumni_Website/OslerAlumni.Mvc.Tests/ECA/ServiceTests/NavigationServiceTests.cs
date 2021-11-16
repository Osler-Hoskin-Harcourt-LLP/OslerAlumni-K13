using System;
using System.Collections.Generic;
using System.Linq;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.Helpers;
using ECA.Content.Repositories;
using ECA.Core.Definitions;
using ECA.Core.Repositories;
using ECA.Mvc.Navigation.Definitions;
using ECA.Mvc.Navigation.Kentico.Models;
using ECA.Mvc.Navigation.Models;
using ECA.Mvc.Navigation.Repositories;
using ECA.Mvc.Navigation.Services;
using ECA.PageURL.Services;
using Moq;
using OslerAlumni.Core.Repositories;
using Tests.DocumentEngine;
using Xunit;

namespace ECA.Mvc.Tests.ServiceTests
{
    [Trait(TestConstants.Traits.Category, TestConstants.Categories.Services)]
    public class NavigationServiceTests
        : MvcTestsBase
    {
        #region "Constants"

        protected const string MockUrlPrefix = "~/mock-url/";

        #endregion

        public NavigationServiceTests()
            : base(true)
        {
            // IMPORTANT: This ensures that Kentico will not try to connect to DB (e.g. to pull in class meta data)
            // Use this syntax, instead of Fake<PageType_NavigationItem>() for any class that represents a page type
            Fake().DocumentType<PageType_NavigationItem>(PageType_NavigationItem.CLASS_NAME);
        }

        #region "Mock components"

        protected IUserRepository GetMockUserRepository()
        {
            var userRepository = new Mock<IUserRepository>();

            userRepository
                .Setup(
                    dr => dr.IsSystemUser(It.IsAny<string>()))
                .Returns<bool>(a => false);

            return userRepository.Object;
        }

        protected IDocumentRepository GetMockDocumentRepository()
        {
            var docRepository = new Mock<IDocumentRepository>();

            docRepository
                .Setup(
                    dr => dr.GetDocuments(
                        It.IsAny<IEnumerable<Guid>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<IEnumerable<string>>(),
                        It.IsAny<bool>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns<IEnumerable<Guid>, string, bool, IEnumerable<string>, bool, string, string>(
                    GetMockDocuments);

            return docRepository.Object;
        }

        protected INavigationItemRepository GetMockNavigationItemRepository(
            Dictionary<NavigationType, IList<PageType_NavigationItem>> navItemDict)
        {
            var navItemRepository = new Mock<INavigationItemRepository>();

            navItemRepository
                .Setup(
                    nr => nr.GetNavigationItems(
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .Returns((
                    string path,
                    bool includeProtected,
                    string cultureName,
                    string siteName, bool loggedInViaOslerNetwork) =>
                {
                    IList<PageType_NavigationItem> navItems = null;

                    switch (path)
                    {
                        case ECAGlobalConstants.Settings.Navigation.PrimaryNavigationPath:
                            navItems = navItemDict[NavigationType.Primary];
                            break;
                        case ECAGlobalConstants.Settings.Navigation.SecondaryNavigationPath:
                            navItems = navItemDict[NavigationType.Secondary];
                            break;
                        case ECAGlobalConstants.Settings.Navigation.FooterNavigationPath:
                            navItems = navItemDict[NavigationType.Footer];
                            break;
                    }

                    if (string.IsNullOrWhiteSpace(cultureName))
                    {
                        cultureName = CurrentCulture;
                    }

                    return navItems?
                        .Where(item =>
                            string.Equals(
                                item.DocumentCulture,
                                cultureName,
                                StringComparison.OrdinalIgnoreCase));
                });

            return navItemRepository.Object;
        }

        protected ISettingsKeyRepository GetMockSettingsKeyRepository()
        {
            var settingRepository = new Mock<ISettingsKeyRepository>();

            settingRepository
                .Setup(
                    sr => sr.GetValue<string>(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns<string, string, string>(
                    GetMockSettingStringValue);

            return settingRepository.Object;
        }
        
        delegate bool TryGetUrlDelegate(TreeNode page, out string url);

        protected IPageUrlService GetMockPageUrlService()
        {

            var pageUrlService = new Mock<IPageUrlService>();

            TryGetUrlDelegate tryGetMockUrl =
                (TreeNode page, out string url) =>
                {
                    url = GetMockUrl(page.NodeGUID);

                    return true;
                };

            pageUrlService
                .Setup(
                    ps => ps.TryGetPageMainUrl(It.IsAny<TreeNode>(), out It.Ref<string>.IsAny))
                .Returns(tryGetMockUrl);

            return pageUrlService.Object;
        }

        protected Dictionary<NavigationType, IList<PageType_NavigationItem>> GetMockNavigationItems()
        {
            var baseItems = new List<PageType_NavigationItem>
            {
                new PageType_NavigationItem
                {
                    NavigationItemID = 0,
                    PageName = "Nav Item 1",
                    Title = "Nav Item with Page GUID",
                    ExternalURL = null
                },
                new PageType_NavigationItem
                {
                    NavigationItemID = 1,
                    PageName = "Nav Item 2",
                    Title = "Nav Item with External URL",
                    ExternalURL = "http://example1.com"
                },
                new PageType_NavigationItem
                {
                    NavigationItemID = 2,
                    PageName = "Nav Item 3",
                    Title = "Nav Item with both Page GUID and External URL",
                    ExternalURL = "http://example2.com"
                },
                new PageType_NavigationItem
                {
                    NavigationItemID = 3,
                    PageName = "Nav Item 4",
                    Title = "Nav Item with neither Page GUID nor External URL",
                    ExternalURL = null
                }
            };

            Func<PageType_NavigationItem, NavigationType, string, PageType_NavigationItem>
                convertToTypedNavigationItem =
                    (item, navType, cultureName) =>
                    {
                        var navTypeName = navType.ToString();

                        return new PageType_NavigationItem
                        {
                            DocumentCulture = cultureName,
                            PageName = $"{navTypeName} {item.PageName}",
                            Title = $"{navTypeName} {item.Title}",
                            // The second item in the list is supposed to have only external URL,
                            // and the fourth item in the list is supposed to have neither PageGUID nor external URL,
                            // which is why we are setting PageGUID to Guid.Empty
                            PageGUID = new[] { 1, 3 }.Contains(item.NavigationItemID)
                                ? Guid.Empty
                                : Guid.NewGuid(),
                            ExternalURL = item.ExternalURL
                                ?.Replace("http://", $"http://{navTypeName.ToLowerCSafe()}-")
                        };
                    };

            var navItems = new Dictionary<NavigationType, IList<PageType_NavigationItem>>();

            foreach (NavigationType navType in Enum.GetValues(typeof(NavigationType)))
            {
                navItems[navType] =
                    baseItems
                        .SelectMany(item =>
                            new[]
                            {
                                convertToTypedNavigationItem(
                                    item,
                                    navType,
                                    TestConstants.Cultures.English),
                                convertToTypedNavigationItem(
                                    item,
                                    navType,
                                    TestConstants.Cultures.French)
                            })
                        .ToList();
            }

            return navItems;
        }

        protected IEnumerable<TreeNode> GetMockDocuments(
            IEnumerable<Guid> nodeGuids,
            string pageTypeName,
            bool ignorePreviewMode,
            IEnumerable<string> columnNames,
            bool includeAllCoupledColumns,
            string cultureName,
            string siteName)
        {
            var nodeOrder = 1;

            return nodeGuids?
                .Select(nodeGuid =>
                {
                    var page = new Mock<TreeNode>();

                    page.Setup(n => n.NodeGUID).Returns(nodeGuid);

                    // Set NodeOrder so that we can verify if NavigationService is ordering nav items correctly
                    page.Setup(n => n.NodeOrder).Returns(nodeOrder);

                    ++nodeOrder;

                    return page.Object;
                });
        }

        protected string GetMockUrl(
            Guid pageGuid,
            bool resolve = false)
        {
            var url = $"{MockUrlPrefix}{pageGuid}";

            return resolve
                ? URLHelper.ResolveUrl(url)
                : url;
        }

        protected string GetMockSettingStringValue(
            string keyName,
            string culture,
            string siteName)
        {
            return keyName;
        }

        #endregion

        #region "Facts"

        [Fact]
        public void GeneratesPrimaryNavigation()
        {
            TestNavigation(
                NavigationType.Primary,
                TestConstants.Cultures.English);

            TestNavigation(
                NavigationType.Primary,
                TestConstants.Cultures.French);
        }

        [Fact]
        public void GeneratesSecondaryNavigation()
        {
            TestNavigation(
                NavigationType.Secondary,
                TestConstants.Cultures.English);

            TestNavigation(
                NavigationType.Secondary,
                TestConstants.Cultures.French);
        }

        [Fact]
        public void GeneratesFooterNavigation()
        {
            TestNavigation(
                NavigationType.Footer,
                TestConstants.Cultures.English);

            TestNavigation(
                NavigationType.Footer,
                TestConstants.Cultures.French);
        }


        #endregion

        #region "Methods"

        protected void TestNavigation(
            NavigationType navType,
            string cultureName)
        {
            var navItemDict = GetMockNavigationItems();

            CurrentCulture = cultureName;

            var navService = new NavigationService(
                GetMockDocumentRepository(),
                GetMockUserRepository(),
                GetMockNavigationItemRepository(navItemDict),
                GetMockSettingsKeyRepository(),
                GetMockCacheService<IEnumerable<NavigationItem>>(),
                GetMockPageUrlService(),
                GetMockContext());

            var generatedNavItems = navService.GetNavigation(navType)
                ?.ToList();

            // Make sure the navigation is not empty
            Assert.NotNull(generatedNavItems);
            Assert.NotEmpty(generatedNavItems);

            // Confirm that:
            // - correct items were included in the navigation based on the navigation type;
            // - correct items were included in the navigation based on their culture;
            // - nav items were generated in the correct order.
            var navItems = navItemDict[navType]
                .Where(item =>
                    string.Equals(
                        item.DocumentCulture,
                        cultureName,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.Equal(navItems.Count, generatedNavItems.Count);
            Assert.Equal(
                navItems.Select(ni => ni.Title),
                generatedNavItems.Select(ni => ni.Title));

            for (var i = 0; i < navItems.Count; i++)
            {
                var navItem = navItems[i];
                var generatedNavItem = generatedNavItems[i];

                // Make sure navigation doesn't contain empty items
                Assert.NotNull(generatedNavItem);

                // Confirm that:
                // - correct URL is being used;
                // - PageGUID is given priority over the ExternalURL.
                if (navItem.PageGUID != Guid.Empty)
                {
                    Assert.False(generatedNavItem.IsExternal);
                    Assert.Equal(GetMockUrl(navItem.PageGUID, true), generatedNavItem.Url);
                }
                else if (!string.IsNullOrWhiteSpace(navItem.ExternalURL))
                {
                    Assert.True(generatedNavItem.IsExternal);
                    Assert.Equal(navItem.ExternalURL, generatedNavItem.Url);
                }
                else
                {
                    Assert.Null(generatedNavItem.IsExternal);
                    Assert.Null(generatedNavItem.Url);
                }
            }
        }

        #endregion
    }
}
