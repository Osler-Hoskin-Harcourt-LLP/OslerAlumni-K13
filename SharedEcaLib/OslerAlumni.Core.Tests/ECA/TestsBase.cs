using System.Configuration;
using System.Linq;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using CMS.Tests;
using ECA.Core.Definitions;
using ECA.Core.Models;

namespace ECA.Core.Tests
{
    public abstract class TestsBase
        : UnitTests
    {
        #region "Constants"

        public const string SiteIdKey = "DefaultSiteId";
        public const string SiteNameKey = "DefaultSiteName";

        #endregion

        #region "Properties"

        public virtual SiteInfo DefaultSite
            => new SiteInfo
            {
                SiteID = ValidationHelper.GetInteger(ConfigurationManager.AppSettings[SiteIdKey], 0),
                SiteName = ConfigurationManager.AppSettings[SiteNameKey]
            };

        public virtual SiteInfo CurrentSite
        {
            get
            {
                return SiteContext.CurrentSite;
            }
            set
            {
                SiteContext.CurrentSite = value;
            }
        }

        public abstract string CurrentCulture { get; set; }

        #endregion

        protected TestsBase()
            : this(false)
        { }

        protected TestsBase(
            bool initializeKentico)
        {
            if (initializeKentico)
            {
                UnitTestsSetUp();

                Fake()
                    .Info<SiteInfo>();

                // Define fake users here. Public user has to be defined,
                // so that MembershipContext.AuthenticatedUser can return it,
                // otherwise it will throw an error
                Fake()
                    .InfoProvider<UserInfo, UserInfoProvider>()
                    .WithData(
                        new UserInfo
                        {
                            UserID = 123,
                            UserName = "public",
                            UserNickName = "FakePublicUser"
                        });
            }
        }

        #region "Shared mock components"

        protected ContextConfig GetMockContext()
        {
            return new ContextConfig
            {
                CultureName = CurrentCulture,
                Site = CurrentSite,
                AllowedCultureCodes =
                    new[]
                    {
                        TestConstants.Cultures.English,
                        TestConstants.Cultures.French
                    }
                        .ToDictionary(
                            c => CultureHelper.GetShortCultureCode(c),
                            c => c)
            };
        }

        #endregion
    }
}
