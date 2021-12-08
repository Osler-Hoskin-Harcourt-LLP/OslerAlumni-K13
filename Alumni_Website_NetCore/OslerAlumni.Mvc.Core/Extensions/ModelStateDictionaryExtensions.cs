
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OslerAlumni.Mvc.Core.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static Dictionary<string, string[]> GetErrorDictionary(this ModelStateDictionary modelState)
        {
            Dictionary<string, string[]> errorList = modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors?.Select(e => e.ErrorMessage)?.ToArray()
            );

            return errorList;
        }
    }
}
