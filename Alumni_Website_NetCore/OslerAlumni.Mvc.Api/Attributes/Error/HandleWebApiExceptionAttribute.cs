using ECA.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

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
            ExceptionContext actionExecutedContext)
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

            var actionDescriptor = actionExecutedContext.ActionDescriptor;

            EventLogRepository.LogError(
                actionDescriptor.DisplayName,
                actionDescriptor.DisplayName,
                exception);

            actionExecutedContext.Result = new ContentResult() { StatusCode = (int)HttpStatusCode.InternalServerError, Content = "A server error occurred when processing the request." };
        }

        #endregion
    }
}
