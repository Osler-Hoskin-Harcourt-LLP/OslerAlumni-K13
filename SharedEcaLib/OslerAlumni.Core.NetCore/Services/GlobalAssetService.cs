using System;
using System.Collections.Generic;
using CMS.Localization;
using CMS.SiteProvider;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Services
{
    /// <remarks>
    /// This class doesn't need to use caching. See remarks on <see cref="SettingsKeyRepository"/>.
    /// </remarks>
    public class GlobalAssetService
        : ServiceBase, IGlobalAssetService
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;
        private readonly ContextConfig _context;

        private readonly Dictionary<string, string> _websiteLogoUrls = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _websiteMobileLogoUrls = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _emailLogoUrls = new Dictionary<string, string>();

        private string _defaultImageUrl;

        #endregion

        #region "Properties"

        public string DefaultImageUrl
            => _defaultImageUrl ?? (_defaultImageUrl =
                   _settingsKeyRepository.GetValue<string>(GlobalConstants.Settings.Global.DefaultImage));

        #endregion

        public GlobalAssetService(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;

            _context = context;
        }

        #region "Methods"

        public string GetWebsiteLogoUrl(
            string cultureName = null)
        {
            try
            {
                cultureName =
                    cultureName.ReplaceIfEmpty(LocalizationContext.CurrentCulture.CultureCode);

                string logoUrl;

                if (!_websiteLogoUrls.TryGetValue(cultureName, out logoUrl)
                    || string.IsNullOrWhiteSpace(logoUrl))
                {
                    logoUrl =
                        _settingsKeyRepository.GetValue<string>(
                            GlobalConstants.Settings.Global.SiteHeaderLogo,
                            cultureName);

                    _websiteLogoUrls[cultureName] = logoUrl;
                }

                return logoUrl;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetWebsiteLogoUrl),
                    ex);

                return null;
            }
        }

        public string GetWebsiteMobileLogoUrl(
            string cultureName = null)
        {
            try
            {
                cultureName =
                    cultureName.ReplaceIfEmpty(SiteContext.CurrentSiteName);

                string logoUrl;

                if (!_websiteMobileLogoUrls.TryGetValue(cultureName, out logoUrl)
                    || string.IsNullOrWhiteSpace(logoUrl))
                {
                    logoUrl =
                        _settingsKeyRepository.GetValue<string>(
                            GlobalConstants.Settings.Global.SiteMobileLogo,
                            cultureName);

                    _websiteMobileLogoUrls[cultureName] = logoUrl;
                }

                return logoUrl;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetWebsiteMobileLogoUrl),
                    ex);

                return null;
            }
        }

        public string GetEmailLogoUrl(string cultureName = null)
        {
            try
            {
                cultureName =
                    cultureName.ReplaceIfEmpty(LocalizationContext.CurrentCulture.CultureCode);

                string logoUrl;

                if (!_emailLogoUrls.TryGetValue(cultureName, out logoUrl)
                    || string.IsNullOrWhiteSpace(logoUrl))
                {
                    logoUrl =
                        _settingsKeyRepository.GetValue<string>(
                            GlobalConstants.Settings.Global.EmailLogo,
                            cultureName);

                    _emailLogoUrls[cultureName] = logoUrl;
                }

                return logoUrl;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetWebsiteLogoUrl),
                    ex);

                return null;
            }
        }

        #endregion
    }
}
