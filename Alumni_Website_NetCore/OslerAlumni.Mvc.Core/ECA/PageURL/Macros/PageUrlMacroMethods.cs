using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using CMS;
using ECA.Mvc.PageURL.Macros;
using ECA.PageURL.Macros;
using ECA.PageURL.Services;

// NOTE: You should use both Extension on the namespace and RegisterExtension
// registration on the macro method container class iteself:
// - Without RegisterExtension registration, macros don't work on MVC site.
[assembly: RegisterExtension(typeof(PageUrlMacroMethods), typeof(PageUrlMacroNamespace))]

namespace ECA.Mvc.PageURL.Macros
{
    public class PageUrlMacroMethods
        : PageUrlMacroMethodsBase
    {
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

        #region "Helper methods"

        protected override ILifetimeScope ResolveDependencies()
        {
            // NOTE: This is not ideal, since we are creating a direct dependency on Autofac as the DI container.
            // NOTE: Autofac tries to resolve the interface using a request lifetime, if no other lifetime is present,
            // which will not work if there is no request context (e.g. when Kentico spins out a separate thread,
            // for instance, for an event listener that is listening to a document publish event)
            // or if the request lifetime scope has already been disposed of (e.g. when we are in OWIN pipeline as it is
            // trying to redirect the user to a login page URL, after a failed authorization check. MVC and OWIN do not have 
            // a shared pipeline, which would result in an exception being thrown, since request lifetime scope would have
            // been already descoped). Hence we are creating our own Autofac lifetime,
            // so that it is able to resolve the dependencies for us.
            var appContainer =
                ((AutofacDependencyResolver)DIResolver).ApplicationContainer;

            // NOTE: In cases when request context is not available, we have to create our own Autofac lifetime,
            // so that it is able to resolve the dependencies for us
            var scope = appContainer.BeginLifetimeScope();

            PageUrlService = scope.Resolve<IPageUrlService>();

            return scope;
        }

        #endregion
    }
}
