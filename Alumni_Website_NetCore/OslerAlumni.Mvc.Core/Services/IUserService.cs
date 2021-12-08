using System;
using System.Web;
using ECA.Core.Services;
using Microsoft.AspNetCore.Http;
using OslerAlumni.Core.Kentico.Models;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IUserService
        : IService
    {
        bool Save(
            IOslerUserInfo user);

        /// <summary>
        /// Updates the User Object
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userDelta"> Action that allows to overwrite some or all User Properties. </param>
        /// <returns>true if update was successful</returns>
        bool Save(
            Guid userGuid,
            Action<IOslerUserInfo> userDelta);

        /// <summary>
        /// Returns the cultured url for the profile page for the user if exists.
        /// Note: Must have been imported from oneplace.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        string GetProfileUrl(IOslerUserInfo user, string culture = null);


        bool UploadProfileImage(IOslerUserInfo user, IFormFile file);

        bool CropProfileImage(IOslerUserInfo user, int x, int y, int width, int height);

        bool DeleteProfileImage(IOslerUserInfo user);
    }
}
