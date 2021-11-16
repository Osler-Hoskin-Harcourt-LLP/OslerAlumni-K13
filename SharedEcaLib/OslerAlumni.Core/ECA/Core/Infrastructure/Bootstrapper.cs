using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;

namespace ECA.Core.Infrastructure
{
    /// <remarks>
    /// This is based on the code orginally written by Philip Proplesch @ ecentricarts.
    /// </remarks>
    public class Bootstrapper
    {
        public static void Bootstrap(
            ContainerBuilder builder,
            HttpConfiguration httpConfiguration = null,
            params Assembly[] assemblies)
        {
            var bootstrapItemType = typeof(IBootstrapItem);

            var bootstrapItems = new List<Type>();

            foreach (var assembly in assemblies)
            {
                bootstrapItems.AddRange(
                    assembly
                        .GetTypes()
                        .Where(type =>
                            !type.IsInterface &&
                            bootstrapItemType.IsAssignableFrom(type)));
            }

            foreach (var type in bootstrapItems)
            {
                var bootstrapItem = (IBootstrapItem)Activator.CreateInstance(type);

                bootstrapItem.ConfigureDependencies(
                    builder, 
                    httpConfiguration, 
                    assemblies);
            }
        }
    }
}
