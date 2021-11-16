using CMS.EmailEngine;
using CMS.MacroEngine;
using ECA.Core.Repositories;

namespace OslerAlumni.Core.Repositories
{
    public interface IEmailRepository
        : IRepository
    {
        void SendEmail(
            EmailMessage email,
            EmailTemplateInfo template,
            MacroResolver resolver,
            string siteName = null);
    }
}
