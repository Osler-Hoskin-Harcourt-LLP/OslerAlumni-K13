
namespace ECA.Mvc.Recaptcha.Models
{
    public interface IGoogleRecaptchaConfig
    {
        string ReCaptchaApiPublicKey { get; set; }

        string ReCaptchaApiSecretKey { get; set; }

        string ReCaptchaApiUrl { get; set; }
    }
}
