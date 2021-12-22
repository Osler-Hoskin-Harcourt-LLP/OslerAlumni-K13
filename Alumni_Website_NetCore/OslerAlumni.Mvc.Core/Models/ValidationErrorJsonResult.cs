using System.Net;
using CMS.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
                        Result = ModelStateDictionaryToDictionary(modelState),
                        Status = WebResponseStatus.Error,
                    });

            ContentType = "application/json";
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode =
                (int) HttpStatusCode.BadRequest;

            return base.ExecuteResultAsync(context);
        }

        private Dictionary<string, string[]> ModelStateDictionaryToDictionary(ModelStateDictionary modelState)
        {
            var result = new Dictionary<string, string[]>();
            foreach (var key in modelState.Keys)
            {
                result.Add(key, modelState[key].Errors.Select(e => ResHelper.GetString(e.ErrorMessage)).ToArray());
            }

            return result;
        }
    }
}
