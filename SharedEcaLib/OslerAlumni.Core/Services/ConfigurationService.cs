using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using CMS.DataEngine;
using ECA.Caching.Models;
using ECA.Caching.Services;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Services
{
    public class ConfigurationService
        : ServiceBase, IConfigurationService
    {
        #region "Constants"

        public const string AppSettingsSectionName = "appSettings";

        #endregion

        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly ICacheService _cacheService;

        #endregion

        public ConfigurationService(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            ICacheService cacheService)
        {
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _cacheService = cacheService;
        }

        #region "Methods"

        public virtual NameValueCollection GetWebConfigSection(string sectionName)
        {
            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.Configuration.WebConfigSettingBySection,
                    sectionName),
                IsCultureSpecific = false,
                IsSiteSpecific = true
            };

            var result = _cacheService.Get(
                () => ConfigurationManager.GetSection(sectionName) as NameValueCollection, cacheParameters);

            return result;
        }

        public virtual object GetWebConfigSetting(
            Type type,
            string key,
            string sectionName = null)
        {
            sectionName = sectionName.ReplaceIfEmpty(AppSettingsSectionName);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.Configuration.WebConfigSettingBySectionAndName,
                    sectionName,
                    key),
                IsCultureSpecific = false,
                IsSiteSpecific = true
            };

            var result = _cacheService.Get(
                () =>
                {
                    try
                    {
                        var settingsCollection =
                            ConfigurationManager.GetSection(sectionName) as NameValueCollection;

                        var value = settingsCollection?[key];

                        // Note that there is no point in setting cache dependencies here,
                        // since if the web.config were to be updated,
                        // site would restart and its cache would be cleared anyways
                        return Convert.ChangeType(value, type);
                    }
                    catch (Exception ex)
                    {
                        _eventLogRepository.LogError(
                            GetType(),
                            nameof(GetWebConfigSetting),
                            ex);

                        return null;
                    }
                },
                cacheParameters);

            return result;
        }

        public virtual T GetWebConfigSetting<T>(
            string key,
            string sectionName = null)
        {
            return (T)(GetWebConfigSetting(typeof(T), key, sectionName) ?? default(T));
        }

        public virtual T GetConfig<T>()
            where T : class, IConfig, new()
        {
            var type = typeof(T);

            var cacheParameters = new CacheParameters
            {
                CacheKey = string.Format(
                    GlobalConstants.Caching.Configuration.ConfigByType,
                    type.FullName),
                IsCultureSpecific = false,
                IsSiteSpecific = true
            };

            var result = _cacheService.Get(
                cp =>
                {
                    if (type == typeof(EmailConfig))
                    {
                        return GetEmailConfig(cp) as T;
                    }

                    var config = new T();

                    var properties = type
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance );

                    foreach (var prop in properties)
                    {
                        if (prop.GetSetMethod() != null)
                        {
                            var value = GetWebConfigSetting(
                                prop.PropertyType,
                                prop.Name,
                                type.Name);

                            prop.SetValue(config, value);
                        }
                    }

                    // Note that there is no point in setting cache dependencies here,
                    // since if the web.config were to be updated,
                    // site would restart and its cache would be cleared anyways
                    return config;
                },
                cacheParameters);

            return result;
        }

        #endregion

        #region "Helper methods"

        protected EmailConfig GetEmailConfig(
            CacheParameters cacheParameters = null)
        {
            // Bust the cache (if used) whenever any of the email-related settings is modified
            cacheParameters?.CacheDependencies
                .AddRange(
                    new[]
                    {
                        GlobalConstants.Settings.EmailSettings.NewUserAccountAlumniEmailTemplate,
                        GlobalConstants.Settings.EmailSettings.PasswordResetEmailTemplate,
                        GlobalConstants.Settings.EmailSettings.PasswordResetConfirmationEmailTemplate,
                        GlobalConstants.Settings.EmailSettings.SendEmailNotificationsFrom,
                        GlobalConstants.Settings.EmailSettings.ContactUsNotificationExternalEmailTemplate,
                        GlobalConstants.Settings.EmailSettings.ContactUsNotificationInternalEmailTemplate,
                        GlobalConstants.Settings.EmailSettings.ContactUsNotificationInternalEmailSendTo
                    }
                        .Select(sn => $"{SettingsKeyInfo.OBJECT_TYPE}|byname|{sn}")
                        .ToList());

            Func<string, string> getSettingValue =
                key => _settingsKeyRepository.GetValue<string>(key);

            return new EmailConfig
            {
                NewUserAccountAlumniEmailTemplate =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.NewUserAccountAlumniEmailTemplate),
                PasswordResetEmailTemplate =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.PasswordResetEmailTemplate),
                PasswordResetConfirmationEmailTemplate =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.PasswordResetConfirmationEmailTemplate),
                SendEmailNotificationsFrom =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.SendEmailNotificationsFrom),
                ContactUsNotificationExternalEmailTemplate =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.ContactUsNotificationExternalEmailTemplate),
                ContactUsNotificationInternalEmailTemplate =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.ContactUsNotificationInternalEmailTemplate),
                ContactUsNotificationInternalEmailSendTo =
                    getSettingValue(GlobalConstants.Settings.EmailSettings.ContactUsNotificationInternalEmailSendTo)
            };
        }

        #endregion
    }
}
