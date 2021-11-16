using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web;
using CMS.Helpers;
using ECA.Core.Models;
using ECA.PageURL.Definitions;
using ECA.PageURL.Services;
using Kentico.Membership;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Infrastructure;
using Owin;

using ICultureService = ECA.Mvc.Core.Services.ICultureService;
using Constants = OslerAlumni.Mvc.Core.Definitions.Constants;

// Assembly attribute that sets the OWIN startup class
[assembly: OwinStartup(typeof(Startup))]

namespace OslerAlumni.Mvc
{
    public partial class Startup
    {
        #region "Constants"

        // Cookie name prefix used by OWIN when creating authentication cookies
        private const string OWIN_COOKIE_PREFIX = ".AspNet.";

        #endregion

        #region "Private fields"
        
        private static readonly object _lockDIResolver = new object();

        private static IDependencyResolver _diResolver;

        #endregion

        #region "Properties"

        public static IDependencyResolver DIResolver
        {
            get
            {
                lock (_lockDIResolver)
                {
                    return _diResolver;
                }
            }
            set
            {
                lock (_lockDIResolver)
                {
                    _diResolver = value;
                }
            }
        }

        #endregion

        #region "Methods"

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(
                () => MvcBootstrapItem.GetCurrentContext());

            app.CreatePerOwinContext(
                () => DIResolver.GetService<ICultureService>());

            app.CreatePerOwinContext(
                () => DIResolver.GetService<IPasswordPolicyService>());

            app.CreatePerOwinContext(
                () => DIResolver.GetService<IPageUrlService>());

            // Registers the Kentico.Membership identity implementation
            app.CreatePerOwinContext(
                (IdentityFactoryOptions<UserManager> factoryOptions, IOwinContext owinContext) =>
                {
                    var context =
                        owinContext.Get<ContextConfig>();

                    var passwordPolicyService =
                        owinContext.Get<IPasswordPolicyService>();

                    var userManager = UserManager.Initialize(app,
                        new UserManager(new UserStore(context.Site?.SiteName)));

                    userManager.PasswordValidator =
                        passwordPolicyService.PasswordValidator;

                    return userManager;
                });

            app.CreatePerOwinContext<SignInManager>(
                SignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/"),
                ReturnUrlParameter = Constants.RouteParams.ReturnUrl,
                Provider = new CookieAuthenticationProvider
                {
                    OnApplyRedirect = ApplyUnauthorizedRedirect
                }
            });

            // Registers the authentication cookie with the 'Essential' cookie level
            // Ensures that the cookie is preserved when changing a visitor's allowed cookie level below 'Visitor'
            CookieHelper.RegisterCookie(
                OWIN_COOKIE_PREFIX + DefaultAuthenticationTypes.ApplicationCookie,
                CookieLevel.Essential);
        }

        #endregion

        #region "Helper methods"

        protected void ApplyUnauthorizedRedirect(
            CookieApplyRedirectContext redirectContext)
        {
            var request = redirectContext.Request;

            // Do not redirect on Web API routes
            if (WebApiConfig.IsApiRoute(request.Path.Value))
            {
                return;
            }

            var response = redirectContext.Response;

            var owinContext = redirectContext.OwinContext;

            var cultureService = owinContext.Get<ICultureService>();
            var pageUrlService = owinContext.Get<IPageUrlService>();

            // IMPORTANT: We cannot use the ContextConfig.CultureName here,
            // because this callback function is going to run in a different thread,
            // in which the culture will NOT be set based on route.
            var defaultCulture = new CultureInfo(
                GlobalConstants.Cultures.Default);

            string cultureKey;

            var currentCulture =
                cultureService
                    .GetCurrentCulture(
                        Constants.RouteParams.Culture,
                        defaultCulture,
                        out cultureKey)
                ?? defaultCulture;

            string url;

            if (!pageUrlService.TryGetPageMainUrl(
                    StandalonePageType.Login,
                    currentCulture.Name,
                    out url))
            {
                response.Redirect(redirectContext.RedirectUri);

                return;
            }

            var redirectUri = new Uri(redirectContext.RedirectUri);

            response.Redirect(url + redirectUri.Query + GetUtmParameters(redirectUri));
        }

        protected string GetUtmParameters(Uri redirectUri)
        {
            // Decode the redirectUri.Query to see if there are any UTM params in it so that they can be passed to the login page
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(redirectUri.Query))
                return string.Empty;

            var decodedReturnUrl = HttpUtility.UrlDecode(redirectUri.Query).Trim('?').Replace("?", "&");

            var uri = new Uri($"{redirectUri.Scheme}://{redirectUri.Host}?{decodedReturnUrl}");

            var returnUrl = HttpUtility.ParseQueryString(uri.Query).Get(Constants.RouteParams.ReturnUrl);

            if (string.IsNullOrEmpty(returnUrl))
                return string.Empty;

            var utmSource = HttpUtility.ParseQueryString(uri.Query).Get(Constants.RouteParams.UtmSource);
            var utmCampaign = HttpUtility.ParseQueryString(uri.Query).Get(Constants.RouteParams.UtmCampaign);
            var utmMedium = HttpUtility.ParseQueryString(uri.Query).Get(Constants.RouteParams.UtmMedium);
            var utmTerm = HttpUtility.ParseQueryString(uri.Query).Get(Constants.RouteParams.UtmTerm);
            var utmContent = HttpUtility.ParseQueryString(uri.Query).Get(Constants.RouteParams.UtmContent);

            if (!string.IsNullOrEmpty(utmSource))
            {
                sb.Append($"&{Constants.RouteParams.UtmSource}={utmSource}");
            }

            if (!string.IsNullOrEmpty(utmCampaign))
            {
                sb.Append($"&{Constants.RouteParams.UtmCampaign}={utmCampaign}");
            }

            if (!string.IsNullOrEmpty(utmMedium))
            {
                sb.Append($"&{Constants.RouteParams.UtmMedium}={utmMedium}");
            }

            if (!string.IsNullOrEmpty(utmTerm))
            {
                sb.Append($"&{Constants.RouteParams.UtmTerm}={utmTerm}");
            }

            if (!string.IsNullOrEmpty(utmContent))
            {
                sb.Append($"&{Constants.RouteParams.UtmContent}={utmContent}");
            }

            return sb.ToString();
        }

        #endregion
    }
}
