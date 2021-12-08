using System;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Core.Repositories
{
    public interface IUserRepository
        : IRepository
    {
        IOslerUserInfo CurrentUser { get; }

        IOslerUserInfo GetById(
            int userId,
            bool getFromDb = false);

        IOslerUserInfo GetByGuid(
            Guid userGuid);

        IOslerUserInfo GetByName(
            string userName);

        IOslerUserInfo GetByEmail(
            string email,
            bool enabledOnly = true);

        IOslerUserInfo GetSystemUser();

        /// <summary>
        /// Updates the User Object
        /// </summary>
        /// <param name="user"></param>
        /// <param name="siteName"></param>
        /// <returns>true if update was successful</returns>
        bool Save(
            IOslerUserInfo user,
            string siteName = null);

        bool IsSystemUser(string userName);
    }
}
