using Microsoft.AspNetCore.Http;
using System.IO;
using System.Reflection;
using System.Web;

namespace ECA.Core.Extensions
{
    public static class HttpPostedFileBaseExtensions
    {
        /// <summary>
        /// Used to convert HttpPostedFileBase to HttpPostedFile
        /// Need reflection because the contructor is private
        /// Not the best solution as this can lead to problems
        /// 
        /// https://stackoverflow.com/questions/36133385/httppostedfilebase-convert-to-httppostedfile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        //public static IFormFile ToHttpPostedFile(this IFormFile file)
        //{
        //    var constructorInfo =
        //        typeof(IFormFile).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
        //    var httpPostedFile = (IFormFile) constructorInfo
        //        .Invoke(new object[] {file.FileName, file.ContentType, file.InputStream});

        //    return httpPostedFile;
        //}

        public static byte[] ToByteArray(this IFormFile file)
        {
            using (MemoryStream target = new MemoryStream())
            {
                file.CopyTo(target);
                byte[] data = target.ToArray();

                return data;
            }            
        }
    }

}
