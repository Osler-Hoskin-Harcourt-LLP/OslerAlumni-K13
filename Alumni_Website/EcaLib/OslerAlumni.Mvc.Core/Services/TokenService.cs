using System;
using System.Threading.Tasks;
using System.Web;
using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OslerAlumni.Core.Repositories;

using Constants = OslerAlumni.Mvc.Core.Definitions.Constants;

namespace OslerAlumni.Mvc.Core.Services
{
    public class TokenService
        : ServiceBase, ITokenService
    {
        #region "Private fields"

        private readonly IUserRepository _userRepository;

        #endregion

        #region "Properties"

        private IOwinContext OwinContext
            => new HttpContextWrapper(HttpContext.Current).GetOwinContext();

        private UserManager UserManager
            => OwinContext.Get<UserManager>();

        #endregion

        public TokenService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region "Methods"

        public Task<string> GeneratePasswordResetTokenAsync(
            Guid userGuid)
        {
            var user = _userRepository.GetByGuid(userGuid);

            if (user != null)
            {
                return UserManager.GeneratePasswordResetTokenAsync(user.UserID);
            }

            return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// Verifies if user's password reset token is valid.
        /// </summary>
        /// <param name="userGuid">User Guid.</param>
        /// <param name="token">Password reset token.</param>
        /// <returns>True if user's password reset token is valid, false when user was not found or token is invalid or has expired.</returns>
        public bool VerifyPasswordResetToken(
            Guid userGuid,
            string token)
        {
            try
            {
                var user = _userRepository.GetByGuid(userGuid);

                return user.Enabled && UserManager.VerifyUserToken(user.UserID, Constants.VerifyUserTokenPurpose, token);
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
