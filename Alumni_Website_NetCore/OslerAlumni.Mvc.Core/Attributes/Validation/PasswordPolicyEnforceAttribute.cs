using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class PasswordPolicyEnforceAttribute 
        : KenticoValidateAttribute
    {
        private readonly IPasswordPolicyService _passwordPolicyService;

        public PasswordPolicyEnforceAttribute(IPasswordPolicyService passwordPolicyService)
        {
            _passwordPolicyService = passwordPolicyService;
        }

        public override bool IsValid(object value)
        {

            string password = value?.ToString() ?? string.Empty;

            var identityResult = _passwordPolicyService.PasswordValidator.ValidateAsync(password).GetAwaiter()
                .GetResult();

            return identityResult.Succeeded;
        }
    }
}
