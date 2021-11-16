using CMS;
using ECA.Admin.Core.Modules;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Infrastructure;
using OslerAlumni.Core.Services;

[assembly: RegisterModule(typeof(CustomFileProviderModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    /// <summary>
    /// This module registers a file provider.
    /// This is needed because we want a common file upload path for the Admin and the MVC site.
    /// </summary>
    public class CustomFileProviderModule
        : BaseModule
    {
        #region "Properties"

        public IConfigurationService ConfigurationService { get; set; }

        #endregion

        /// <summary>
        /// Module class constructor, the system registers the module under the name "CustomFileProvider".
        /// </summary>
        public CustomFileProviderModule()
            : base($"{GlobalConstants.ModulePrefix}.{nameof(CustomFileProviderModule)}")
        {
        }

        /// <summary>
        /// Contains initialization code that is executed when the application starts.
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            
            var rootMediaStoragePath = ConfigurationService
                .GetWebConfigSetting<string>(
                    GlobalConstants.Config.RootMediaStoragePath);

            OslerAlumniStorageProvider.RegisterProvider(
                rootMediaStoragePath);
        }
    }
}
