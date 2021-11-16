using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    /// <summary>
    /// This attribute should only be used for internal server APIs.
    /// see: http://www.ryadel.com/en/http-basic-authentication-asp-net-mvc-using-custom-actionfilter/#comment-2507605761
    /// </summary>
    public class BasicAuthorizeAttribute
        : ActionFilterAttribute
    {
        public IConfigurationService ConfigurationService { get; set; }

        public string BasicRealm => "osler-realm";
        protected string Username { get; set; }
        protected string Password { get; set; }

        private string ApiKey => ConfigurationService
            .GetWebConfigSetting<string>
                (GlobalConstants.Config.BasicAuthenticationAPIKey);

        public BasicAuthorizeAttribute()
        {

        }

        public BasicAuthorizeAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            EnsureUserNameAndPassword();

            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!String.IsNullOrEmpty(auth))
            {
                var cred = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };
                if (user.Name == Username && user.Pass == Password) return;
            }

            filterContext.HttpContext.Response.AddHeader("WWW-Authenticate", $"Basic realm=\"{BasicRealm}\"");
            
            filterContext.Result = new HttpUnauthorizedResult();
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
