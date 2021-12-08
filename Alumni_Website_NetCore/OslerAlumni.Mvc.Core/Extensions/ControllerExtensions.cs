using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OslerAlumni.Mvc.Core.Extensions
{
    public static class ControllerExtensions
    {
        public static StatusCodeResult BadRequest(
            this Controller controller,
            string statusDescription = null)
        {
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        public static StatusCodeResult Forbidden(
            this Controller controller,
            string statusDescription = null)
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
