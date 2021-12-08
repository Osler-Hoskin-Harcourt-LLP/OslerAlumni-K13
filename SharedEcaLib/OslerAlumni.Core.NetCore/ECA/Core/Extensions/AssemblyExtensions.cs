using System.Linq;
using System.Reflection;

namespace ECA.Core.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Checks if the full name of the assembly starts with any of the provided prefixes.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="assemblyNamePrefixes"></param>
        /// <returns></returns>
        public static bool NameStartsWith(
            this AssemblyName assembly,
            string[] assemblyNamePrefixes)
        {
            var fullName = assembly?.FullName;

            return !string.IsNullOrWhiteSpace(fullName)
                   && assemblyNamePrefixes.Any(fullName.StartsWith);
        }

        /// <summary>
        /// Checks if the full name of the assembly starts with any of the provided prefixes.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="assemblyNamePrefixes"></param>
        /// <returns></returns>
        public static bool NameStartsWith(
            this Assembly assembly,
            string[] assemblyNamePrefixes)
        {
            var fullName = assembly?.FullName;

            return !string.IsNullOrWhiteSpace(fullName)
                   && assemblyNamePrefixes.Any(fullName.StartsWith);
        }
    }
}
