using CMS;
using CMS.MacroEngine;
using ECA.Admin.Core.Modules;
using ECA.Admin.PageURL.Macros;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;

[assembly: RegisterModule(typeof(MacroRegistrationModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    public class MacroRegistrationModule 
        : BaseModule
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
