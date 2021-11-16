using CMS.Base;
using CMS.MacroEngine;
using ECA.Core.Definitions;

namespace ECA.Admin.PageURL.Macros
{
    // NOTE: You have to use both Extension on the namespace and RegisterExtension
    // registration on the macro method container class iteself:
    // - Without Extension registration, macros don't work on Admin site;
    // - Without RegisterExtension, macro methods load too late.
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
