using System;
using System.IO;
using System.Web;
using CMS.MediaLibrary;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IMediaLibraryService
        : IService
    {
        /// <summary>
        /// Uploads a file to a media library        
        /// </summary>
        /// <param name="file"></param>
        /// <param name="mediaLibraryType"></param>
        /// <param name="filePath">returns the full path to uploaded image</param>
        /// <param name="fileGuid">If fileGuid provided, will update the image if it already exist, else will create one with this guid.</param>
        /// <returns>true/false if successful</returns>
        bool UploadMediaFile(HttpPostedFileBase file, MediaLibrary mediaLibraryType, out string filePath,
            Guid? fileGuid = null);

        /// <summary>
        /// Uploads a file to a media library
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <param name="mediaLibraryType"></param>
        /// <param name="filePath">returns the full path to uploaded image</param>
        /// <param name="fileGuid">If fileGuid provided, will update the image if it already exist, else will create one with this guid.</param>
        /// <returns></returns>
        bool UploadMediaFile(
            Byte[] file,
            string fileName,
            string fileExtension,
            MediaLibrary mediaLibraryType,
            out string filePath,
            Guid? fileGuid = null);

        bool GetMediaFile(Guid fileGuid, out MediaFileInfo mediaFileInfo, out byte[] fileData);

        void DeleteMediaFileInfo(MediaFileInfo mediaFileInfo);

        bool DeleteMediaFile(Guid fileGuid);
    }
}
