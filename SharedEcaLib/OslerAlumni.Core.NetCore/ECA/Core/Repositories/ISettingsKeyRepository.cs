namespace ECA.Core.Repositories
{
    public interface ISettingsKeyRepository
        : IRepository
    {
        T GetValue<T>(
            string keyName,
            string culture = null,
            string siteName = null);
    }
}
