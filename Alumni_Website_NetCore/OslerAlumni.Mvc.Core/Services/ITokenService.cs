using System;
using System.Threading.Tasks;
using ECA.Core.Services;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface ITokenService 
        : IService
    {
        Task<string> GeneratePasswordResetTokenAsync(
            Guid userGuid);

        Task<bool> VerifyPasswordResetToken(
            Guid userGuid, 
            string token);
    }
}
