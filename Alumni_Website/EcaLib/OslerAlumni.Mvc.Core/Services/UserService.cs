using System;
using System.Net;
using System.Web;
using CMS.IO;
using CMS.MediaLibrary;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.OnePlace.Services;

namespace OslerAlumni.Mvc.Core.Services
{
    public class UserService
        : ServiceBase, IUserService
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOnePlaceUserExportService _onePlaceUserExportService;
        private readonly IProfileService _profileService;
        private readonly IMediaLibraryService _mediaLibraryService;
        private readonly IImageService _imageService;
        private readonly ContextConfig _context;

        #endregion

        public UserService(
            IEventLogRepository eventLogRepository,
            IUserRepository userRepository,
            IOnePlaceUserExportService onePlaceUserExportService,
            IProfileService profileService,
            IMediaLibraryService mediaLibraryService,
            IImageService imageService,
            ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _userRepository = userRepository;
            _onePlaceUserExportService = onePlaceUserExportService;
            _profileService = profileService;
            _mediaLibraryService = mediaLibraryService;
            _imageService = imageService;
            _context = context;
        }

        #region "Methods"

        public bool Save(
            IOslerUserInfo user)
        {
            try
            {
                if (user?.UserInfo == null)
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(Save),
                        "Missing user object.");

                    return false;
                }

                if (!_onePlaceUserExportService
                    .SubmitUserUpdateToOnePlace(ref user))
                {
                    // Assume the error was handled in the service
                    return false;
                }

                // Reset this field so that it doesn't affect the next save action
                user.UpdateOnePlace = null;

                if (!_userRepository.Save(user))
                {
                    // Assume the error was handled in the repo
                    return false;
                }

                // Update the profile document associated with this user
                if (!_profileService.UpdateProfile(user))
                {
                    _eventLogRepository.LogError(
                        GetType(),
                        nameof(Save),
                        "Failed to Update User Profile Page");

                    return false;
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

        /// <inheritdoc />
        public bool Save(
            Guid userGuid,
            Action<IOslerUserInfo> userDelta)
        {
            try
            {
                var user =
                    _userRepository.GetByGuid(userGuid);

                userDelta.Invoke(user);

                return Save(user);
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


        public string GetProfileUrl(IOslerUserInfo user, string culture = null)
        {
            return _profileService
                .GetProfileUrl(
                    user?.OnePlaceReference,
                    culture.ReplaceIfEmpty(_context.CultureName));
        }

        public bool UploadProfileImage(IOslerUserInfo user, HttpPostedFileBase file)
        {
            bool updated = false;

            string profileImageUrl;


            if (_mediaLibraryService.UploadMediaFile(file, MediaLibrary.ProfileImages, out profileImageUrl,
                fileGuid: user.UserGUID))
            {
                updated = Save(
                    user.UserGUID,
                    userInfo => { userInfo.ProfileImage = profileImageUrl; });
            }


            return updated;
        }

        public bool CropProfileImage(IOslerUserInfo user, int x, int y, int width, int height)
        {
            try
            {
                bool updated = false;

                MediaFileInfo mediaFile;

                byte[] profileImage;

                if (!_mediaLibraryService.GetMediaFile(user.UserGUID, out mediaFile, out profileImage))
                {
                    return false;
                }

                var croppedImage = _imageService.ImageCrop(profileImage, x, y, width, height);

                string profileImageUrl;

                if (_mediaLibraryService.UploadMediaFile(
                    croppedImage,
                    mediaFile.FileName,
                    mediaFile.FileExtension,
                    MediaLibrary.ProfileImages,
                    out profileImageUrl,
                    user.UserGUID))
                {
                    updated = Save(
                        user.UserGUID,
                        userInfo => { userInfo.ProfileImage = profileImageUrl; });
                }


                return updated;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(), nameof(CropProfileImage), ex);

                return false;
            }
        }

        public bool DeleteProfileImage(IOslerUserInfo user)
        {
            bool updated = false;

            if (_mediaLibraryService.DeleteMediaFile(fileGuid: user.UserGUID))
            {
                updated = Save(
                    user.UserGUID,
                    userInfo => { userInfo.ProfileImage = null; });
            }

            return updated;
        }
    }

    #endregion
}

