using System.Globalization;
using ECA.Core.Services;
using Microsoft.AspNetCore.Routing;

namespace ECA.Mvc.Core.Services
{
    public interface ICultureService
        : IService
    {
        CultureInfo GetCurrentCulture(
            string cultureParameterName,
            CultureInfo defaultCulture,
            out string cultureKey);

        CultureInfo GetCurrentCulture(
            RouteValueDictionary routeValues,
            string cultureParameterName,
            CultureInfo defaultCulture,
            out string cultureKey);
    }
}
