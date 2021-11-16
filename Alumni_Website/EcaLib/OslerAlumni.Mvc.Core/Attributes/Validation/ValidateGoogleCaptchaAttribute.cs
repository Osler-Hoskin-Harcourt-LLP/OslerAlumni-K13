using System;
using System.Web.Mvc;
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

        }

        //This constructor is for Unit Testing
        public ValidateGoogleCaptchaAttribute(
            IEventLogRepository eventLogRepository,
            IGoogleRecaptchaService googleRecaptchaService)
        {
            _eventLogRepository = eventLogRepository;
            _googleRecaptchaService = googleRecaptchaService;
        }

        public override bool IsValid(object value)
        {
            IEventLogRepository eventLogRepository = _eventLogRepository ?? (IEventLogRepository)DependencyResolver.Current.GetService(typeof(IEventLogRepository));
            IGoogleRecaptchaService googleRecaptchaService = _googleRecaptchaService ?? (IGoogleRecaptchaService)DependencyResolver.Current.GetService(typeof(IGoogleRecaptchaService));

            string captchaResponse = value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(captchaResponse)) //This should never happen as long as required flag is also placed on the captcha field
            {
                return false;
            }

            //Only Validate if filled out.
            try
            {
                var result = googleRecaptchaService.ValidateCaptchaResponse(new RecaptchaPostBody()
                {
                    Response = captchaResponse,
                    RemoteIp = null, /*We can pass IP as well if we want.*/
                });

                return result.Success;
            }
            catch (Exception ex)
            {
                eventLogRepository.LogError(
                    GetType(),
                    nameof(ValidateGoogleCaptchaAttribute),
                    ex);

                return false;
            }
        }
    }
}
