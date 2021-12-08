using System;
using System.Linq;
using System.Reflection;

namespace ECA.Core.Extensions
{
    public static class AppDomainExtensions
    {
        public static Assembly[] GetCustomAssemblies(
            this AppDomain appDomain,
            string[] assemblyNamePrefixes)
        {
            // Once we made sure all necessary assemblies are loaded, we can just scan the app domain
            // IMPORTANT: The reason for pulling them from AppDomain instead of simply using the list we got above,
            // is because in the list above, the assemblies and their types are marked as "Runtime" assemblies and types
            // and Autofac is not able to scan them properly
            var assemblies = appDomain
                .GetAssemblies()
                .Where(assembly =>
                    assembly.NameStartsWith(assemblyNamePrefixes))
                .ToArray();

            // IMPORTANT: ASP.NET does NOT load all of the referenced assemblies at startup
            // (it automatically determines which ones are needed, e.g. which ones contain
            // explicit references, and only loads those),
            // which is why this step is necessary, so that we can force all of our relevant
            // assemblies to load, before registering a dependency container
            var referencedAssemblies = assemblies
                .SelectMany(assembly =>
                    assembly
                        .GetReferencedAssemblies()
                        .Where(refAssembly =>
                            refAssembly.NameStartsWith(assemblyNamePrefixes))
                        .Select(refAssembly =>
                            Assembly.Load(refAssembly.FullName)))
                .ToArray();

            return assemblies
                .Union(referencedAssemblies)
                .Distinct()
                .ToArray();
        }
    }
}
