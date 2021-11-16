using System.Globalization;
using System.Web.Routing;
using ECA.Core.Services;

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
