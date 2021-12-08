using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class PasswordPolicyService
        : ServiceBase, IPasswordPolicyService
    {
        #region "Private fields"

        private static readonly object _threadLock = new object();

        private static PasswordValidator<ApplicationUser> _passwordValidator;

        private readonly PasswordPolicyConfig _passwordPolicyConfig;

        #endregion

        #region "Properties"

        public PasswordValidator<ApplicationUser> PasswordValidator
        {
            get
            {
                if (_passwordValidator == null)
                {
                    // Thread-safe.
                    lock (_threadLock)
                    {
                        if (_passwordValidator == null)
                        {
                            _passwordValidator = new PasswordValidator<ApplicationUser>();
                        }
                    }
                }

                return _passwordValidator;
            }
        }

        #endregion

        public PasswordPolicyService(
            PasswordPolicyConfig passwordPolicyConfig)
        {
            _passwordPolicyConfig = passwordPolicyConfig;
        }
    }
}
