using System.Web;
using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Services
{
    public class AuthorizationService
        : ServiceBase, IAuthorizationService
    {
        private readonly IUserRepository _userRepository;

        #region Properties

        private IOwinContext OwinContext
        {
            get { return new HttpContextWrapper(HttpContext.Current).GetOwinContext(); }
        }

        private UserManager UserManager
        {
            get { return OwinContext.Get<UserManager>(); }
        }

        #endregion

        public AuthorizationService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Checks where a logged in user is a competitor
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsCurrentUserInRole(string roleName)
        {
            if (_userRepository?.CurrentUser != null)
            {
                return UserManager.IsInRole(_userRepository.CurrentUser.UserID, roleName);
            }

            return false;
        }

        public bool CurrentUserHasCompetitorRole()
        {
            return IsCurrentUserInRole(GlobalConstants.Roles.CompetitorRole);
        }
    }
}
