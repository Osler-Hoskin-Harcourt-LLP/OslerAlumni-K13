using CMS;
using CMS.DataEngine;
using CMS.MacroEngine;
using ECA.Mvc.PageURL.Macros;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Mvc.Core.Modules;

[assembly: RegisterModule(typeof(MacroRegistrationModule))]

namespace OslerAlumni.Mvc.Core.Modules
{
    public class MacroRegistrationModule
        : Module
    {
        public MacroRegistrationModule()
            : base($"{GlobalConstants.ModulePrefix}.MacroRegistrationModule")
        { }

        protected override void OnInit()
        {
            base.OnInit();

            MacroContext.GlobalResolver.SetNamedSourceData(
                PageUrlMacroNamespace.Name,
                PageUrlMacroNamespace.Instance);
        }
    }
}
