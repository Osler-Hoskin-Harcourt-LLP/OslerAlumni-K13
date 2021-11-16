using System.Reflection;
using System.Web.Http;
using CMS.Base;
using OslerAlumni.Admin.Core.Infrastructure;

/// <summary>
/// Application methods.
/// </summary>
public class Global
    : AutofacHttpApplication
{
    #region "Methods"

    static Global()
    {
#if DEBUG
        // Set debug mode based on current web project build configuration
        SystemContext.IsWebProjectDebug = true;
#endif

        // Initialize CMS application. This method should not be called from custom code.
        InitApplication(Assembly.GetExecutingAssembly());
    }

    #endregion

    #region "Custom methods"

    protected void Application_Start()
    {
        GlobalConfiguration.Configure(
            config => ConfigureDependencies(config));
    }

    #endregion
}