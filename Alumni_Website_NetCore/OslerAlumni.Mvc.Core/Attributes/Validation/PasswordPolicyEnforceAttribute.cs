using CMS.Membership;
using Kentico.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public class PasswordPolicyEnforceAttribute 
        : KenticoValidateAttribute
    {

        public PasswordPolicyEnforceAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            var _passwordPolicyService = CMS.Core.Service.Resolve<IPasswordPolicyService>();
            var _userManager = CMS.Core.Service.Resolve<UserManager<ApplicationUser>>();
            string password = value?.ToString() ?? string.Empty;

            var identityResult = _passwordPolicyService.PasswordValidator.ValidateAsync(_userManager, new ApplicationUser(MembershipContext.AuthenticatedUser), password).GetAwaiter()
                .GetResult();

            return identityResult.Succeeded;
        }
    }
}
