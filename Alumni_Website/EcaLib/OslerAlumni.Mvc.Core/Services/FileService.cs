using System;
using System.Web;
using CMS.Helpers;
using CMS.IO;
using ECA.Core.Repositories;
using ECA.Core.Services;
using OslerAlumni.Core.Definitions;

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
        public string SaveFileForFormAttachment(HttpPostedFileBase fileUpload)
        {
            try
            {
                var fileAttachment = GetAttachmentFileInfo(fileUpload);

                fileUpload.SaveAs(fileAttachment.FinalFilePath);

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
            HttpPostedFileBase fileUpload)
        {
            var fileName = Path.GetFileNameWithoutExtension(fileUpload.FileName);
            var fileExtension = Path.GetExtension(fileUpload.FileName);

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
