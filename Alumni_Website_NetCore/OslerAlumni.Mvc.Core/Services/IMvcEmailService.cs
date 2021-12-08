using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Core.Kentico.Models.Forms;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IMvcEmailService 
        : IEmailService
    {
        void SendContactUsNotificationExternalEmail(
            IContactUsFormItem formItem);

        void SendContactUsNotificationInternalEmail(
            IContactUsFormItem formItem);
    }
}
