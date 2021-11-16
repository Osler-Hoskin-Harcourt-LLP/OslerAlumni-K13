using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.WebApi;

namespace ECA.Admin.Core.Extensions
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Creates and returns an Autofac lifetime scope, that is nested either
        /// in the request (if request context is available) or in the root lifetime scope.
        /// Resolves all of the public properties of the class within the newly created scope.
        /// NOTE 1: The scope HAS TO stay alive while the resolved properties are being used.
        /// NOTE 2: The scope HAS TO be manualy disposed of, once it is safe to do so.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ILifetimeScope InjectDependencies<T>(
            this T obj)
            where T : class
        {
            if (obj == null)
            {
                return null;
            }

            // NOTE: This is not ideal, since we are creating a direct dependency on Autofac as the DI container
            ILifetimeScope parentScope;

            var context = HttpContext.Current;

            if (context == null)
            {
                var diResolver = GlobalConfiguration.Configuration.DependencyResolver;

                parentScope = diResolver.GetRequestLifetimeScope() ?? diResolver.GetRootLifetimeScope();
            }
            else
            {
                var cpa = (IContainerProviderAccessor) context.ApplicationInstance;
                var cp = cpa.ContainerProvider;

                parentScope = cp.RequestLifetime;
            }

            // NOTE: Autofac tries to resolve the interface using a request lifetime, if no other lifetime is present,
            // which will not work if there is no request context (e.g. when Kentico spins out a separate thread,
            // for instance, for an event listener that is listening to a document publish event)
            // or if the request lifetime scope has already been disposed of (e.g. when we are in OWIN pipeline as it is
            // trying to redirect the user to a login page URL, after a failed authorization check. MVC and OWIN do not have 
            // a shared pipeline, which would result in an exception being thrown, since request lifetime scope would have
            // been already descoped). Hence we are creating our own Autofac lifetime,
            // so that it is able to resolve the dependencies for us.
            var scope = parentScope?.BeginLifetimeScope();

            // Resolves dependencies in public properties
            scope?.InjectProperties(obj);

            return scope;
        }
    }
}
