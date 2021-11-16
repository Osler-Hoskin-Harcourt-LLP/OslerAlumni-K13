using System.Web.Http;
using OslerAlumni.Mvc.Api.Attributes.Error;

namespace OslerAlumni.Mvc.Api.Controllers
{
    [HandleWebApiException]
    public abstract class BaseApiController
        : ApiController
    {
    }
}
