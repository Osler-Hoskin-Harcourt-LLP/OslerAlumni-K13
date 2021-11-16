using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Models;
using OslerAlumni.Mvc.Core.Extensions;

namespace OslerAlumni.Mvc.Core.Models
{
    public class ValidationErrorJsonResult: ContentResult
    {
        public ValidationErrorJsonResult(ModelStateDictionary modelState)
        {
            Content = JsonConvert.SerializeObject(

                new BaseWebResponse<Dictionary<string, string[]>>()
                    {
                        Result = modelState.GetErrorDictionary(),
                        Status = WebResponseStatus.Error,
                    });

            ContentType = "application/json";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.StatusCode =
                (int) HttpStatusCode.BadRequest;

            base.ExecuteResult(context);
        }
    }
}
