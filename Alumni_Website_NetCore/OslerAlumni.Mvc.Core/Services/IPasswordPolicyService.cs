using ECA.Core.Services;
using Kentico.Membership;
using Microsoft.AspNetCore.Identity;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IPasswordPolicyService
        : IService
    {
        PasswordValidator<ApplicationUser> PasswordValidator { get; }
    }
}
