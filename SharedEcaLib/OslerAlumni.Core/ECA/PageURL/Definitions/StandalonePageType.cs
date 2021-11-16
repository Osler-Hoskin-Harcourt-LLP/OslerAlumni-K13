using CMS.Helpers;
using ECA.Core.Definitions;

namespace ECA.PageURL.Definitions
{
    public enum StandalonePageType
    {
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.PageNotFoundPage)]
        PageNotFound,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.ServerErrorPage)]
        ServerError,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.HomePage)]
        Home,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.LoginPage)]
        Login,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.RequestPasswordResetPage)]
        RequestPasswordReset,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.ResetPasswordPage)]
        ResetPassword,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.ContactUsPage)]
        ContactUs,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.ProfileAndPreferencesPage)]
        ProfileAndPreferences,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.SearchPage)]
        Search,
        [EnumStringRepresentation(ECAGlobalConstants.Settings.StandalonePages.NewUserLoginPage)]
        NewUserLoginPage
    }
}
