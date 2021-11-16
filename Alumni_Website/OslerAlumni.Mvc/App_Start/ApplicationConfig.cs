using System.Web.Mvc;
using ECA.Mvc.PageURL.Macros;
using Kentico.Activities.Web.Mvc;
using Kentico.CampaignLogging.Web.Mvc;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Newsletters.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Newtonsoft.Json;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Infrastructure;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Extensions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Services;

namespace OslerAlumni.Mvc
{
    public class ApplicationConfig
    {
        public static void ConfigureDefaults()
        {
            // Set Default Serialization Options
            JsonConvert.DefaultSettings = () =>
                new JsonSerializerSettings
                {
                    // Can add default serialization settings here
                    DateFormatString = Constants.DateTimeFormat.LongDate,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                };
        }

        public static void RegisterFeatures(
            ApplicationBuilder builder)
        {
            //enable content tree based routing
            builder.UsePageRouting();

            // Enables the activity tracking feature
            builder.UseActivityTracking();

            builder.UseEmailTracking(new EmailTrackingOptions());

            // Override Data Annotiations to use Kentico Resource Strings.
            builder.UseDataAnnotationsLocalization();

            builder.UseCampaignLogger();
        }

        public static void RegisterCustomComponents(
            IDependencyResolver diResolver)
        {
            var configurationService =
                diResolver.GetService<IConfigurationService>();

            var rootMediaStoragePath = configurationService
                .GetWebConfigSetting<string>(
                    GlobalConstants.Config.RootMediaStoragePath);

            OslerAlumniStorageProvider.RegisterProvider(
                rootMediaStoragePath);
        }

        public static void RegisterModelBinders()
        {
            ModelBinders.Binders
                .AddPageModelBinder<PageType_DevelopmentResource>()
                .AddPageModelBinder<PageType_Event>()
                .AddPageModelBinder<PageType_Home>()
                .AddPageModelBinder<PageType_Job>()
                .AddPageModelBinder<PageType_LandingPage>()
                .AddPageModelBinder<PageType_News>()
                .AddPageModelBinder<PageType_Page>()
                .AddPageModelBinder<PageType_Profile>()
                .AddPageModelBinder<PageType_Resource>();
        }
    }
}
