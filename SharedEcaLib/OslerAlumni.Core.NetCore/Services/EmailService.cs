using System;
using System.Collections.Generic;
using System.Linq;
using CMS.EmailEngine;
using CMS.MacroEngine;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using ECA.Core.Extensions;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Core.Services
{
    public class EmailService
        : ServiceBase, IEmailService
    {
        #region "Constants"

        public const string LoginUrlParameterName = "LoginUrl";
        public const string ResetPasswordUrlParameterName = "ResetPasswordUrl";
        public const string NewUserLoginUrlParameterName = "NewUserLoginUrl";

        public const string UserParameterName = "User";
        public const string UserNameParameterName = "UserName";
        public const string PasswordParameterName = "Password";
        public const string FirstNameParameterName = "FirstName";
        public const string LastNameParameterName = "LastName";

        public const string ContactUsUrlParameterName = "ContactUsUrl";
        public const string ProfileAndPreferencesUrlParameterName = "ProfileAndPreferencesUrl";
        public const string EmailLogoParameterName = "EmailLogo";
        #endregion

        #region "Private fields"

        protected readonly IEmailRepository _emailRepository;
        protected readonly IEmailTemplateRepository _emailTemplateRepository;
        protected readonly IEventLogRepository _eventLogRepository;
        protected readonly IUserRepository _userRepository;
        protected readonly IPageUrlService _pageUrlService;
        private readonly IGlobalAssetService _globalAssetService;

        protected readonly EmailConfig _emailConfig;
        protected readonly ContextConfig _context;

        #endregion

        public EmailService(
            IEmailRepository emailRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IEventLogRepository eventLogRepository,
            IUserRepository userRepository,
            IPageUrlService pageUrlService,
            IGlobalAssetService globalAssetService,
            EmailConfig emailConfig,    
            ContextConfig context)
        {
            _emailRepository = emailRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _eventLogRepository = eventLogRepository;
            _userRepository = userRepository;
            _pageUrlService = pageUrlService;
            _globalAssetService = globalAssetService;

            _emailConfig = emailConfig;
            _context = context;
        }

        #region "Methods"

        public void SendNewAlumniUserAccountNotificationEmail(
            IOslerUserInfo user,
            string token)
        {
            if ((user?.UserInfo == null)
                || string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            var template = _emailConfig.NewUserAccountAlumniEmailTemplate;

            try
            {
                var parameters = GetLocalizedUrlParameters(
                    StandalonePageType.NewUserLoginPage,
                    NewUserLoginUrlParameterName,
                    new
                    {
                        userGuid = user.UserGUID,
                        token = token
                    });

                // Add the user name into the parameter collection
                parameters.Add(
                    UserNameParameterName,
                    user.UserName);

                SendTemplatedEmailToUser(
                    user,
                    template,
                    parameters);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SendPasswordResetEmail),
                    ex);
            }
        }

        public void SendPasswordResetEmail(
            Guid userGuid,
            string token)
        {
            var template = _emailConfig.PasswordResetEmailTemplate;

            try
            {
                var parameters = GetLocalizedUrlParameters(
                    StandalonePageType.ResetPassword,
                    ResetPasswordUrlParameterName,
                    new
                    {
                        userGuid = userGuid,
                        token = token
                    });

                SendTemplatedEmailToUser(userGuid, template, parameters);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SendPasswordResetEmail),
                    ex);
            }
        }

        public void SendPasswordResetConfirmationEmail(
            Guid userGuid)
        {
            var template = _emailConfig.PasswordResetConfirmationEmailTemplate;

            SendTemplatedEmailToUser(userGuid, template);
        }

        #endregion

        #region "Helper methods"

        /// <summary>
        /// Returns the URL of a specific standalone page (<see cref="StandalonePageType"/>,
        /// localized per site culture (English, French), with optional query string parameters appended,
        /// and returns the results in the form of appropriately named macro parameters,
        /// e.g.
        /// <code>
        /// {
        ///   { "ResetPasswordUrlenCA", "/en/account/reset-password" },
        ///   { "ResetPasswordUrlfrCA", "/fr/fr-account/fr-reset-password" }
        /// }
        /// </code>
        /// </summary>
        /// <param name="standalonePageType"></param>
        /// <param name="baseParameterName"></param>
        /// <param name="queryStringObj"></param>
        /// <returns></returns>
        protected Dictionary<string, object> GetLocalizedUrlParameters(
            StandalonePageType standalonePageType,
            string baseParameterName,
            object queryStringObj = null)
        {
            return _context.AllowedCultureCodes.Values
                .ToDictionary(
                    cultureName =>
                        $"{baseParameterName}_{cultureName.Replace("-", string.Empty)}",
                    cultureName =>
                    {
                        string url;

                        if (!_pageUrlService.TryGetPageMainUrl(
                                standalonePageType,
                                cultureName,
                                out url))
                        {
                            _eventLogRepository.LogError(
                                GetType(),
                                nameof(GetLocalizedUrlParameters),
                                $"Failed to obtain the main URL of the {standalonePageType.ToString()} standalone page.");

                            return null;
                        }

                        if (queryStringObj != null)
                        {
                            url = _pageUrlService.GetUrl(
                                url,
                                queryStringObj);
                        }

                        return (object)_pageUrlService
                            .ResolveToAbsolute(
                                url,
                                _context.Site);
                    });
        }

        /// <summary>
        /// Returns Email Logo Image urls for all cultures.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, object> GeEmailLogoUrlParameters()
        {
            return _context.AllowedCultureCodes.Values
                .ToDictionary(
                    cultureName =>
                        $"{EmailLogoParameterName}_{cultureName.Replace("-", string.Empty)}",
                    cultureName => (object) _pageUrlService.ResolveToAbsolute(
                        _globalAssetService.GetEmailLogoUrl(cultureName),
                        _context.Site));
        }


        protected void SendTemplatedEmail(
            string templateCode,
            Func<EmailMessage, EmailMessage> emailTemplateConfigOverrideAction = null,
            Dictionary<string, object> parameters = null)
        {
            try
            {
                var emailTemplateInfo =
                    _emailTemplateRepository.GetEmailTemplate(templateCode);

                var emailMessage = new EmailMessage
                {
                    EmailFormat = EmailFormatEnum.Both,
                    From = _emailConfig.SendEmailNotificationsFrom,
                    Subject = emailTemplateInfo.TemplateSubject,
                    ReplyTo = emailTemplateInfo.TemplateReplyTo,
                };

                if (emailTemplateConfigOverrideAction != null)
                {
                    emailMessage = emailTemplateConfigOverrideAction.Invoke(emailMessage);
                }

                var resolver = MacroResolver.GetInstance();

                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }

                //Add Generic Parameters that all templated emails require
                parameters.AddRange(
                    GetLocalizedUrlParameters(
                        StandalonePageType.ContactUs,
                        ContactUsUrlParameterName));

                parameters.AddRange(
                    GetLocalizedUrlParameters(
                        StandalonePageType.ProfileAndPreferences,
                        ProfileAndPreferencesUrlParameterName));

                parameters.AddRange(GeEmailLogoUrlParameters());

                foreach (var keyValuePair in parameters)
                {
                    resolver.SetNamedSourceData(keyValuePair.Key, keyValuePair.Value);
                }

                _emailRepository.SendEmail(
                    emailMessage,
                    emailTemplateInfo,
                    resolver);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SendTemplatedEmail),
                    ex);
            }
        }

        protected void SendTemplatedEmailToUser(
            Guid userGuid,
            string templateName,
            Dictionary<string, object> parameters = null)
        {
            try
            {
                var user = _userRepository.GetByGuid(userGuid);

                SendTemplatedEmailToUser(
                    user,
                    templateName,
                    parameters);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SendTemplatedEmailToUser),
                    ex);
            }
        }

        protected void SendTemplatedEmailToUser(
            IOslerUserInfo user,
            string templateName,
            Dictionary<string, object> parameters = null)
        {
            try
            {
                if (user?.UserInfo == null)
                {
                    return;
                }

                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }

                if (!parameters.ContainsKey(UserParameterName))
                {
                    parameters.Add(UserParameterName, user);
                }

                SendTemplatedEmail(
                    templateName,
                    email =>
                    {
                        email.Recipients = user.Email;

                        return email;
                    },
                    parameters);
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SendTemplatedEmailToUser),
                    ex);
            }
        }

        #endregion
    }
}

