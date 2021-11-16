using System.Collections.Generic;
using CMS.EmailEngine;
using CMS.Helpers;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.PageURL.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models.Forms;

namespace OslerAlumni.Mvc.Core.Services
{
    public class MvcEmailService
        : EmailService, IMvcEmailService
    {
        #region "Constants"

        public const string ContactUsIdParameterName = "ContactUsId";
        public const string CompanyNameParameterName = "CompanyName";
        public const string PhoneNumberParameterName = "PhoneNumber";
        public const string EmailParameterName = "Email";
        public const string SubjectParameterName = "Subject";
        public const string MessageParameterName = "Message";
        public const string AttachmentParameterName = "Attachment";
        public const string ReasonForContactingUsParameterName = "ReasonForContactingUs";
        public const string OpportunityTypeParameterName = "OpportunityType";

        #endregion

        #region "Private fields"

        private readonly IFileService _fileService;

        #endregion

        public MvcEmailService(
            IEmailRepository emailRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IEventLogRepository eventLogRepository,
            IUserRepository userRepository,
            IFileService fileService,
            IPageUrlService pageUrlService,
            IGlobalAssetService globalAssetService,
            EmailConfig emailConfig,
            ContextConfig context)
            : base(
                emailRepository,
                emailTemplateRepository,
                eventLogRepository,
                userRepository,
                pageUrlService,
                globalAssetService,
                emailConfig,
                context)
        {
            _fileService = fileService;
        }

        #region "Methods"

        public void SendContactUsNotificationExternalEmail(
            IContactUsFormItem formItem)
        {
            var template = _emailConfig.ContactUsNotificationExternalEmailTemplate;

            SendTemplatedEmail(
                template,
                email =>
                {
                    email.Recipients = formItem.Email;

                    return email;
                },
                new Dictionary<string, object>
                {
                    { FirstNameParameterName, formItem.FirstName},
                    { LastNameParameterName, formItem.LastName},
                    { ReasonForContactingUsParameterName, formItem.ReasonForContactingUsDisplay}
                });
        }

        public void SendContactUsNotificationInternalEmail(
            IContactUsFormItem formItem)
        {
            var template = _emailConfig.ContactUsNotificationInternalEmailTemplate;
            var toAddress = _emailConfig.ContactUsNotificationInternalEmailSendTo;

            SendTemplatedEmail(
                template,
                email =>
                {
                    email.Recipients = toAddress;

                    var subject = !string.IsNullOrWhiteSpace(formItem.Subject)
                        ? formItem.Subject
                        : ResHelper.GetString(
                            Constants.ResourceStrings.EmailTemplates.ContactUsNotificationInternalEmail.Subject,
                            GlobalConstants.Cultures.Default);

                    email.Subject = $"{formItem.ReasonForContactingUsDisplay} - {subject}";

                    if (!string.IsNullOrWhiteSpace(formItem.Attachment))
                    {
                        var filePath = _fileService.GetFullPathForFormAttachment(formItem.Attachment);

                        email.Attachments.Add(new EmailAttachment(filePath));
                    }

                    return email;
                },
                new Dictionary<string, object>
                {
                    { ContactUsIdParameterName, formItem.ContactUsId },
                    { FirstNameParameterName, formItem.FirstName },
                    { LastNameParameterName, formItem.LastName },
                    { CompanyNameParameterName, formItem.CompanyName },
                    { PhoneNumberParameterName, formItem.PhoneNumber },
                    { EmailParameterName, formItem.Email },
                    { SubjectParameterName, formItem.Subject },
                    { MessageParameterName, formItem.Message },
                    { AttachmentParameterName, formItem.Attachment },
                    { ReasonForContactingUsParameterName, formItem.ReasonForContactingUsDisplay },
                    { OpportunityTypeParameterName, formItem.OpportunityTypeDisplay }
                });
        }

        #endregion
    }
}

