using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using CMS.DocumentEngine;
using ECA.Core.Definitions;
using ECA.Core.Repositories;
using ECA.Mvc.PageURL.Models;
using ECA.PageURL.Kentico.Models;
using ECA.PageURL.Services;

using ICultureService = ECA.Mvc.Core.Services.ICultureService;

namespace ECA.Mvc.PageURL.Constraints
{
    public class PageUrlConstraint
        : IRouteConstraint
    {
        #region "Private fields"

        private readonly IDependencyResolver _diResolver;
        private readonly PageUrlConstraintSettings _constraintSettings;

        #endregion

        #region "Properties"

        protected ISettingsKeyRepository SettingsKeyRepository { get; set; }

        protected ICultureService CultureService { get; set; }

        protected IPageService PageService { get; set; }

        protected IPageUrlService PageUrlService { get; set; }

        #endregion

        public PageUrlConstraint(
            IDependencyResolver diResolver,
            PageUrlConstraintSettings constraintSettings)
        {
            _diResolver = diResolver;

            _constraintSettings = constraintSettings;
        }

        #region "Methods"

        public bool Match(
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var scope = ResolveDependencies();

            try
            {
                var urlPath =
                    values[parameterName]?.ToString();

                var urlItem = GetUrlItem(
                    urlPath,
                    values);

                // If we weren't able to match the URL, let it continue to other routes down the line
                if (urlItem == null)
                {
                    return false;
                }

                // If the URL is not the main URL of the page and is set to redirect to the main URL,
                // we should trigger the redirect
                if (RequiresRedirect(urlItem))
                {
                    TriggerRedirect(
                        httpContext,
                        values,
                        urlItem);

                    return true;
                }

                var page = GetPage(
                    urlItem);

                if ((page == null) || !PageUrlService.IsNavigable(page))
                {
                    return false;
                }

                TransferToAction(
                    httpContext,
                    values,
                    page);

                // Even if the transfer to the target controller/action fails,
                // the constraint should match to the route (by default a PageNotFound will be returned)
                return true;
            }
            finally
            {
                // NOTE: It is unclear if this is still the case, but there are posts out there that claim 
                // that the Autofac lifetime scope in which you resolve components (dependencies) has to stay alive
                // while you are still using those resolved instances. Which is why we are
                // taking responsibility over the scope disposal in this way.
                scope?.Dispose();
            }
        }

        #endregion

        #region "Helper methods"

        protected TreeNode GetPage(
            CustomTable_PageURLItem urlItem)
        {
            TreeNode page;

            if (PageService.TryGetUrlItemPage(
                    urlItem,
                    out page,
                    // This page is passed into the controller and we don't know how it is going to be used,
                    // i.e. which fields the particular controller/view is going to require,
                    // so we request all of them
                    includeAllCoupledColumns: true))
            {
                return page;
            }

            return null;
        }

        protected bool RequiresRedirect(
            CustomTable_PageURLItem urlItem)
        {
            if ((urlItem == null) || urlItem.IsMainURL)
            {
                return false;
            }

            return false;
            // TODO##
            //switch (urlItem.RedirectTypeEnum)
            //{
            //    case AliasActionModeEnum.UseSiteSettings:
            //        return SettingsKeyRepository.GetValue<bool>(
            //            ECAGlobalConstants.Settings.RedirectAliasesToMainUrl);

            //    case AliasActionModeEnum.RedirectToMainURL:
            //        return true;

            //    default:
            //        return false;
            //}
        }

        protected ILifetimeScope ResolveDependencies()
        {
            // NOTE: This is not ideal, since we are creating a direct dependency on Autofac as the DI container.
            // NOTE: Autofac tries to resolve the interface using a request lifetime, if no other lifetime is present,
            // which will not work if there is no request context (e.g. when a new thread has been spun out)
            // or if the request lifetime scope has already been disposed of (e.g. when we are in OWIN pipeline as it is
            // trying to redirect the user to a login page URL, after a failed authorization check. MVC and OWIN do not have 
            // a shared pipeline, which would result in an exception being thrown, since request lifetime scope would have
            // been already descoped). Hence we are creating our own Autofac lifetime,
            // so that it is able to resolve the dependencies for us.
            var scope = 
                ((AutofacDependencyResolver)_diResolver).ApplicationContainer.BeginLifetimeScope();

            SettingsKeyRepository = scope.Resolve<ISettingsKeyRepository>();
            CultureService = scope.Resolve<ICultureService>();
            PageService = scope.Resolve<IPageService>();
            PageUrlService = scope.Resolve<IPageUrlService>();

            return scope;
        }

        protected CustomTable_PageURLItem GetUrlItem(
            string urlPath,
            RouteValueDictionary routeValues)
        {
            string cultureKey;

            var culture = CultureService.GetCurrentCulture(
                routeValues,
                _constraintSettings.CultureRouteParameter,
                _constraintSettings.DefaultCulture,
                out cultureKey);

            return PageUrlService.GetUrlItem(
                urlPath,
                culture.Name);
        }

        protected void TransferToAction(
            HttpContextBase httpContext,
            RouteValueDictionary values,
            TreeNode page)
        {
            if (page == null)
            {
                return;
            }

            var controllerName =
                page.GetStringValue(_constraintSettings.ControllerFieldName, null);

            var actionName =
                page.GetStringValue(_constraintSettings.ActionFieldName, null);

            if (string.IsNullOrWhiteSpace(controllerName)
                || string.IsNullOrWhiteSpace(actionName))
            {
                return;
            }

            values[_constraintSettings.ControllerRouteParameter] =
                controllerName;

            values[_constraintSettings.ActionRouteParameter] =
                actionName;

            // IMPORTANT: In order for the TreeNode page to be properly bound to
            // a strongly-typed action parameter, you need to add the strongly-typed page type
            // to the list of page model binders in ApplicationConfig.RegisterModelBinders()
            values[_constraintSettings.PageRouteParameter] =
                page;
        }

        protected void TriggerRedirect(
            HttpContextBase httpContext,
            RouteValueDictionary values,
            CustomTable_PageURLItem urlItem)
        {
            values[_constraintSettings.ControllerRouteParameter] =
                _constraintSettings.RedirectControllerName;

            values[_constraintSettings.ActionRouteParameter] =
                _constraintSettings.MainUrlRedirectActionName;

            values[_constraintSettings.UrlItemRouteParameter] =
                urlItem;
        }

        #endregion
    }
}
