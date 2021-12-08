using ECA.Core.Services;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IIpLocatorService
        : IService
    {
        string GetCurrentUserIpAddress();

        bool IsCurrentUserInOslerNetwork();
    }
}
