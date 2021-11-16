using System.Net;
using System.Web;
using System.Web.Mvc;
using ECA.Core.Repositories;
using OslerAlumni.Mvc.Core.Definitions;

namespace OslerAlumni.Mvc.Core.Attributes.Error
{
    public class HandleMvcExceptionAttribute
        : HandleErrorAttribute
    {
        #region "Properties"

        public IEventLogRepository EventLogRepository { get; set; }

        #endregion

        public HandleMvcExceptionAttribute()
        {
            // Name of the error view
            View = "ServerError";
        }

        #region "Events"
        
        public override void OnException(
            ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

            if (exception == null)
            {
                return;
            }

            var routeValues = filterContext.RouteData.Values;
            
            EventLogRepository.LogError(
                filterContext.Controller.GetType(),
                (string)routeValues[Constants.RouteParams.Action],
                exception);

            base.OnException(filterContext);

            if (exception is HttpRequestValidationException)
            {
                filterContext.HttpContext.Response.StatusCode = 
                    (int) HttpStatusCode.BadRequest;
            }
        }

        #endregion
    }
}
