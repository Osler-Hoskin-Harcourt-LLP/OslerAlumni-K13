using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CMS.DocumentEngine;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Core.Attributes.ActionFilters
{
    public class OslerAuthorizeAttribute
        : AuthorizeAttribute
    {
        #region "Private fields"

        private readonly string _accountControllerName;
        private readonly string _logOutActionName;

        #endregion

        #region "Properties"

        public IIpLocatorService IpLocatorService { get; set; }
        public IUserRepository UserRepository { get; set; }

        #endregion

        public OslerAuthorizeAttribute(
            string accountControllerName = "Account",
            string logOutActionName = "LogOut")
        {
            _accountControllerName = accountControllerName;
            _logOutActionName = logOutActionName;
        }

        #region "Events"

        public override void OnAuthorization(
            AuthorizationContext filterContext)
        {
            var routeValues = filterContext.RouteData.Values;

            var page = routeValues[Constants.RouteParams.Page] as TreeNode;

            //Do not check for authorization for public pages.
            if (PageIsPublic(page))
            {
               return;
            }

            base.OnAuthorization(filterContext);

            // If it has already been determined that the visitor is not authorized,
            // no additional logic is required
            if (filterContext.Result?.GetType() == typeof(HttpUnauthorizedResult))
            {
                return;
            }

            // If this is already a request for logout, 
            // no need for additional logic,
            // which would result in circular redirects
            if (string.Equals(
                    routeValues["controller"]?.ToString(),
                    _accountControllerName, 
                    StringComparison.OrdinalIgnoreCase)
                && string.Equals(
                    routeValues["action"]?.ToString(), 
                    _logOutActionName, 
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }


            // If out-of-the-box code determined that the visitor is authorized,
            // we need to also make sure that their user still exists and is still enabled
            if (IsVerifiedCurrentUser(filterContext.HttpContext))
            {
                //if the User is not logged in as System User, return
                if (!UserRepository.IsSystemUser(UserRepository.CurrentUser?.UserName))
                {
                    return;
                }

                //if the User is logged in as System User and is not part of the osler network ip log them out
                if (IpLocatorService.IsCurrentUserInOslerNetwork())
                {
                    return;
                }

            }

            // If the site thinks the visitor is authenticated,
            // but their user doesn't exist anymore or had been disabled,
            // we need to log them out, i.e. clear their authentication cookie
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = _accountControllerName,
                        action = _logOutActionName
                    }));
        }

        protected override HttpValidationStatus OnCacheAuthorization(
            HttpContextBase httpContext)
        {
            var status = base.OnCacheAuthorization(httpContext);

            // If it has already been determined that the visitor
            // is not authorized to access cache storage,
            // no additional logic is required
            if (status != HttpValidationStatus.Valid)
            {
                return status;
            }

            // If out-of-the-box code determined that the visitor is authorized,
            // we need to also make sure that their user still exists and is still enabled
            return IsVerifiedCurrentUser(httpContext)
                ? status
                : HttpValidationStatus.Invalid;
        }

        #endregion

        #region "Methods"

        protected bool IsVerifiedCurrentUser(
            HttpContextBase httpContext)
        {
            // If the user is not authenticated, there is nothing else to do
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }

            // Otherwise, check that the current user still exists and is still enabled
            var currentUser = UserRepository.CurrentUser;

            return currentUser?.Enabled ?? false;
        }


        private bool PageIsPublic(TreeNode page)
        {
            return page is IPublicType && ((IPublicType) page).IsPublic;
        }

        #endregion
    }
}
