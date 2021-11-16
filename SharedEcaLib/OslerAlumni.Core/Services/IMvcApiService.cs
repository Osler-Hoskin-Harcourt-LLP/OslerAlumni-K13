using System;
using System.Threading.Tasks;

namespace OslerAlumni.Core.Services
{
    /// <summary>
    /// Used to Communcate with MVC site's apis.
    /// </summary>
    public interface IMvcApiService
    {
        Task<string> GetPasswordResetTokenAsync(Guid userGuid);
    }
}
