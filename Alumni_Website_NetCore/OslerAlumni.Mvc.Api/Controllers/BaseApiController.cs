using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using OslerAlumni.Mvc.Api.Attributes.Error;

namespace OslerAlumni.Mvc.Api.Controllers
{
    [HandleWebApiException]
    public abstract class BaseApiController
        : Controller
    {
    }
}
