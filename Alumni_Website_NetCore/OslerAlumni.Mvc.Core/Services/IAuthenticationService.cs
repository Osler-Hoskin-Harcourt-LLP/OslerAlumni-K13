using System;
using ECA.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IAuthenticationService
         : IService
    {
        Task<SignInResult> LogInAsUserAsync(
            string userName);

        Task<SignInResult> LogInAsync(
            string userName, 
            string password);

        void LogOut();

        Task<IdentityResult> ResetUserPassword(
            Guid userGuid, 
            string token,
            string password);
    }
}
