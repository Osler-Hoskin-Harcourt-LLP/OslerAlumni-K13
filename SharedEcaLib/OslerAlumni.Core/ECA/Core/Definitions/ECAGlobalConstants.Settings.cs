namespace ECA.Core.Definitions
{
    public static partial class ECAGlobalConstants
    {
        public static class Settings
        {
            public const string DefaultSystemUser = "ECADefaultCMSUserGUID";

            public static class Navigation
            {
                public const string PrimaryNavigationPath = "ECAPrimaryNavigationPath";
                public const string SecondaryNavigationPath = "ECASecondaryNavigationPath";
                public const string FooterNavigationPath = "ECAFooterNavigationPath";
            }

            public const string AllowedUrlCharacters = "CMSAllowedURLCharacters";
            public const string ForbiddenUrlCharactersReplacement = "CMSForbiddenCharactersReplacement";
            public const string ForbiddenUrlCharacters = "CMSForbiddenURLCharacters";
            public const string RedirectAliasesToMainUrl = "CMSRedirectAliasesToMainURL";
            public const string UrlMaxLength = "ECAURLMaxLength";

            public static class StandalonePages
            {
                public const string PageNotFoundPage = "ECAPageNotFoundPage";
                public const string ServerErrorPage = "ECAServerErrorPage";
                public const string HomePage = "ECAHomePage";
                public const string LoginPage = "ECALoginPage";
                public const string RequestPasswordResetPage = "ECARequestPasswordResetPage";
                public const string ResetPasswordPage = "ECAResetPasswordPage";


                public const string ContactUsPage = "ECAContactUsPage";
                public const string ProfileAndPreferencesPage = "ECAProfileAndPreferencesPage";
                public const string SearchPage = "ECASearchPage";
                public const string NewUserLoginPage = "ECANewUserLoginPage";
            }

            public static class Caching
            {
                public const string CacheEnabled = "ECACacheEnabled";
                public const string CacheExpiryMinutes = "ECACacheExpiryMinutes";
            }
        }
    }
}
