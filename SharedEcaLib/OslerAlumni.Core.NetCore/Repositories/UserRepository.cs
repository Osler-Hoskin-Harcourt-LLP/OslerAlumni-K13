using System;
using System.Linq;
using System.Web;
using CMS.Membership;
using ECA.Core.Definitions;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using Microsoft.AspNetCore.Http;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Core.Repositories
{
    public class UserRepository
        : IUserRepository
    {
        #region "Private fields"

        protected readonly IEventLogRepository _eventLogRepository;
        protected readonly ISettingsKeyRepository _settingsKeyRepository;
        protected readonly ContextConfig _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        #endregion

        #region "Properties"

        public IOslerUserInfo CurrentUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return (_context.User == null)
                        ? null
                        : new OslerUserInfo(_context.User);
                }

                return null;
            }
        }

        #endregion

        public UserRepository(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository,
            ContextConfig context,
            IHttpContextAccessor httpContextAccessor)
        {
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #region "Methods"

        public IOslerUserInfo GetById(
            int userId,
            bool getFromDb = false)
        {
            if (getFromDb)
            {
                UserInfoProvider.InvalidateUser(userId);
            }

            var user = UserInfoProvider.GetUserInfo(userId);

            return (user == null)
                ? null
                : new OslerUserInfo(user);
        }

        public IOslerUserInfo GetByGuid(
            Guid userGuid)
        {
            if (userGuid == Guid.Empty)
            {
                return null;
            }

            var user = UserInfoProvider.GetUserInfoByGUID(userGuid);

            return (user == null)
                ? null
                : new OslerUserInfo(user);
        }

        public IOslerUserInfo GetByName(
            string userName)
        {
            var user = UserInfoProvider.GetUserInfo(userName);

            return user == null ? null : new OslerUserInfo(user);
        }

        public IOslerUserInfo GetByEmail(
            string email,
            bool enabledOnly = true)
        {
            // Note that the assumption is that the email address is not necessarily unique to a user
            // An arbitrary account is selected if more than 1 users with the same email exist.

            var query = UserInfoProvider
                .GetUsers()
                .WhereEquals(nameof(IOslerUserInfo.Email), email);

            if (enabledOnly)
            {
                query = query.WhereEquals(nameof(IOslerUserInfo.UserEnabled), true);
            }

            var user = query.FirstOrDefault();

            return user == null ? null : new OslerUserInfo(user);
        }

        public IOslerUserInfo GetSystemUser()
        {
            // If the current user is null then use the system user
            var defaultUserGuid = _settingsKeyRepository.GetValue<Guid>(
                ECAGlobalConstants.Settings.DefaultSystemUser);

            IOslerUserInfo user = null;

            if (defaultUserGuid != Guid.Empty)
            {
                user = GetByGuid(defaultUserGuid);
            }

            if (user == null)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(GetSystemUser),
                    "Default system user is either not set or is missing.");
            }

            return user;
        }

        /// <inheritdoc />
        public virtual bool Save(
            IOslerUserInfo user,
            string siteName = null)
        {
            if (user?.UserInfo == null)
            {
                return false;
            }

            try
            {
                user.AutopopulateDependantFields();

                UserInfoProvider.SetUserInfo(user.UserInfo);

                if (!string.IsNullOrWhiteSpace(user.UserName))
                {
                    UserInfoProvider.AddUserToSite(
                        user.UserName,
                        siteName.ReplaceIfEmpty(_context.Site?.SiteName));
                }

                if (user.IsCompetitor)
                {
                    UserInfoProvider.AddUserToRole(user.UserName, GlobalConstants.Roles.CompetitorRole,
                        siteName.ReplaceIfEmpty(_context.Site?.SiteName));
                }
                else
                {
                    UserInfoProvider.RemoveUserFromRole(user.UserName, GlobalConstants.Roles.CompetitorRole,
                        siteName.ReplaceIfEmpty(_context.Site?.SiteName));
                }


                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(Save),
                    ex);

                return false;
            }
        }
        /// <summary>
        /// Determines if the user is logged in ViaOslerNetwork
        /// We assign those users as System User
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsSystemUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            var systemUser = GetSystemUser();

            if (systemUser == null)
            {
                return false;
            }

            return (userName == systemUser.UserName);
        }

        #endregion
    }
}
