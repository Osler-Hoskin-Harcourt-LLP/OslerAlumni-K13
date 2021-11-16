using System.Reflection;
using System.Web.Http;
using Autofac;

namespace ECA.Core.Infrastructure
{
    public interface IBootstrapItem
    {
        void ConfigureDependencies(
            ContainerBuilder builder,
            HttpConfiguration httpConfiguration = null,
            params Assembly[] assemblies);
    }
}
