using ECA.Core.Services;
using Microsoft.AspNet.Identity;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public class PasswordPolicyService
        : ServiceBase, IPasswordPolicyService
    {
        #region "Private fields"

        private static readonly object _threadLock = new object();

        private static PasswordValidator _passwordValidator;

        private readonly PasswordPolicyConfig _passwordPolicyConfig;

        #endregion

        #region "Properties"

        public PasswordValidator PasswordValidator
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
                            _passwordValidator = new PasswordValidator
                            {
                                RequiredLength = _passwordPolicyConfig.RequiredLength,
                                RequireUppercase = _passwordPolicyConfig.RequireUppercase,
                                RequireLowercase = _passwordPolicyConfig.RequireLowercase,
                                RequireNonLetterOrDigit = _passwordPolicyConfig.RequireNonLetterOrDigit,
                                RequireDigit = _passwordPolicyConfig.RequireDigit
                            };
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
