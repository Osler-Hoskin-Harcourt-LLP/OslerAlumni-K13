using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Services;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// This attribute should only be used for internal server APIs.
    /// see: http://www.ryadel.com/en/http-basic-authentication-asp-net-mvc-using-custom-actionfilter/#comment-2507605761
    /// </summary>
    public class BasicAuthorizeAttribute
        : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {

        public string BasicRealm => "osler-realm";
        protected string Username { get; set; }
        protected string Password { get; set; }

        private string ApiKey
        {
            get
            {
                var configuration = CMS.Core.Service.Resolve<IConfiguration>();
                return configuration[GlobalConstants.Config.BasicAuthenticationAPIKey];

            }
        }
        
        public BasicAuthorizeAttribute()
        {

        }

        public BasicAuthorizeAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {

            EnsureUserNameAndPassword();

            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"].ToString();
            if (!String.IsNullOrEmpty(auth))
            {
                var cred = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };
                if (user.Name == Username && user.Pass == Password) return;
            }

            filterContext.HttpContext.Response.Headers.Add("WWW-Authenticate", $"Basic realm=\"{BasicRealm}\"");
            
            filterContext.Result = new UnauthorizedResult();
        }

        private void EnsureUserNameAndPassword()
        {
            //Use Default if username/password not specificed.

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                var cred = ApiKey.Split(':');

                Username = cred[0];
                Password = cred[1];
            }
        }
    }
}
