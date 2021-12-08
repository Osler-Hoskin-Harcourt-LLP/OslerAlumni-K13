using ECA.Core.Models;

namespace OslerAlumni.Mvc.Api.Models
{
    public class ApiConfig 
        : IConfig
    {
        public bool EnableSwagger { get; set; }
    }
}
