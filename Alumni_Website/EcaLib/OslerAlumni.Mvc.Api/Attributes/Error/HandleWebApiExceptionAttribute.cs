using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using ECA.Core.Repositories;

namespace OslerAlumni.Mvc.Api.Attributes.Error
{
    public class HandleWebApiExceptionAttribute
        : ExceptionFilterAttribute
    {
        #region "Properties"

        public IEventLogRepository EventLogRepository { get; set; }

        #endregion

        #region "Events"

        public override void OnException(
            HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exception == null)
            {
                return;
            }

            // OperationCanceledException and TaskCanceledException (derived from OperationCanceledException)
            // are thrown when browser cancels the API call, e.g. when user navigates away from the page
            // that called the API, before API execution was finished
            if (exception is OperationCanceledException)
            {
                return;
            }

            var actionDescriptor = actionExecutedContext.ActionContext.ActionDescriptor;

            EventLogRepository.LogError(
                actionDescriptor.ControllerDescriptor.ControllerType,
                actionDescriptor.ActionName,
                exception);

            actionExecutedContext.Response =
                actionExecutedContext.Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    "A server error occurred when processing the request.");
        }

        #endregion
    }
}
