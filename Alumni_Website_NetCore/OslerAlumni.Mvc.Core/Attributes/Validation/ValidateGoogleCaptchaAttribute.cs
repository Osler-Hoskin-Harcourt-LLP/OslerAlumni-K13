
using ECA.Core.Repositories;
using ECA.Mvc.Recaptcha.Models;
using ECA.Mvc.Recaptcha.Services;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class ValidateGoogleCaptchaAttribute 
        : KenticoValidateAttribute
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IGoogleRecaptchaService _googleRecaptchaService;

        public ValidateGoogleCaptchaAttribute()
        {
            _eventLogRepository = CMS.Core.Service.Resolve<IEventLogRepository>();
            _googleRecaptchaService = CMS.Core.Service.Resolve<IGoogleRecaptchaService>();
        }

        public override bool IsValid(object value)
        {

            string captchaResponse = value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(captchaResponse)) //This should never happen as long as required flag is also placed on the captcha field
            {
                return false;
            }

            //Only Validate if filled out.
            try
            {
                var result = _googleRecaptchaService.ValidateCaptchaResponse(new RecaptchaPostBody()
                {
                    Response = captchaResponse,
                    RemoteIp = null, /*We can pass IP as well if we want.*/
                });

                return result.Success;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(ValidateGoogleCaptchaAttribute),
                    ex);

                return false;
            }
        }
    }
}
