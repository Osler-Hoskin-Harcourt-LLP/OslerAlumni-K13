using System;
using Newtonsoft.Json;

namespace ECA.Mvc.Recaptcha.Models
{
    public class RecaptchaResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        /// </summary>
        [JsonProperty("challenge_ts")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// the hostname of the site where the reCAPTCHA was solved
        /// </summary>
        [JsonProperty("hostname")]
        public string HostName { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}
