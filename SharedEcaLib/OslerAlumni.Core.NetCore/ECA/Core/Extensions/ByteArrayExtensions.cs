using System.IO;

namespace ECA.Core.Extensions
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Writes byte data to a tempory file
        /// </summary>
        /// <param name="data"></param>
        /// <returns>path to the temp file</returns>
        public static string ToTempFile(this byte[] data)
        {
            var tempfilePath = Path.GetTempFileName();

            File.WriteAllBytes(tempfilePath, data);

            return tempfilePath;
        }
    }

}
