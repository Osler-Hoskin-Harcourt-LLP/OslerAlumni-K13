using System;
using System.Web.Http;
using System.Web.Mvc;
using OslerAlumni.Core.Services;
using OslerAlumni.Mvc.Api.Models;
using Swashbuckle.Application;

namespace OslerAlumni.Mvc
{
    public static class SwaggerConfig
    {
        #region "Constants"

        public const string AppTitle = "Osler Alumni API";
        public const string XmlCommentFilePath = @"\bin\OslerAlumni.Mvc.Api.xml";

        #endregion

        #region "Methods"

        public static void Register(
            IDependencyResolver diResolver,
            HttpConfiguration config,
            string apiPrefix)
        {
            var apiConfig = GetApiConfig(diResolver);

            if (!apiConfig.EnableSwagger)
            {
                return;
            }

            var baseSwaggerPath = $"{apiPrefix}/swagger";

            RegisterRoutes(
                config, 
                baseSwaggerPath);
            
            var defaultVersion = GetDefaultVersion(config);

            config?
                .EnableSwagger(
                    $"{baseSwaggerPath}/{{apiVersion}}/docs",
                    c =>
                    {
                        // Use "SingleApiVersion" to describe a single version API. Swagger 2.0 includes an "Info" object to
                        // hold additional metadata for an API. Version and title are required but you can also provide
                        // additional fields by chaining methods off SingleApiVersion.

                        c.SingleApiVersion($"v{defaultVersion}", AppTitle);

                        // In accordance with the built in JsonSerializer, Swashbuckle will, by default, describe enums as integers.
                        // You can change the serializer behavior by configuring the StringToEnumConverter globally or for a given
                        // enum type. Swashbuckle will honor this change out-of-the-box. However, if you use a different
                        // approach to serialize enums as strings, you can also force Swashbuckle to describe them as strings.

                        c.DescribeAllEnumsAsStrings();

                        // In a Swagger 2.0 document, complex types are typically declared globally and referenced by unique
                        // Schema Id. By default, Swashbuckle does NOT use the full type name in Schema Ids. In most cases, this
                        // works well because it prevents the "implementation detail" of type namespaces from leaking into your
                        // Swagger docs and UI. However, if you have multiple types in your API with the same class name, you'll
                        // need to opt out of this behavior to avoid Schema Id conflicts.

                        c.UseFullTypeNameInSchemaIds();

                        // If you annotate Controllers and API Types with
                        // Xml comments (http://msdn.microsoft.com/en-us/library/b2s063f7(v=vs.110).aspx), you can incorporate
                        // those comments into the generated docs and UI. You can enable this by providing the path to one or
                        // more Xml comment files.
                        
                        c.IncludeXmlComments(
                            GetXmlCommentsPath());
                    })
                .EnableSwaggerUi(
                    $"{baseSwaggerPath}/ui/{{*assetPath}}",
                    c =>
                    {
                        // Use the "DocumentTitle" option to change the Document title.
                        // Very helpful when you have multiple Swagger pages open, to tell them apart.

                        c.DocumentTitle(AppTitle);

                        // By default, swagger-ui will validate specs against swagger.io's online validator and display the result
                        // in a badge at the bottom of the page. Use these options to set a different validator URL or to disable the
                        // feature entirely.

                        c.DisableValidator();
                    });
        }

        #endregion

        #region "Helper methods"

        private static ApiConfig GetApiConfig(
            IDependencyResolver diResolver)
        {
            var configurationService = 
                diResolver.GetService<IConfigurationService>();

            return configurationService
                .GetConfig<ApiConfig>();
        }

        private static string GetDefaultVersion(
            HttpConfiguration config)
        {
            // Note: If API versioning is ever enabled,
            // you can use the following to determine the default value dynamically:
            // 
            // return config?.GetApiVersioningOptions()?.DefaultApiVersion?.ToString();

            return "1";
        }

        private static string GetXmlCommentsPath()
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}{XmlCommentFilePath}";
        }

        private static void RegisterRoutes(
            HttpConfiguration config,
            string baseSwaggerPath)
        {
            config?.Routes.MapHttpRoute(
                name: "SwaggerUi",
                routeTemplate: baseSwaggerPath,
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(
                    SwaggerDocsConfig.DefaultRootUrlResolver,
                    $"{baseSwaggerPath}/ui/index")
            );
        }

        #endregion
    }
}
