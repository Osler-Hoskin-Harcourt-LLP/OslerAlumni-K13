using System.Web;

namespace OslerAlumni.Mvc.Core.Helpers
{
    public static class HttpResponseHelper
    {
        /// <summary>
        /// Note it is important to set this, so that IIS lets through the existing response body
        /// in the case of a non-successful response status code (400 in this case). If this flag is not set,
        /// and <httpErrors existingResponse="..."> is set to:
        /// - "PassThrough": the response body will still go through;
        /// - "Auto": the response body will NOT go through;
        /// - "Replace": the response body will NOT go through regardless of the flag.
        /// Response body here would contain the actual validation error messages for the front-end to display.
        /// </summary>
        /// <param name="context"></param>
        public static void SkipIisCustomErrors(HttpContextBase context)
        {
            context.Response.TrySkipIisCustomErrors = true;
        }
    }
}
