namespace ECA.Mvc.Recaptcha.Models
{
    public class GoogleRecaptchaConfig 
        : IGoogleRecaptchaConfig
    {
        public string ReCaptchaApiUrl { get; set; }

        public string ReCaptchaApiSecretKey { get; set; }

        public string ReCaptchaApiPublicKey { get; set; }

        public GoogleRecaptchaConfig(
            string reCaptchaApiUrl, 
            string reCaptchaApiSecretKey, 
            string reCaptchaApiPublicKey)
        {
            ReCaptchaApiUrl = reCaptchaApiUrl;
            ReCaptchaApiSecretKey = reCaptchaApiSecretKey;
            ReCaptchaApiPublicKey = reCaptchaApiPublicKey;   
        }
    }
}
