using ECA.Core.Models;

namespace OslerAlumni.Core.Models
{
    public class EmailConfig
        : IConfig
    {
        public string NewUserAccountAlumniEmailTemplate { get; set; }

        public string PasswordResetEmailTemplate { get; set; }
        
        public string PasswordResetConfirmationEmailTemplate { get; set; }
        
        public string SendEmailNotificationsFrom { get; set; }

        public string ContactUsNotificationExternalEmailTemplate { get; set; }

        public string ContactUsNotificationInternalEmailTemplate { get; set; }

        public string ContactUsNotificationInternalEmailSendTo { get; set; }
    }
}
