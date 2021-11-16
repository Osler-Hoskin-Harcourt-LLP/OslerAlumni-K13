using System;
using System.Net.Http;
using System.Text;
using ECA.Core.Services;
using ECA.Mvc.Recaptcha.Models;
using Newtonsoft.Json.Linq;

namespace ECA.Mvc.Recaptcha.Services
{
    public class GoogleRecaptchaService
        : ServiceBase, IGoogleRecaptchaService
    {
        #region "Private fields"

        private static readonly object _threadLock = new object();

        private static HttpClient _httpClient;

        private readonly GoogleRecaptchaConfig _googleRecaptchaConfig;

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
                        }
                    }
                }

                return _httpClient;
            }
        }

        #endregion

        public GoogleRecaptchaService(
            GoogleRecaptchaConfig googleRecaptchaConfig)
        {
            if (googleRecaptchaConfig == null)
            {
                throw new ArgumentNullException(
                    nameof(GoogleRecaptchaConfig),
                    $"The {nameof(GoogleRecaptchaConfig)} object is not specified.");
            }

            _googleRecaptchaConfig = googleRecaptchaConfig;
        }

        #region "Methods"

        public RecaptchaResponseModel ValidateCaptchaResponse(
            RecaptchaPostBody recaptchaPostBody)
        {
            var uri = new Uri(
                $"{_googleRecaptchaConfig.ReCaptchaApiUrl}?secret={_googleRecaptchaConfig.ReCaptchaApiSecretKey}&response={recaptchaPostBody.Response}&remoteip={recaptchaPostBody.RemoteIp}");

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = null;

            httpResponse = HttpClient.PostAsync(uri, content)
                .GetAwaiter()
                .GetResult();

            // Throw an Exception if not successful. Let the Application Handle it.
            httpResponse.EnsureSuccessStatusCode(); 

            var contentResponse = httpResponse.Content
                .ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            return JObject
                .Parse(contentResponse)
                .ToObject<RecaptchaResponseModel>();
        }

        #endregion
    }
}
