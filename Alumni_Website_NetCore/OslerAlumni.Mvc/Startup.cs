using CMS.DataEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using ECA.Caching.Services;
using ECA.Content.Repositories;
using ECA.Core.Models;
using ECA.Core.Repositories;
using ECA.Mvc.Navigation.Repositories;
using ECA.Mvc.Navigation.Services;
using ECA.PageURL.Repositories;
using ECA.PageURL.Services;
using Kentico.Activities.Web.Mvc;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Membership;
using Kentico.Newsletters.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Infrastructure;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Constraints;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Models;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Extensions.OslerControls.RichTextField.Models.Helpers;
using OslerAlumni.OnePlace.Factories;
using OslerAlumni.OnePlace.Models;
using OslerAlumni.OnePlace.Repositories;
using OslerAlumni.OnePlace.Services;
using System;
using System.Linq;

namespace BlankSiteCore
{
    public class Startup
    {
        private const string AUTHENTICATION_COOKIE_NAME = "identity.authentication";
        public readonly string HttpErrorsControllerName = nameof(HttpErrorsController);

        public readonly string NotFoundActionName = nameof(HttpErrorsController.NotFound);
        public IWebHostEnvironment Environment { get; }
        private IConfiguration Configuration { get; }


        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable desired Kentico Xperience features
            var kenticoServiceCollection = services.AddKentico(features =>
            {
                // features.UsePageBuilder();
                features.UseActivityTracking();
                // features.UseABTesting();
                // features.UseWebAnalytics();
                features.UseEmailTracking();
                // features.UseCampaignLogger();
                // features.UseScheduler();
                features.UsePageRouting();
            });

            if (Environment.IsDevelopment())
            {
                // By default, Xperience sends cookies using SameSite=Lax. If the administration and live site applications
                // are hosted on separate domains, this ensures cookies are set with SameSite=None and Secure. The configuration
                // only applies when communicating with the Xperience administration via preview links. Both applications also need 
                // to use a secure connection (HTTPS) to ensure cookies are not rejected by the client.
                kenticoServiceCollection.SetAdminCookiesSameSiteNone();

                // By default, Xperience requires a secure connection (HTTPS) if administration and live site applications
                // are hosted on separate domains. This configuration simplifies the initial setup of the development
                // or evaluation environment without a the need for secure connection. The system ignores authentication
                // cookies and this information is taken from the URL.
                kenticoServiceCollection.DisableVirtualContextSecurityForLocalhost();

            }
            // Adds Xperience services required by the system's Identity implementation
            services.AddScoped<IPasswordHasher<ApplicationUser>, Kentico.Membership.PasswordHasher<ApplicationUser>>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddApplicationIdentity<ApplicationUser, ApplicationRole>()
                // Adds token providers used to generate tokens for email confirmations, password resets, etc.
                .AddApplicationDefaultTokenProviders()
                // Adds an implementation of the UserStore for working with Xperience user objects
                .AddUserStore<ApplicationUserStore<ApplicationUser>>()
                // Adds an implementation of the RoleStore used for working with Xperience roles
                .AddRoleStore<ApplicationRoleStore<ApplicationRole>>()
                // Adds an implementation of the UserManager for Xperience membership
                .AddUserManager<ApplicationUserManager<ApplicationUser>>()
                // Adds the default implementation of the SignInManger
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddAuthentication();
            services.AddAuthorization();

            services.AddControllersWithViews();

            // Configures the application's authentication cookie
            services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = new PathString("/");
                c.ExpireTimeSpan = TimeSpan.FromDays(14);
                c.SlidingExpiration = true;
                c.Cookie.Name = AUTHENTICATION_COOKIE_NAME;
            });

            // Registers the authentication cookie in Xperience with the 'Essential' cookie level
            // Ensures that the cookie is preserved when changing a visitor's allowed cookie level below 'Visitor'
            CookieHelper.RegisterCookie(AUTHENTICATION_COOKIE_NAME, CookieLevel.Essential);

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = Configuration.GetValue<bool>("PasswordPolicyConfig:RequireDigit");
                options.Password.RequireLowercase = Configuration.GetValue<bool>("PasswordPolicyConfig:RequireLowercase");
                options.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("PasswordPolicyConfig:RequireNonLetterOrDigit");
                options.Password.RequireUppercase = Configuration.GetValue<bool>("PasswordPolicyConfig:RequireUppercase");
                options.Password.RequiredLength = Configuration.GetValue<int>("PasswordPolicyConfig:RequiredLength");
            });

            services.AddSwaggerGen();
            services.AddTransient(cc => new ContextConfig
            {
                CultureName = LocalizationContext.CurrentCulture.CultureCode,
                User = MembershipContext.AuthenticatedUser,
                Site = SiteContext.CurrentSite,
                IsPreviewMode = CMS.Core.Service.Resolve<IHttpContextAccessor>()?.HttpContext?.Kentico().GetFeature<IPreviewFeature>()?.Enabled ?? false,
                AllowedCultureCodes = GlobalConstants.Cultures.AllowedCultureCodes,
                BasePageType = PageType_BasePageType.CLASS_NAME
            });
            services.AddSingleton(ec => new EmailConfig()
            {
                NewUserAccountAlumniEmailTemplate =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.NewUserAccountAlumniEmailTemplate),
                PasswordResetEmailTemplate =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.PasswordResetEmailTemplate),
                PasswordResetConfirmationEmailTemplate =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.PasswordResetConfirmationEmailTemplate),
                SendEmailNotificationsFrom =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.SendEmailNotificationsFrom),
                ContactUsNotificationExternalEmailTemplate =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.ContactUsNotificationExternalEmailTemplate),
                ContactUsNotificationInternalEmailTemplate =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.ContactUsNotificationInternalEmailTemplate),
                ContactUsNotificationInternalEmailSendTo =
                    SettingsKeyInfoProvider.GetValue(GlobalConstants.Settings.EmailSettings.ContactUsNotificationInternalEmailSendTo)
            });
            services.AddSingleton(nc => new OslerNetworkConfig()
            {
                OslerIpAddresses = Configuration.GetValue<string>("OslerNetworkConfig:OslerIpAddresses"),
                LogAllIps = Configuration.GetValue<bool>("OslerNetworkConfig:LogAllIps")
            });
            services.AddSingleton(opc => new OnePlaceConfig()
            {
                Url = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.Url)}"),
                ApiVersion = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.ApiVersion)}"),
                ConsumerKey = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.ConsumerKey)}"),
                ConsumerSecret = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.ConsumerSecret)}"),
                Username = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.Username)}"),
                Password = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.Password)}"),
                SecurityToken = Configuration.GetValue<string>($"{nameof(OnePlaceConfig)}:{nameof(OnePlaceConfig.SecurityToken)}")
            });
            services.AddScoped<OslerAlumni.Mvc.Core.Services.IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IEventLogRepository, EventLogRepository>();
            services.AddSingleton<ISettingsKeyRepository, SettingsKeyRepository>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IGlobalAssetService, GlobalAssetService>();
            services.AddSingleton<IDocumentRelationshipRepository, DocumentRelationshipRepository>();
            services.AddSingleton<IDocumentRepository, DocumentRepository>();
            services.AddSingleton<IKenticoClassRepository, KenticoClassRepository>();
            services.AddSingleton<IPageUrlItemRepository, PageUrlItemRepository>();
            services.AddSingleton<IPageUrlService, PageUrlService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationItemRepository, NavigationItemRepository>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IBreadCrumbService, BreadCrumbService>();
            services.AddSingleton<IBoardOpportunityService, BoardOpportunityService>();
            services.AddSingleton<IEventsService, EventsService>();
            services.AddSingleton<IJobsService, JobsService>();
            services.AddSingleton<INewsService, NewsService>();
            services.AddSingleton<ICustomTableRepository, CustomTableRepository>();
            services.AddSingleton<IResourceTypeItemRepository, ResourceTypeItemRepository>();
            services.AddSingleton<IResourceService, ResourceService>();
            services.AddSingleton<IHomeService, HomeService>();
            services.AddSingleton<InlineWidgetResolver, InlineWidgetResolver>();
            services.AddScoped<ICtaService, CtaService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IIpLocatorService, IpLocatorService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddSingleton<IEmailRepository, EmailRepository>();
            services.AddSingleton<IMvcEmailService, MvcEmailService>();
            services.AddSingleton<IGenericPageService, GenericPageService>();
            services.AddSingleton<IOnePlaceConnectionService, OnePlaceConnectionService>();
            services.AddSingleton<IOnePlaceDataService, OnePlaceDataService>();
            services.AddSingleton<IOnePlaceAccountService, OnePlaceAccountService>();
            services.AddSingleton<IOnePlaceContactService, OnePlaceContactService>();
            services.AddSingleton<IOnePlaceRelationshipTypeService, OnePlaceRelationshipTypeService>();
            services.AddSingleton<IOnePlaceRelationshipService, OnePlaceRelationshipService>();
            services.AddSingleton<IDataSubmissionService, UserAccountSubmissionService>();
            services.AddSingleton<IDataSubmissionService, UserBoardMembershipSubmissionService>();
            services.AddSingleton<IDataSubmissionService, UserContactSubmissionService>();
            services.AddSingleton<IDataSubmissionServiceFactory, DataSubmissionServiceFactory>(x => new DataSubmissionServiceFactory(x.GetServices<IDataSubmissionService>().ToArray<IDataSubmissionService>()));
            services.AddSingleton<IDataSubmissionQueueItemRepository, DataSubmissionQueueItemRepository>();
            services.AddSingleton<IDataSubmissionQueueService, DataSubmissionQueueService>();
            services.AddSingleton<IOnePlaceUserExportService, OnePlaceUserExportService>();
            services.AddSingleton<IProfileService, ProfileService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMediaLibraryService, MediaLibraryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseKentico();

            app.UseCookiePolicy();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Kentico().MapRoutes();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("The site has not been configured yet.");
                });

                endpoints.MapControllerRoute(
                    name: "sitemap",
                    pattern: "{culture}/sitemap.xml",
                    defaults: new { controller = "Sitemap", action = "Index" },
                    constraints: new
                    {
                        culture = new CultureConstraint(
                            GlobalConstants.Cultures.AllowedCultureCodes,
                            true),
                    }
                    );

                endpoints.MapControllerRoute(
                    name: "MvcRoute",
                    pattern: "{controller}/{action}",
                    defaults: new { controller = HttpErrorsControllerName, action = NotFoundActionName }
                    );

                endpoints.MapControllerRoute(
                    name: "MvcLocalizedRoute",
                    pattern: "{culture}/{controller}/{action}",
                    defaults: new { controller = HttpErrorsControllerName, action = NotFoundActionName }
                    );


            });

            app.UseExceptionHandler("/Error");

            // Set Default Serialization Options
            JsonConvert.DefaultSettings = () =>
                new JsonSerializerSettings
                {
                    // Can add default serialization settings here
                    DateFormatString = Constants.DateTimeFormat.LongDate,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                };

            //register other compoments
            var configurationService =
               CMS.Core.Service.Resolve<IConfigurationService>();

            var rootMediaStoragePath = Configuration.GetValue<string>(GlobalConstants.Config.RootMediaStoragePath);

            OslerAlumniStorageProvider.RegisterProvider(
                rootMediaStoragePath);

        }
    }
}
