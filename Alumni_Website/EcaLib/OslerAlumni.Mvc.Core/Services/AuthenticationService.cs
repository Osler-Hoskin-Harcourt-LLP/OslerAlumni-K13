using System;
using System.Threading.Tasks;
using System.Web;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Services
{
    public class AuthenticationService 
        : ServiceBase, IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventLogRepository _eventLogRepository;

        #region Properties

        private IOwinContext OwinContext
        {
            get { return new HttpContextWrapper(HttpContext.Current).GetOwinContext(); }
        }

        // <summary>
        /// Provides access to the Kentico.Membership.SignInManager instance.
        /// </summary>
        private SignInManager SignInManager
        {
            get { return OwinContext.Get<SignInManager>(); }
        }

        private UserManager UserManager
        {
            get { return OwinContext.Get<UserManager>(); }
        }

        /// <summary>
        /// Provides access to the Microsoft.Owin.Security.IAuthenticationManager instance.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get { return OwinContext.Authentication; }
        }

        #endregion

        public AuthenticationService(
            IUserRepository userRepository, IEventLogRepository eventLogRepository)
        {
            _userRepository = userRepository;
            _eventLogRepository = eventLogRepository;
        }

        public Task<SignInStatus> LogInAsync(
            string userName,
            string password)
        {           
            return SignInManager.PasswordSignInAsync(userName, password, isPersistent: true, shouldLockout: false);
        }

        public async Task<SignInStatus> LogInAsUserAsync(
            string userName)
        {
            try
            {
                var user = await UserManager.FindByNameAsync(userName);

                await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);

                return SignInStatus.Success;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(),nameof(LogInAsUserAsync), ex);

                return SignInStatus.Failure;
            }
        }

        public void LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        /// <summary>
        /// Reset user's password.
        /// </summary>
        /// <param name="userGuid">User Guid.</param>
        /// <param name="token">Password reset token.</param>
        /// <param name="password">New password.</param>
        public IdentityResult ResetUserPassword(Guid userGuid, string token, string password)
        {
            try
            {
                var user = _userRepository.GetByGuid(userGuid);

                return UserManager.ResetPassword(user.UserID, token, password);
            }
            catch (InvalidOperationException)
            {
                // User with given userId was not found
                return IdentityResult.Failed("UserGuid not found.");
            }
        }
    }
}
