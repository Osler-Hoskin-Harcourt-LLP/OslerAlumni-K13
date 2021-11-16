namespace OslerAlumni.Core.Definitions
{
    public static partial class GlobalConstants
    {
        public static class Settings
        {
            public static class Global
            {
                public const string SiteHeaderLogo = "OslerAlumniSiteHeaderLogo";
                public const string SiteMobileLogo = "OslerAlumniSiteMobileLogo";
                public const string DefaultImage = "OslerAlumniDefaultImage";
                public const string EmailLogo = "OslerAlumniEmailLogo";
            }

            public static class EmailSettings
            {
                public const string NewUserAccountAlumniEmailTemplate = "OslerAlumniNewUserAccountAlumniEmailTemplate";
                public const string PasswordResetEmailTemplate = "OslerAlumniPasswordResetEmailTemplate";
                public const string PasswordResetConfirmationEmailTemplate = "OslerAlumniPasswordResetConfirmationEmailTemplate";

                public const string SendEmailNotificationsFrom = "CMSSendEmailNotificationsFrom";

                public const string ContactUsNotificationExternalEmailTemplate = "OslerAlumniContactUsNotificationExternalEmailTemplate";
                public const string ContactUsNotificationInternalEmailTemplate = "OslerAlumniContactUsNotificationInternalEmailTemplate";
                public const string ContactUsNotificationInternalEmailSendTo = "OslerAlumniContactUsNotificationInternalEmailSendTo";
            }

            public const string AllowableUploadFileTypeExtensions = "OslerAlumniAllowableUploadFileTypeExtensions";

            public const string MaximumFileSizeInMb = "OslerAlumniMaximumFileSizeInMb";

            public const string FormFilesFolder = "CMSBizFormFilesFolder";

            public const string DirectoryLandingPageGuid = "OslerAlumniDirectoryLandingPageNodeGuid";
			
            public const string DefaultProfilesController = "OslerAlumniDefaultProfilesController";

            public const string DefaultProfilesAction = "OslerAlumniDefaultProfilesAction";

            public static class OnePlace
            {
                public const string Enabled = "OslerAlumniOPEnabled";
                public const string UpdateTimeMargin = "OslerAlumniOPUpdateTimeMargin";
                public const string Environment = "OslerAlumniOPEnvironment";
            }

            public static class Ctas
            {
                public const string DefaultCtasPath = "OslerAlumniDefaultCtasPath";
            }
            
        }
    }
}
