using System;
using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Core.Services
{
    public interface IProfileService
        : IService
    {
        bool CreateProfile(
            IOslerUserInfo user);

        bool UpdateProfile(
            IOslerUserInfo user);

        bool DeleteProfile(
            IOslerUserInfo user);

        /// <summary>
        /// For a given culture, returns all directory urls for published profiles
        /// in the system.
        /// </summary>
        /// <returns>
        /// Dictionary of OnePlaceReferenceId and Cultured Urls
        /// </returns>
        Dictionary<string, string> GetProfileUrls(string culture);

        /// <summary>
        /// Returns profile link for a corresponding OnePlaceUser if exits
        /// in the system.
        /// </summary>
        /// <returns>
        /// Cultured Url
        /// </returns>
        string GetProfileUrl(string oneplaceReferenceId, string culture);

        PageType_Profile GetUserProfile(Guid userGuid);
    }
}
