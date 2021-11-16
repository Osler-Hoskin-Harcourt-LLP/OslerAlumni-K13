using System;
using System.Threading.Tasks;
using ECA.Core.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IAuthenticationService
         : IService
    {
        Task<SignInStatus> LogInAsUserAsync(
            string userName);

        Task<SignInStatus> LogInAsync(
            string userName, 
            string password);

        void LogOut();

        IdentityResult ResetUserPassword(
            Guid userGuid, 
            string token,
            string password);
    }
}
