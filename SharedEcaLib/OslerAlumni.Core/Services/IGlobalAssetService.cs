using ECA.Core.Services;

namespace OslerAlumni.Core.Services
{
    public interface IGlobalAssetService 
        : IService
    {
        string DefaultImageUrl { get; }

        string GetWebsiteLogoUrl(
            string cultureName = null);

        string GetEmailLogoUrl(
            string cultureName = null);

        string GetWebsiteMobileLogoUrl(
            string cultureName = null);
    }
}
