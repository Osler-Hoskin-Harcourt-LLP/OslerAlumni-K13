using Autofac;
using ECA.Core.Infrastructure;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Api.Models;
using System.Reflection;
using System.Web.Http;

namespace OslerAlumni.Mvc.Api.Infrastructure
{
    public class WebApiBootstrapItem
        : IBootstrapItem
    {
        public void ConfigureDependencies(
            ContainerBuilder builder,
            HttpConfiguration httpConfiguration = null,
            params Assembly[] assemblies)
        {
            builder
                .RegisterApiControllers(
                    assemblies)
                .WithParameter(
                    (parameter, context) =>
                        parameter.ParameterType == typeof(SearchConfig),
                    (parameter, context) =>
                        context.Resolve<IConfigurationService>().GetConfig<SearchConfig>());

            // Note: not checking for httpConfiguration being null,
            // so that the error does get thrown
            builder.RegisterWebApiFilterProvider(
                httpConfiguration);
        }
    }
}
