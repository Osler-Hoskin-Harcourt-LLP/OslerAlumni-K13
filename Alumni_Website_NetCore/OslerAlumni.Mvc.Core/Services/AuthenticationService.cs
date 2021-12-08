
using CMS.Membership;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Services
{
    public class AuthenticationService 
        : ServiceBase, IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventLogRepository _eventLogRepository;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        #region Properties



        #endregion

        public AuthenticationService(
            IUserRepository userRepository,
            IEventLogRepository eventLogRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _eventLogRepository = eventLogRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public Task<SignInResult> LogInAsync(
            string userName,
            string password)
        {           
            return _signInManager.PasswordSignInAsync(userName, password, isPersistent: true, false);
        }

        public async Task<SignInResult> LogInAsUserAsync(
            string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                await _signInManager.SignInAsync(user, isPersistent: true);

                return SignInResult.Success;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(),nameof(LogInAsUserAsync), ex);

                return SignInResult.Failed;
            }
        }

        public void LogOut()
        {
            _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Reset user's password.
        /// </summary>
        /// <param name="userGuid">User Guid.</param>
        /// <param name="token">Password reset token.</param>
        /// <param name="password">New password.</param>
        public async Task<IdentityResult> ResetUserPassword(Guid userGuid, string token, string password)
        {
            try
            {
                var user = _userRepository.GetByGuid(userGuid);

                return await _userManager.ResetPasswordAsync(new ApplicationUser((UserInfo)user), token, password);
            }
            catch (InvalidOperationException)
            {
                // User with given userId was not found
                return IdentityResult.Failed(new IdentityError() { Description = "UserGuid not found." });
            }
        }

    }
}
