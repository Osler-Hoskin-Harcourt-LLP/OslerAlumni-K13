using CMS.Base;
using CMS.Membership;
using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using OslerAlumni.Core.Repositories;

using Constants = OslerAlumni.Mvc.Core.Definitions.Constants;

namespace OslerAlumni.Mvc.Core.Services
{
    public class TokenService
        : ServiceBase, ITokenService
    {
        #region "Private fields"

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region "Properties"

        

        #endregion

        public TokenService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        #region "Methods"

        public Task<string> GeneratePasswordResetTokenAsync(
            Guid userGuid)
        {
            var user = _userRepository.GetByGuid(userGuid);

            if (user != null)
            {
                return _userManager.GeneratePasswordResetTokenAsync(new ApplicationUser(user.UserInfo));
            }

            return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Verifies if user's password reset token is valid.
        /// </summary>
        /// <param name="userGuid">User Guid.</param>
        /// <param name="token">Password reset token.</param>
        /// <returns>True if user's password reset token is valid, false when user was not found or token is invalid or has expired.</returns>
        public async Task<bool> VerifyPasswordResetToken(
            Guid userGuid,
            string token)
        {
            try
            {
                var user = _userRepository.GetByGuid(userGuid);

                return user.Enabled && await _userManager.VerifyUserTokenAsync(new ApplicationUser(user.UserInfo), _userManager.Options.Tokens.PasswordResetTokenProvider, Constants.VerifyUserTokenPurpose, token);
            }
            catch (Exception)
            {
                // User with given userGuid was not found
                return false;
            }
        }

        #endregion
    }
}
