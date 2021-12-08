using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace ECA.Core.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterImplementations<TType>(
            this ContainerBuilder builder,
            Assembly[] assemblies,
            string suffix = null,
            Type[] excludedTypes = null)
            where TType : class
        {
            return builder?
                .RegisterAssemblyTypes(assemblies)
                .Where(t =>
                    t.IsClass
                    && !t.IsGenericType
                    && !t.IsAbstract
                    && ((excludedTypes == null) || excludedTypes.All(et => !et.IsAssignableFrom(t)))
                    && (string.IsNullOrWhiteSpace(suffix) || t.Name.EndsWith(suffix))
                    && (t.GetInterface(typeof(TType).Name) != null))
                .AsImplementedInterfaces(typeof(TType));
        }
    }
}
