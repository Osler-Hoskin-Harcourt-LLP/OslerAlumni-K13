using CMS.Membership;
using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Services
{
    public class AuthorizationService
        : ServiceBase, IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        #region Properties


        #endregion

        public AuthorizationService(
            IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Checks where a logged in user is a competitor
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<bool> IsCurrentUserInRole(string roleName)
        {
            if (MembershipContext.AuthenticatedUser != null)
            {
                return await _userManager.IsInRoleAsync(new ApplicationUser(MembershipContext.AuthenticatedUser), roleName);
            }

            return false;
        }

        public async Task<bool> CurrentUserHasCompetitorRole()
        {
            return await IsCurrentUserInRole(GlobalConstants.Roles.CompetitorRole);
        }
    }
}
