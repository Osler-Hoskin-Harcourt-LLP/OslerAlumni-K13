using System;
using System.Web;
using CMS.Helpers;
using CMS.IO;
using ECA.Core.Repositories;
using ECA.Core.Services;
using Microsoft.AspNetCore.Http;
using OslerAlumni.Core.Definitions;
using FileInfo = CMS.IO.FileInfo;

namespace OslerAlumni.Mvc.Core.Services
{
    public class FileService
        : ServiceBase, IFileService
    {
        #region "Private fields"

        private readonly IEventLogRepository _eventLogRepository;
        private readonly ISettingsKeyRepository _settingsKeyRepository;

        #endregion

        public FileService(
            IEventLogRepository eventLogRepository,
            ISettingsKeyRepository settingsKeyRepository)
        {
            _eventLogRepository = eventLogRepository;
            _settingsKeyRepository = settingsKeyRepository;
        }

        #region "Methods"

        /// <inheritdoc />
        public string SaveFileForFormAttachment(IFormFile fileUpload)
        {
            try
            {
                var fileAttachment = GetAttachmentFileInfo(fileUpload);

                using (Stream fileStream = new System.IO.FileStream(fileAttachment.FinalFilePath, System.IO.FileMode.Create))
                {
                    fileUpload.CopyTo(fileStream);
                }

                return fileAttachment.AttachmentFileName;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(SaveFileForFormAttachment),
                    ex);

                return null;
            }
        }

        /// <inheritdoc />
        public string GetFullPathForFormAttachment(
            string attachmentName)
        {
            // Remove the extra file name that kentico adds for display purposes.
            if (attachmentName.Contains("/"))
            {
                attachmentName = attachmentName.Substring(0, attachmentName.LastIndexOf("/", StringComparison.Ordinal));
            }

            var relativeFilePath = _settingsKeyRepository.GetValue<string>(
                GlobalConstants.Settings.FormFilesFolder);

            relativeFilePath = $"{URLHelper.GetPhysicalPath(relativeFilePath)}\\{attachmentName}";

            // We are Using CMS.IO instead of System.IO since CMS.IO takes into account StorageProviders
            var fi = FileInfo.New(relativeFilePath);

            return fi.FullName;
        }

        #endregion

        #region "Helper methods"

        private FileAttachment GetAttachmentFileInfo(
            IFormFile fileUpload)
        {
            var fileName = CMS.IO.Path.GetFileNameWithoutExtension(fileUpload.FileName);
            var fileExtension = CMS.IO.Path.GetExtension(fileUpload.FileName);

            var guidFileName = $"{Guid.NewGuid()}{fileExtension}";
            var attachmentFileName = $"{guidFileName}/{fileName}{fileExtension}";

            return new FileAttachment
            {
                FinalFilePath = GetFullPathForFormAttachment(guidFileName),
                AttachmentFileName = attachmentFileName
            };
        }

        #endregion

        #region "Nested classes"

        private class FileAttachment
        {
            public string FinalFilePath { get; set; }

            public string AttachmentFileName { get; set; }
        }

        #endregion
    }
}
