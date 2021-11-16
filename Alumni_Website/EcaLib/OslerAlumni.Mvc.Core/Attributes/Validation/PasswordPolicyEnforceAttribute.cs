using System.Web.Mvc;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class PasswordPolicyEnforceAttribute 
        : KenticoValidateAttribute
    {
        private readonly IPasswordPolicyService _passwordPolicyService;

        public PasswordPolicyEnforceAttribute()
        {

        }

        //This constructor is for Unit Testing
        public PasswordPolicyEnforceAttribute(IPasswordPolicyService passwordPolicyService)
        {
            _passwordPolicyService = passwordPolicyService;
        }

        public override bool IsValid(object value)
        {
            IPasswordPolicyService passwordPolicyService = _passwordPolicyService ?? (IPasswordPolicyService)DependencyResolver.Current.GetService(typeof(IPasswordPolicyService));

            string password = value?.ToString() ?? string.Empty;

            var identityResult = passwordPolicyService.PasswordValidator.ValidateAsync(password).GetAwaiter()
                .GetResult();

            return identityResult.Succeeded;
        }
    }
}
