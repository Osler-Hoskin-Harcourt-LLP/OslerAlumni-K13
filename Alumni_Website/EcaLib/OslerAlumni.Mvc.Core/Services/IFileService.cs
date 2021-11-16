using System.Web;
using ECA.Core.Services;

namespace OslerAlumni.Mvc.Core.Services
{
    public interface IFileService
        : IService
    {
        /// <summary>
        /// Copies the Uploaded file to the Forms Upload Directory and returns a filename that can be added to an File property of a Form Table
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns>Null if unsuccessful and FileAttachmentName if successful</returns>
        string SaveFileForFormAttachment(
            HttpPostedFileBase fileUpload);

        /// <summary>
        /// Given the attachmentFileName, returns the full file system path for it.
        /// </summary>
        /// <param name="attachmentName"></param>
        /// <returns></returns>
        string GetFullPathForFormAttachment(
            string attachmentName);
    }
}
