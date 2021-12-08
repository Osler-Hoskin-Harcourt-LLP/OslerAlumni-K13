using ECA.Mvc.Recaptcha.Models;

namespace ECA.Mvc.Recaptcha.Services
{
    /// <summary>
    /// Useful to Add Google Captcha to Forms.
    /// Ideally should be its own nuget package.
    /// See: https://developers.google.com/recaptcha/intro on how to generate an API KEY.
    /// The type of reCAPTCHA should be `reCAPTCHA V2`.
    /// </summary>
    public interface IGoogleRecaptchaService
    {
        RecaptchaResponseModel ValidateCaptchaResponse(
            RecaptchaPostBody recaptchaPostBody);
    }
}
