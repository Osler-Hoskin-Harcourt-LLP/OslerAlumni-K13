using System;
using System.IO;
using System.Web;
using CMS.Helpers;
using CMS.MediaLibrary;
using ECA.Core.Extensions;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Microsoft.AspNetCore.Http;
using OslerAlumni.Core.Definitions;
using FileInfo = CMS.IO.FileInfo;
using Path = CMS.IO.Path;

namespace OslerAlumni.Mvc.Core.Services
{
    public class MediaLibraryService : ServiceBase, IMediaLibraryService
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly ContextConfig _context;

        public MediaLibraryService(IEventLogRepository eventLogRepository, ContextConfig context)
        {
            _eventLogRepository = eventLogRepository;
            _context = context;
        }

        public bool UploadMediaFile(IFormFile file, MediaLibrary mediaLibraryType, out string filePath,
            Guid? fileGuid = null)
        {
            return UploadMediaFile(file.ToByteArray(), Path.GetFileNameWithoutExtension(file.FileName),
                Path.GetExtension(file.FileName), mediaLibraryType, out filePath, fileGuid);
        }


        /// <inheritdoc />
        public bool UploadMediaFile(
            Byte[] file,
            string fileName,
            string fileExtension,
            MediaLibrary mediaLibraryType,
            out string filePath,
            Guid? fileGuid = null)
        {
            filePath = string.Empty;

            if (file == null)
            {
                return false;
            }

            var mediaLibrary =
                MediaLibraryInfoProvider.GetMediaLibraryInfo(mediaLibraryType.ToStringRepresentation(),
                    _context.Site.SiteName);

            if (mediaLibrary == null)
            {
                _eventLogRepository.LogError(GetType(), nameof(UploadMediaFile), $"Media library {mediaLibraryType.ToStringRepresentation()} doesn't exist.");

                return false;
            }

            //Delete old image if exits
            if (fileGuid.HasValue)
            {
                DeleteMediaFile(fileGuid.Value);
            }

            var tempfilePath = file.ToTempFile();

            fileGuid = fileGuid ?? Guid.NewGuid();

            MediaFileInfo mediaFile;

            if (!CreateMediaFileInfo(
                tempfilePath,
                fileName,
                fileExtension,
                fileGuid.Value,
                mediaLibrary,
                out mediaFile))
            {
                return false;
            }

            filePath = MediaLibraryHelper.GetPermanentUrl(mediaFile);

            return true;
        }

        private bool CreateMediaFileInfo(string sourceFilePath, string fileName,
            string fileExtension, Guid fileGuid, MediaLibraryInfo mediaLibrary, out MediaFileInfo mediaFile)
        {
            mediaFile = null;

            try
            {
                mediaFile = new MediaFileInfo(sourceFilePath, mediaLibrary.LibraryID)
                {
                    FileName = fileName,
                    FileTitle = fileName,
                    FilePath = "/",
                    FileExtension = fileExtension,
                    FileSiteID = _context.Site.SiteID,
                    FileLibraryID = mediaLibrary.LibraryID,
                    FileGUID = fileGuid
                };

                // Sets the media library file properties
                mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(mediaFile.FileExtension);

                MediaFileInfoProvider.SetMediaFileInfo(mediaFile);
                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(), nameof(CreateMediaFileInfo), ex);

                return false;
            }
        }

        public bool GetMediaFile(Guid fileGuid, out MediaFileInfo mediaFileInfo, out byte[] fileData)
        {
            mediaFileInfo = null;

            fileData = null;

            try
            {
                mediaFileInfo = MediaFileInfoProvider.GetMediaFileInfo(fileGuid, _context.Site.SiteName);

                if (mediaFileInfo == null)
                {
                    return false;
                }

                var filePath =
                    MediaFileInfoProvider.GetMediaFilePath(mediaFileInfo.FileLibraryID, mediaFileInfo.FilePath);

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return false;
                }

                FileInfo file = FileInfo.New(filePath);

                fileData = file.ToByteArray();

                if (fileData == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(), nameof(GetMediaFile), ex);

                return false;
            }

            return true;

        }

        /// <summary>
        /// Deletes the mediafile by fileGuid
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        public bool DeleteMediaFile(Guid fileGuid)
        {
            try
            {
                var previousFile = MediaFileInfoProvider.GetMediaFileInfo(fileGuid, _context.Site.SiteName);

                if (previousFile != null)
                {
                    DeleteMediaFileInfo(previousFile);
                }

                return true;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(GetType(), nameof(DeleteMediaFile), ex);

                return false;
            }
        }

        /// <summary>
        /// Deletes the media file
        /// </summary>
        /// <param name="mediaFileInfo"></param>
        public void DeleteMediaFileInfo(MediaFileInfo mediaFileInfo)
        {
            MediaFileInfoProvider.DeleteMediaFileInfo(mediaFileInfo);
        }
    }
}

