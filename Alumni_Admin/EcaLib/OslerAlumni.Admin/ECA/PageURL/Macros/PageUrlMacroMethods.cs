using Autofac;
using CMS;
using ECA.Admin.Core.Extensions;
using ECA.Admin.PageURL.Macros;
using ECA.PageURL.Macros;

// NOTE: You have to use both Extension on the namespace and RegisterExtension
// registration on the macro method container class iteself:
// - Without Extension registration, macros don't work on Admin site;
// - Without RegisterExtension, macro methods load too late.
[assembly: RegisterExtension(typeof(PageUrlMacroMethods), typeof(PageUrlMacroNamespace))]

namespace ECA.Admin.PageURL.Macros
{
    public class PageUrlMacroMethods
        : PageUrlMacroMethodsBase
    {
        #region "Helper methods"

        protected override ILifetimeScope ResolveDependencies()
        {
            return this.InjectDependencies();
        }

        #endregion
    }
}
