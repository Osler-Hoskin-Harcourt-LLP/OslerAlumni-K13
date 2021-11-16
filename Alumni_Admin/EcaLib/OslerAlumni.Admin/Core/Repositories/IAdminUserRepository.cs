using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.DataEngine;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;

namespace OslerAlumni.Admin.Core.Repositories
{
    public interface IAdminUserRepository
        : IUserRepository
    {
        string GenerateNewPassword(
            string siteName = null);

        string GetPasswordResetToken(Guid userGuid);

        IList<IOslerUserInfo> GetAlumniUsers(
            int topN,
            int? startingId = null,
            IEnumerable<string> columnNames = null,
            string orderByColumnName = null,
            OrderDirection orderDirection = OrderDirection.Ascending,
            string siteName = null);

        IOslerUserInfo GetUserByOnePlaceReference(
            string onePlaceRef,
            string siteName = null);

        IOslerUserInfo GetUserByUserName(
            string userName,
            string siteName = null);

        string SanitizeUserName(
            string userName,
            string siteName = null);

        IOslerUserInfo SetTemporaryPassword(
            IOslerUserInfo user,
            string password);
    }
}
