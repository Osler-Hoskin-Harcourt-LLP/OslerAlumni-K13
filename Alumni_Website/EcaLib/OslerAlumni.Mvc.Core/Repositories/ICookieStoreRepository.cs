using ECA.Core.Repositories;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface ICookieStoreRepository 
        : IRepository
    {
        T Get<T>() 
            where T : new();

        bool Exists<T>() 
            where T : new();

        void Save(
            object obj);

        void Save(
            object obj, 
            object options);

        void Delete<T>() 
            where T : new();
    }
}
