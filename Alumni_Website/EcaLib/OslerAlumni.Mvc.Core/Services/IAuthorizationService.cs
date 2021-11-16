using System;
using System.Threading.Tasks;
using ECA.Core.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IAuthorizationService
         : IService
    {
        bool IsCurrentUserInRole(string roleName);
        bool CurrentUserHasCompetitorRole();
    }
}
