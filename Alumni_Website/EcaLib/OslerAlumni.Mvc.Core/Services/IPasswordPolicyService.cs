using ECA.Core.Services;
using Microsoft.AspNet.Identity;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IPasswordPolicyService
        : IService
    {
        PasswordValidator PasswordValidator { get; }
    }
}
