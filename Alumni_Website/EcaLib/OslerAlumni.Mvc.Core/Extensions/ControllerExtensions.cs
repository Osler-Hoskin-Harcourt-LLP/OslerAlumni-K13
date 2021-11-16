using System.Net;
using System.Web.Mvc;

namespace OslerAlumni.Mvc.Core.Extensions
{
    public static class ControllerExtensions
    {
        public static HttpStatusCodeResult BadRequest(
            this Controller controller,
            string statusDescription = null)
        {
            return new HttpStatusCodeResult(
                HttpStatusCode.BadRequest,
                statusDescription);
        }

        public static HttpStatusCodeResult Forbidden(
            this Controller controller,
            string statusDescription = null)
        {
            return new HttpStatusCodeResult(
                HttpStatusCode.Forbidden,
                statusDescription);
        }
    }
}
