using System;
using ECA.Core.Services;


namespace OslerAlumni.Mvc.Core.Services
{
    public interface IAuthorizationService
         : IService
    {
        Task<bool> IsCurrentUserInRole(string roleName);
        Task<bool> CurrentUserHasCompetitorRole();
    }
}
