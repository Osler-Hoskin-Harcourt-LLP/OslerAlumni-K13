using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CMS.Base;
using CMS.DataEngine;
using CMS.Membership;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;

namespace OslerAlumni.Admin.Core.Repositories
{
    public class AdminUserRepository
        : UserRepository, IAdminUserRepository
    {
        private readonly IMvcApiService _mvcApiService;

        #region "Constants"

        protected const string UserNameForbiddenCharacters = @"[^a-zA-Z0-9]+";

        #endregion
        
        public AdminUserRepository(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            IMvcApiService mvcApiService,
            ContextConfig context)
            : base(
                eventLogRepository,
                settingsKeyRepository,
                context)
        {
            _mvcApiService = mvcApiService;
        }

        #region "Methods"

        public virtual string GenerateNewPassword(
            string siteName = null)
        {
            return UserInfoProvider.GenerateNewPassword(
                siteName.ReplaceIfEmpty(_context.Site?.SiteName));
        }

        public string GetPasswordResetToken(Guid userGuid)
        {
            //BEGIN TEMP
                _eventLogRepository.LogWarning(
                      GetType(), "ImportAsUser",
                      $"TEMP Notice for user GUID: '{userGuid.ToString()}'. Code Location 3.4 Successfully Hit.");
            //END TEMP

            //BEGIN TEMP
            if (_mvcApiService != null)
            {
                _eventLogRepository.LogWarning(
                      GetType(), "ImportAsUser",
                      $"TEMP Notice. Code Location 3.5 Successfully Hit.");
            }
            //END TEMP
            return _mvcApiService.GetPasswordResetTokenAsync(userGuid).GetAwaiter().GetResult();
        }

        public IList<IOslerUserInfo> GetAlumniUsers(
            int topN,
            int? startingId = null,
            IEnumerable<string> columnNames = null,
            string orderByColumnName = null,
            OrderDirection orderDirection = OrderDirection.Ascending,
            string siteName = null)
        {
            if (topN < 1)
            {
                return null;
            }

            var userQuery = UserInfoProvider.GetUsers()
                .OnSite(
                    siteName.ReplaceIfEmpty(_context.Site?.SiteName))
                .WhereNotEmpty(
                    nameof(IOslerUserInfo.OnePlaceReference))
                .WhereTrue(
                    nameof(IOslerUserInfo.IsAlumni))
                .WhereTrue(
                    nameof(IOslerUserInfo.UserEnabled))
                .TopN(topN);

            if (startingId.HasValue)
            {
                userQuery = userQuery.WhereGreaterOrEquals(
                    nameof(IOslerUserInfo.UserID),
                    startingId.Value);
            }

            var columnNameList = columnNames?.ToList();

            if ((columnNameList?.Count ?? 0) > 0)
            {
                userQuery = userQuery.Columns(columnNameList);
            }

            if (!string.IsNullOrWhiteSpace(orderByColumnName))
            {
                switch (orderDirection)
                {
                    case OrderDirection.Descending:
                        userQuery = userQuery.OrderByDescending(
                            orderByColumnName);

                        break;
                    default:
                        userQuery = userQuery.OrderByAscending(
                            orderByColumnName);

                        break;
                }
            }

            return userQuery
                .Select(
                    user => (IOslerUserInfo)new OslerUserInfo(user))
                .ToList();
        }
        
        public virtual IOslerUserInfo GetUserByOnePlaceReference(
            string onePlaceRef,
            string siteName = null)
        {
            if (string.IsNullOrWhiteSpace(onePlaceRef))
            {
                return null;
            }

            var user = UserInfoProvider.GetUsers()
                .OnSite(
                    siteName.ReplaceIfEmpty(_context.Site?.SiteName))
                .WhereEquals(
                    nameof(IOslerUserInfo.OnePlaceReference),
                    onePlaceRef)
                // If we end up with the case of multiple users with the same OnePlace reference,
                // the one that was created first will have priority
                .OrderByAscending(
                    nameof(IOslerUserInfo.UserID))
                .TopN(1)
                .FirstOrDefault();

            return (user == null)
                ? null
                : new OslerUserInfo(user);
        }
        
        public virtual IOslerUserInfo GetUserByUserName(
            string userName,
            string siteName = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            var user = UserInfoProvider.GetUsers()
                .OnSite(
                    siteName.ReplaceIfEmpty(_context.Site?.SiteName))
                .WhereEquals(
                    nameof(IOslerUserInfo.UserName),
                    userName)
                .TopN(1)
                .FirstOrDefault();

            return (user == null)
                ? null
                : new OslerUserInfo(user);
        }

        public virtual string SanitizeUserName(
            string userName,
            string siteName = null)
        {
            // NOTE: Tried using ValidationHelper.GetSafeUserName(),
            // but it is leaving a lot of special chars in.
            // This is a bit of a roundabout way of achieving username sanitizations

            var sanitizedUserName = userName
                .ToSafeKenticoIdentifier(
                    siteName.ReplaceIfEmpty(_context.Site?.SiteName));

            sanitizedUserName =
                Regex.Replace(
                    sanitizedUserName,
                    UserNameForbiddenCharacters,
                    string.Empty);

            return sanitizedUserName.ToLowerCSafe();
        }

        public virtual IOslerUserInfo SetTemporaryPassword(
            IOslerUserInfo user,
            string password)
        {
            UserInfoProvider.SetPassword(
                user.UserInfo,
                password,
                false);

            return user;
        }

        #endregion
    }
}
