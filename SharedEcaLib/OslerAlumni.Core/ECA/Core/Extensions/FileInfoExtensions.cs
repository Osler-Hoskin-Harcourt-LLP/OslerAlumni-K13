using System.IO;
using FileInfo = CMS.IO.FileInfo;

namespace ECA.Core.Extensions
{
    public static class FileInfoExtensions
    {
        public static byte[] ToByteArray(this FileInfo file)
        {
            using (MemoryStream target = new MemoryStream())
            {
                using (Stream source = file.OpenRead())
                {
                    source.CopyTo(target);
                }

                byte[] data = target.ToArray();
                return data;
            }
        }
    }

}
