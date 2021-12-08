using CMS.EmailEngine;
using ECA.Core.Repositories;

namespace OslerAlumni.Core.Repositories
{
    public interface IEmailTemplateRepository
        : IRepository
    {
        EmailTemplateInfo GetEmailTemplate(
            string templateName,
            string siteName = null);
    }
}
