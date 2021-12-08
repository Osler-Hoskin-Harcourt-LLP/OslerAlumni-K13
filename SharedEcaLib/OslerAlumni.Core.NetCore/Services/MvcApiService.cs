using OslerAlumni.Core.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Newtonsoft.Json;
using OslerAlumni.Core.Models;

namespace OslerAlumni.Core.Services
{
    /// <inheritdoc />
    public class MvcApiService : ServiceBase, IMvcApiService
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ContextConfig _context;

        #region "Private fields"

        private static IConfigurationService _configurationService;

        private static readonly object _threadLock = new object();

        private static HttpClient _httpClient;

        #endregion

        #region "Properties"

        /// <summary>
        /// An instance of <see cref="System.Net.Http.HttpClient"/>
        /// </summary>
        private static HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    lock (_threadLock)
                    {
                        if (_httpClient == null)
                        {
                            _httpClient = new HttpClient();

                            var byteArray = Encoding.ASCII.GetBytes(ApiKey);

                            _httpClient.DefaultRequestHeaders.Authorization =
                                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                                    Convert.ToBase64String(byteArray));
                        }
                    }
                }

                return _httpClient;
            }
        }

        private static string ApiKey =>
            _configurationService.GetWebConfigSetting<string>(GlobalConstants.Config.BasicAuthenticationAPIKey);

        #endregion


        public MvcApiService(
            IEventLogRepository eventLogRepository,
            IConfigurationService configurationService,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _configurationService = configurationService;
            _context = context;
        }

        public virtual async Task<string> GetPasswordResetTokenAsync(Guid userGuid)
        {
            string token = string.Empty;

            try
            {
                var responseBody = await GetAsync<BaseWebResponse<string>>(
                        $"{_context.Site.SitePresentationURL}/internalapi/getpasswordresettoken?userguid={userGuid}");

                if (responseBody.Status == WebResponseStatus.Success)
                {
                    token = responseBody.Result;
                }
                else
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(GetPasswordResetTokenAsync),
                        "Could not generate reset token.");
                }
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetPasswordResetTokenAsync),
                    ex);
            }

            return token;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var responseBody = await HttpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
