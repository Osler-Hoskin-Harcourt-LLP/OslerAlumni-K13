using Newtonsoft.Json;

namespace ECA.Mvc.Recaptcha.Models
{
    public class RecaptchaPostBody
    {
        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("remoteip")]
        public string RemoteIp { get; set; }
    }
}
