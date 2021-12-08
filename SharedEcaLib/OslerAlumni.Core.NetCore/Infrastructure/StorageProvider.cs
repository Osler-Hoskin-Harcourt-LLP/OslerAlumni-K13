using CMS.IO;

using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Infrastructure
{
    public static class OslerAlumniStorageProvider
    {
        public static void RegisterProvider(
            string rootStoragePath)
        {
            // Creates a new StorageProvider instance
            AbstractStorageProvider mediaProvider = 
                StorageProvider.CreateFileSystemStorageProvider();

            // Specifies the target root directory. The provider creates the relative path of
            // the mapped folders within the given directory.
            mediaProvider.CustomRootPath = rootStoragePath;

            // Maps a directory to the provider
            StorageHelper.MapStoragePath(
                GlobalConstants.FilePaths.Root, 
                mediaProvider);
        }
    }
}
