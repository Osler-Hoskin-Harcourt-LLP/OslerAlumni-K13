using CMS.Base;
using CMS.MacroEngine;
using ECA.Core.Definitions;

namespace ECA.Mvc.PageURL.Macros
{
    // NOTE: You should use both Extension on the namespace and RegisterExtension
    // registration on the macro method container class iteself:
    // - Without RegisterExtension registration, macros don't work on MVC site.
    [Extension(typeof(PageUrlMacroMethods))]

    public class PageUrlMacroNamespace
        : MacroNamespace<PageUrlMacroNamespace>
    {
        /// <summary>
        /// Name of the custom macro namespace that will be used as
        /// the prefix for all fields and methods defined in its
        /// extension classes.
        /// </summary>
        /// <remarks>
        /// The name cannot contain periods or special characters,
        /// i.e. cannot use dot-notation.
        /// </remarks>
        public static string Name
            => ECAGlobalConstants.Macros.PageUrlNamespace;
    }
}
