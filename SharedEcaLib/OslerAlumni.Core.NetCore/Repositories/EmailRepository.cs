using System;
using CMS.EmailEngine;
using CMS.MacroEngine;
using CMS.SiteProvider;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;

namespace OslerAlumni.Core.Repositories
{
    public class EmailRepository
        : IEmailRepository
    {
        #region "Private fields"

        protected readonly IEventLogRepository _eventLogRepository;

        private readonly ContextConfig _context;

        #endregion

        public EmailRepository(
            IEventLogRepository eventLogRepository,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;

            _context = context;
        }

        #region "Methods"

        public void SendEmail(
            EmailMessage email,
            EmailTemplateInfo template,
            MacroResolver resolver,
            string siteName = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email.Recipients))
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(SendEmail),
                        $"Trying to send an email to an empty list of recipients: \r\n\r\nFrom: {email.From} \r\nSubject: {resolver.ResolveMacros(email.Subject)} \r\n\r\nMessage (HTML): {resolver.ResolveMacros(template.TemplateText)} \r\n----- \r\nMessage (Plain text): {resolver.ResolveMacros(template.TemplatePlainText)}");

                    return;
                }

                EmailSender.SendEmailWithTemplateText(
                    siteName.ReplaceIfEmpty(SiteContext.CurrentSiteName),
                    email,
                    template,
                    resolver,
                    // If send immeditaley is true, e-mail queue is not used
                    sendImmediately: false);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SendEmail),
                    ex);
            }
        }

        #endregion
    }
}
